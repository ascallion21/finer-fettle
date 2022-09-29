﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinerFettle.Web.Data;
using FinerFettle.Web.Models.User;
using FinerFettle.Web.Models.Exercise;
using FinerFettle.Web.ViewModels.Newsletter;
using FinerFettle.Web.Extensions;
using FinerFettle.Web.Models.Newsletter;
using System.Numerics;

namespace FinerFettle.Web.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly CoreContext _context;

        /// <summary>
        /// The name of the controller for routing purposes
        /// </summary>
        public const string Name = "Newsletter";

        public NewsletterController(CoreContext context)
        {
            _context = context;
        }

        [Route("newsletter/{email}")]
        public async Task<IActionResult> Newsletter(string email, bool demo = false)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            var user = await _context.Users
                .Include(u => u.ExerciseProgressions)
                .Include(u => u.EquipmentUsers)
                    .ThenInclude(u => u.Equipment)
                .FirstAsync(u => u.Email == email);
                
            if (user.NeedsRest)
            {
                user.NeedsRest = false;
                _context.Update(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }

            if (user.RestDays.HasFlag(RestDaysExtensions.FromDate(today)) == true)
            {
                return NoContent();
            }

            var todoExerciseType = new ExerciseTypeGroups(user.StrengtheningPreference).First(); // Have to start somewhere
            
            var previousNewsletter = await _context.Newsletters
                .Where(n => n.User == user)
                .OrderBy(n => n.Date)
                .ThenBy(n => n.Id) // For testing/demo. When two newsletters get sent in the same day, I want a different exercise set.
                .LastOrDefaultAsync();

            if (previousNewsletter != null)
            {
                todoExerciseType = new ExerciseTypeGroups(user.StrengtheningPreference)
                    .SkipWhile(r => r != previousNewsletter.ExerciseRotation)
                    .Skip(1)
                    .FirstOrDefault() ?? todoExerciseType;
            }

            var lastDeload = await _context.Newsletters
                .Where(n => n.User == user)
                .OrderBy(n => n.Date)
                .ThenBy(n => n.Id) // For testing/demo. When two newsletters get sent in the same day, I want a different exercise set.
                .LastOrDefaultAsync(n => n.IsDeloadWeek) 
                    ?? await _context.Newsletters
                    .Where(n => n.User == user)
                    .OrderBy(n => n.Date)
                    .ThenBy(n => n.Id) // For testing/demo. When two newsletters get sent in the same day, I want a different exercise set.
                    .FirstOrDefaultAsync(); // The oldest newsletter, for if there has never been a deload before.

            bool needsDeload = lastDeload != null 
                && ( 
                    // Dates are the same week. Keep the deload going until the week is over.
                    (lastDeload.IsDeloadWeek && lastDeload.Date.AddDays(-1 * (int)lastDeload.Date.DayOfWeek) == today.AddDays(-1 * (int)today.DayOfWeek))
                    // Or the last deload/oldest newsletter was 1+ months ago
                    || lastDeload.Date.AddMonths(1) < today 
                );

            var newsletter = new Newsletter()
            {
                IsDeloadWeek = needsDeload,
                Date = today,
                User = user,
                ExerciseRotation = todoExerciseType
            };
            _context.Newsletters.Add(newsletter);
            await _context.SaveChangesAsync();

            // Flatten all exercise variations and intensities into one big list
            var allExercises = await _context.Intensities
                .Include(v => v.Variation)
                    .ThenInclude(e => e.Exercise)
                        .ThenInclude(e => e.UserProgressions)
                .Include(i => i.EquipmentGroups)
                    .ThenInclude(eg => eg.Equipment)
                .Where(i => i.Variation.DisabledReason == null)
                // Select the current progression of each exercise.
                // Using averageProgression as a boost so that users can't get stuck without an exercise if they never see it because they are under the exercise's min progression
                .Where(i => i.Progression.Min == null
                                // User hasn't ever seen this exercise before. Show it so an ExerciseUserProgression record is made.
                                || (i.Variation.Exercise.UserProgressions.FirstOrDefault(up => up.User == user) == null
                                    && (5 * (int)Math.Floor(user.AverageProgression / 5d) >= i.Progression.Min))
                                // Compare the exercise's progression range with the average of the user's average progression and the user's exercise progression
                                || (5 * (int)Math.Floor((user.AverageProgression + i.Variation.Exercise.UserProgressions.First(up => up.User == user).Progression) / 10d)) >= i.Progression.Min)
                .Where(i => i.Progression.Max == null
                                // User hasn't ever seen this exercise before. Show it so an ExerciseUserProgression record is made.
                                || (i.Variation.Exercise.UserProgressions.FirstOrDefault(up => up.User == user) == null
                                    && (5 * (int)Math.Ceiling(user.AverageProgression / 5d) < i.Progression.Max))
                                // Compare the exercise's progression range with the average of the user's average progression and the user's exercise progression
                                || (5 * (int)Math.Ceiling((user.AverageProgression + i.Variation.Exercise.UserProgressions.First(up => up.User == user).Progression) / 10d)) < i.Progression.Max)
                .Where(i => (
                        // User owns at least one equipment in at least one of the optional equipment groups
                        !i.EquipmentGroups.Any(eg => !eg.Required && eg.Equipment.Any())
                        || i.EquipmentGroups.Where(eg => !eg.Required && eg.Equipment.Any()).Any(eg => eg.Equipment.Any(e => user.EquipmentIds.Contains(e.Id)))
                    ) && (
                        // User owns at least one equipment in all of the required equipment groups
                        !i.EquipmentGroups.Any(eg => eg.Required && eg.Equipment.Any())
                        || i.EquipmentGroups.Where(eg => eg.Required && eg.Equipment.Any()).All(eg => eg.Equipment.Any(e => user.EquipmentIds.Contains(e.Id)))
                    ))
                .Select(i => new ExerciseViewModel(user, i.Variation.Exercise, i.Variation, i)
                {
                    Demo = demo
                })
                .ToListAsync();

            // Select a random subset of exercises
            allExercises.Shuffle(); // Randomizing in the SQL query produces duplicate rows for some reason.

            // Main exercises
            var mainExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Main)
                .Where(vm => todoExerciseType.ExerciseType.HasAnyFlag32(vm.Variation.ExerciseType));
            var exercises = mainExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Choose either compound exercises that cover at least two muscles in the targeted muscles set
                    (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2))
                    || (
                        // Or isolation exercises that cover a unique muscle group.
                        BitOperations.PopCount((ulong)vm.Exercise.Muscles) == 1 
                        && vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    )
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms);
            exercises = exercises.Concat(mainExercises.Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab any muscle groups we missed in the previous aggregate
                    vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate(exercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles), (m, vm2) => m | vm2.Exercise.Muscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms)).ToList();

            foreach (var exercise in exercises)
            {
                // If this exercise is weighted
                if (exercise.Intensity.EquipmentGroups.Any(eg => eg.IsWeight))
                {
                    if (!newsletter.IsDeloadWeek)
                    {
                        // Each day works part of the body, not the full body. Work each muscle harder.
                        exercise.Intensity.Proficiency.Sets += (int)user.StrengtheningPreference;
                    }

                    if (todoExerciseType.ExerciseType.HasFlag(ExerciseType.Strength))
                    {
                        // When gaining muscle, work less reps at higher weights.
                        // Aiming for 18 reps for maintain, 12 for obtain, and 6 for gain.
                        exercise.Intensity.Proficiency.Reps -= (int)user.StrengtheningPreference * 6;
                    }
                }
            }

            var viewModel = new NewsletterViewModel(exercises, user, newsletter)
            {
                ExerciseType = todoExerciseType.ExerciseType,
                MuscleGroups = todoExerciseType.MuscleGroups,
                Demo = demo
            };

            // Warmup exercises
            var warmupExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Warmup);
            viewModel.WarmupExercises = warmupExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab compound exercises that cover at least two muscles in the targeted muscles set
                    BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2)
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms).ToList();
            viewModel.WarmupExercises = viewModel.WarmupExercises
                .Concat(warmupExercises
                    .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                        // Grab any muscle groups we missed in the previous aggregate
                        vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate(viewModel.WarmupExercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles), (m, vm2) => m | vm2.Exercise.Muscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms))
                .ToList();
                
            // Cooldown exercises
            var cooldownExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Cooldown);
            var temp = cooldownExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab compound exercises that cover at least two muscles in the targeted muscles set
                    BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2)
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms);
            viewModel.CooldownExercises = temp;
            viewModel.CooldownExercises = viewModel.CooldownExercises
                .Concat(cooldownExercises
                    .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                        // Grab any muscle groups we missed in the previous aggregate
                        vm.Exercise.Muscles.UnsetFlag32(vms.Aggregate(viewModel.CooldownExercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.Muscles), (m, vm2) => m | vm2.Exercise.Muscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms))
                .ToList();

            return View(nameof(Newsletter), viewModel);
        }
    }
}
