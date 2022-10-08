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
                    .ThenInclude(ep => ep.Exercise)
                .Include(u => u.EquipmentUsers)
                    .ThenInclude(u => u.Equipment)
                .FirstAsync(u => u.Email == email);
                
            if (user.Disabled || user.RestDays.HasFlag(RestDaysExtensions.FromDate(today)))
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
            var allExercises = (await _context.Intensities
                .Include(v => v.UserIntensities)
                .Include(i => i.IntensityPreferences)
                .Include(i => i.EquipmentGroups)
                    .ThenInclude(eg => eg.Equipment)
                .Include(v => v.Variation)
                    .ThenInclude(e => e.Exercise)
                        .ThenInclude(e => e.UserProgressions)
                .Include(v => v.Variation)
                    .ThenInclude(e => e.Exercise)
                        .ThenInclude(e => e.Prerequisites)
                // Select the current progression of each exercise.
                .Select(i => new {
                    Intensity = i,
                    UserIntensity = i.UserIntensities.FirstOrDefault(ui => ui.User == user),
                    UserProgression = i.Variation.Exercise.UserProgressions.FirstOrDefault(up => up.User == user)
                })
                // Don't grab exercises that the user wants to ignore
                .Where(i => i.UserProgression == null || !i.UserProgression.Ignore)
                // Only show these exercises if the user has completed the previous reqs
                .Where(i => i.Intensity.Variation.Exercise.Prerequisites
                                .Select(r => new { r.PrerequisiteExercise.Proficiency, UserProgression = r.PrerequisiteExercise.UserProgressions.FirstOrDefault(up => up.User == user) })
                                .All(p => p.UserProgression == null || p.UserProgression.Ignore || p.UserProgression.Progression >= p.Proficiency)
                )
                // Using averageProgression as a boost so that users can't get stuck without an exercise if they never see it because they are under the exercise's min progression
                .Where(i => i.Intensity.Progression.Min == null
                                // User hasn't ever seen this exercise before. Show it so an ExerciseUserProgression record is made.
                                || (i.UserProgression == null && (5 * (int)Math.Floor(user.AverageProgression / 5d) >= i.Intensity.Progression.Min))
                                // Compare the exercise's progression range with the average of the user's average progression and the user's exercise progression
                                || (i.UserProgression != null && (5 * (int)Math.Floor((user.AverageProgression + i.UserProgression!.Progression) / 10d)) >= i.Intensity.Progression.Min))
                .Where(i => i.Intensity.Progression.Max == null
                                // User hasn't ever seen this exercise before. Show it so an ExerciseUserProgression record is made.
                                || (i.UserProgression == null && (5 * (int)Math.Ceiling(user.AverageProgression / 5d) < i.Intensity.Progression.Max))
                                // Compare the exercise's progression range with the average of the user's average progression and the user's exercise progression
                                || (i.UserProgression != null && (5 * (int)Math.Ceiling((user.AverageProgression + i.UserProgression!.Progression) / 10d)) < i.Intensity.Progression.Max))
                .Where(i => (
                        // User owns at least one equipment in at least one of the optional equipment groups
                        !i.Intensity.EquipmentGroups.Any(eg => !eg.Required && eg.Equipment.Any())
                        || i.Intensity.EquipmentGroups.Where(eg => !eg.Required && eg.Equipment.Any()).Any(eg => eg.Equipment.Any(e => user.EquipmentIds.Contains(e.Id)))
                    ) && (
                        // User owns at least one equipment in all of the required equipment groups
                        !i.Intensity.EquipmentGroups.Any(eg => eg.Required && eg.Equipment.Any())
                        || i.Intensity.EquipmentGroups.Where(eg => eg.Required && eg.Equipment.Any()).All(eg => eg.Equipment.Any(e => user.EquipmentIds.Contains(e.Id)))
                    ))
                .ToListAsync()).Select(i => new ExerciseViewModel(user, i.Intensity.Variation.Exercise, i.Intensity.Variation, i.Intensity)
                {
                    Demo = demo,
                    UserProgression = i.UserProgression
                }).ToList();

            // Select a random subset of exercises
            allExercises.Shuffle(); // Randomizing in the SQL query produces duplicate rows

            // Main exercises
            var mainExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Main)
                .Where(vm => todoExerciseType.ExerciseType.HasAnyFlag32(vm.Variation.ExerciseType))
                // If a recovery muscle is set, don't choose any exercises that work the injured muscle
                .Where(i => user.RecoveryMuscle == MuscleGroups.None || !i.Exercise.AllMuscles.HasFlag(user.RecoveryMuscle))
                // Select one variation per exercise/intensity
                .GroupBy(i => new { i.Intensity.Variation.Exercise.Id, i.Intensity.IntensityLevel })
                .Select(g => new
                {
                    g.Key,
                    // If there are weighted variations of an exercise, show that before the bodyweight variations if the user wills it
                    GroupOfOne = g.OrderByDescending(a => user.PrefersWeights && a.Intensity.EquipmentGroups.Any(eg => eg.IsWeight))
                        // Show variations that the user has rarely seen
                        .ThenBy(a => a.UserProgression?.SeenCount ?? 0)
                        .Take(1)
                })
                .Select(g => g.GroupOfOne.First())
                // Show exercises that the user has rarely seen
                .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0);
            var exercises = mainExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Choose either compound exercises that cover at least two muscles in the targeted muscles set
                    BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.PrimaryMuscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.PrimaryMuscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2)
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms);
            exercises = exercises.Concat(mainExercises.Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab any muscle groups we missed in the previous aggregate. Include isolation exercises here
                    vm.Exercise.PrimaryMuscles.UnsetFlag32(vms.Aggregate(exercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.PrimaryMuscles), (m, vm2) => m | vm2.Exercise.PrimaryMuscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms))
                // Show most complex exercises first
                .OrderByDescending(e => BitOperations.PopCount((ulong)e.Exercise.PrimaryMuscles))
                .ToList();

            var viewModel = new NewsletterViewModel(exercises, user, newsletter)
            {
                ExerciseType = todoExerciseType.ExerciseType,
                MuscleGroups = todoExerciseType.MuscleGroups,
                AllEquipment = new EquipmentViewModel(_context.Equipment, user.EquipmentUsers.Select(eu => eu.Equipment)),
                Demo = demo
            };

            // Warmup exercises
            var warmupExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Warmup)
                // If a recovery muscle is set, don't choose any exercises that work the injured muscle
                .Where(i => user.RecoveryMuscle == MuscleGroups.None || !i.Exercise.AllMuscles.HasFlag(user.RecoveryMuscle))
                // Show exercises that the user has rarely seen
                .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0)
                .ToList();
            var item = warmupExercises.FirstOrDefault(e => e.Variation.ExerciseType.HasFlag(ExerciseType.Cardio) && !e.Intensity.MuscleContractions.HasFlag(MuscleContractions.Isometric));
            if (item != null)
            {
                // Need something to get the heart rate up
                warmupExercises.Remove(item);
                warmupExercises.Insert(0, item);
            }
            viewModel.WarmupExercises = warmupExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab compound exercises that cover at least two muscles in the targeted muscles set
                    BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.PrimaryMuscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.PrimaryMuscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2)
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms).ToList();
            viewModel.WarmupExercises = viewModel.WarmupExercises
                .Concat(warmupExercises
                    .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                        // Grab any muscle groups we missed in the previous aggregate
                        vm.Exercise.PrimaryMuscles.UnsetFlag32(vms.Aggregate(viewModel.WarmupExercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.PrimaryMuscles), (m, vm2) => m | vm2.Exercise.PrimaryMuscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms))
                // Move the exercises that get the heart rate up to the end
                .OrderBy(e => e.Variation.ExerciseType.HasFlag(ExerciseType.Cardio))
                .ToList();

            // Recovery exercises
            if (user.RecoveryMuscle != MuscleGroups.None)
            {
                var recoveryVariations = new List<ExerciseViewModel>();
                recoveryVariations.AddRange(allExercises.Where(e => e.Intensity.IntensityPreferences.Any(ip => ip.StrengtheningPreference == StrengtheningPreference.Recovery)).Select(e => new ExerciseViewModel(e)
                {
                    IntensityPreference = new ProficiencyViewModel(e.Intensity, StrengtheningPreference.Recovery)
                }));
                viewModel.RecoveryExercises = recoveryVariations
                    .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Warmup)
                    // Choose recovery exercises that work the recovery muscle
                    .Where(i => i.Intensity.Variation.Exercise.PrimaryMuscles.HasFlag(user.RecoveryMuscle))
                    // Show exercises that the user has rarely seen
                    .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0)
                    .Take(1)
                    // Show (guessing) easier exercises first
                    .OrderBy(vm => vm.Intensity.Progression.Min ?? 0)
                    .Concat(recoveryVariations
                        .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Main)
                        // Choose recovery exercises that work the recovery muscle
                        .Where(i => i.Intensity.Variation.Exercise.PrimaryMuscles.HasFlag(user.RecoveryMuscle))
                        // Show exercises that the user has rarely seen
                        .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0)
                        .Take(1)
                        // Show (guessing) easier exercises first
                        .OrderBy(vm => vm.Intensity.Progression.Min ?? 0))
                    .Concat(recoveryVariations
                        .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Cooldown)
                        // Choose recovery exercises that work the recovery muscle
                        .Where(i => i.Intensity.Variation.Exercise.PrimaryMuscles.HasFlag(user.RecoveryMuscle))
                        // Show exercises that the user has rarely seen
                        .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0)
                        .Take(1)
                        // Show (guessing) easier exercises first
                        .OrderBy(vm => vm.Intensity.Progression.Min ?? 0))
                    .ToList();
            }

            // Sports exercises
            if (user.SportsFocus != SportsFocus.None && !newsletter.IsDeloadWeek)
            {
                // TODO Grab the gain intensity preference on strengthening days and the endurance intensity preference on cardio days
                var enduranceVariations = new List<ExerciseViewModel>();
                enduranceVariations.AddRange(allExercises.Where(e => e.Intensity.IntensityPreferences.Any(ip => ip.StrengtheningPreference == StrengtheningPreference.Endurance)).Select(e => new ExerciseViewModel(e)
                {
                    IntensityPreference = new ProficiencyViewModel(e.Intensity, StrengtheningPreference.Endurance)
                }));
                viewModel.SportsExercises = allExercises.Concat(enduranceVariations)
                    .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Main)
                    .Where(vm => todoExerciseType.ExerciseType.HasAnyFlag32(vm.Variation.ExerciseType))
                    // Choose recovery exercises that work the sports muscle
                    .Where(i => i.Intensity.Variation.SportsFocus.HasFlag(user.SportsFocus))
                    // Show exercises that the user has rarely seen
                    .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0)
                    .Take(3)
                    // Show most complex exercises first
                    .OrderByDescending(e => BitOperations.PopCount((ulong)e.Exercise.PrimaryMuscles))
                    // Show endurance before strength, group intensities together
                    .ThenBy(vm => vm.Intensity.Id)
                    .ThenByDescending(vm => vm.IntensityPreference.StrengtheningPreference)
                    .ToList();
            }

            // Cooldown exercises
            var cooldownExercises = allExercises
                .Where(vm => vm.ActivityLevel == ExerciseActivityLevel.Cooldown)
                // If a recovery muscle is set, don't choose any exercises that work the injured muscle
                .Where(i => user.RecoveryMuscle == MuscleGroups.None || !i.Exercise.AllMuscles.HasFlag(user.RecoveryMuscle))
                // Show exercises that the user has rarely seen
                .OrderBy(vm => vm.UserProgression?.SeenCount ?? 0);
            viewModel.CooldownExercises = cooldownExercises
                .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                    // Grab compound exercises that cover at least two muscles in the targeted muscles set
                    BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups.UnsetFlag32(vm.Exercise.AllMuscles.UnsetFlag32(vms.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.AllMuscles)))) <= (BitOperations.PopCount((ulong)todoExerciseType.MuscleGroups) - 2)
                ) ? new List<ExerciseViewModel>(vms) { vm } : vms);
            viewModel.CooldownExercises = viewModel.CooldownExercises
                .Concat(cooldownExercises
                    .Aggregate(new List<ExerciseViewModel>(), (vms, vm) => (
                        // Grab any muscle groups we missed in the previous aggregate
                        vm.Exercise.AllMuscles.UnsetFlag32(vms.Aggregate(viewModel.CooldownExercises.Aggregate((MuscleGroups)0, (m, vm2) => m | vm2.Exercise.AllMuscles), (m, vm2) => m | vm2.Exercise.AllMuscles)).HasAnyFlag32(todoExerciseType.MuscleGroups)
                    ) ? new List<ExerciseViewModel>(vms) { vm } : vms))
                .ToList();

            return View(nameof(Newsletter), viewModel);
        }
    }
}
