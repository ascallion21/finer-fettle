﻿using FinerFettle.Web.Data;
using FinerFettle.Web.Models.Exercise;
using FinerFettle.Web.ViewModels.Exercise;
using FinerFettle.Web.ViewModels.Newsletter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinerFettle.Web.Controllers
{
    [Route("exercises")]
    public class ExerciseController : BaseController
    {
        /// <summary>
        /// The name of the controller for routing purposes
        /// </summary>
        public const string Name = "Exercise";

        public ExerciseController(CoreContext context) : base(context) { }

        [Route("all")]
        public async Task<IActionResult> All([Bind("RecoveryMuscle,SportsFocus,OnlyWeights,OnlyCore,EquipmentBinder,ShowFilteredOut,ExerciseType,MuscleContractions")] ExercisesViewModel? viewModel = null)
        {
            viewModel ??= new ExercisesViewModel();
            viewModel.Equipment = await _context.Equipment
                    .Where(e => e.DisabledReason == null)
                    .OrderBy(e => e.Name)
                    .ToListAsync();

            var queryBuilder = new ExerciseQueryBuilder(_context)
                .WithMuscleGroups(MuscleGroups.All)
                .WithOrderBy(ExerciseQueryBuilder.OrderByEnum.Progression);

            if (viewModel.EquipmentBinder.HasValue)
            {
                if (viewModel.EquipmentBinder.Value == 0)
                {
                    queryBuilder = queryBuilder.WithEquipment(new List<int>(0));
                }
                else
                {
                    queryBuilder = queryBuilder.WithEquipment(new List<int>(1) { viewModel.EquipmentBinder.Value });
                }
            }

            if (viewModel.OnlyWeights.HasValue)
            {
                queryBuilder = queryBuilder.WithOnlyWeights(viewModel.OnlyWeights.Value != Models.NoYes.No);
            }

            if (viewModel.OnlyCore.HasValue && !viewModel.ShowFilteredOut)
            {
                queryBuilder = queryBuilder.WithIncludeBonus(viewModel.OnlyCore.Value == Models.NoYes.No);
            }

            if (viewModel.SportsFocus.HasValue && !viewModel.ShowFilteredOut)
            {
                queryBuilder = queryBuilder.WithSportsFocus(viewModel.SportsFocus.Value);
            }

            if (viewModel.ExerciseType.HasValue && !viewModel.ShowFilteredOut)
            {
                queryBuilder = queryBuilder.WithExerciseType(viewModel.ExerciseType.Value);
            }

            if (viewModel.MuscleContractions.HasValue && !viewModel.ShowFilteredOut)
            {
                queryBuilder = queryBuilder.WithMuscleContractions(viewModel.MuscleContractions.Value);
            }

            if (viewModel.RecoveryMuscle.HasValue)
            {
                queryBuilder = queryBuilder.WithRecoveryMuscle(viewModel.RecoveryMuscle.Value, include: true);
            }
            else
            {
                // Otherwise exlude recovery tracks
                queryBuilder = queryBuilder.WithRecoveryMuscle(MuscleGroups.None);
            }

            var allExercises = (await queryBuilder.Query())
                .Select(r => new ExerciseViewModel(r, ExerciseTheme.Main))
                .ToList();

            if (viewModel.SportsFocus.HasValue && viewModel.ShowFilteredOut)
            {
                var temp = Filters.FilterSportsFocus(allExercises.AsQueryable(), viewModel.SportsFocus);
                allExercises.ForEach(e => {
                    if (!temp.Contains(e))
                    {
                        e.Theme = ExerciseTheme.Other;
                    }
                });
            }

            if (viewModel.OnlyCore.HasValue && viewModel.ShowFilteredOut)
            {
                var temp = Filters.FilterIncludeBonus(allExercises.AsQueryable(), viewModel.OnlyCore.Value == Models.NoYes.No);
                allExercises.ForEach(e => {
                    if (!temp.Contains(e))
                    {
                        e.Theme = ExerciseTheme.Other;
                    }
                });
            }

            if (viewModel.ExerciseType.HasValue && viewModel.ShowFilteredOut)
            {
                var temp = Filters.FilterExerciseType(allExercises.AsQueryable(), viewModel.ExerciseType);
                allExercises.ForEach(e => {
                    if (!temp.Contains(e))
                    {
                        e.Theme = ExerciseTheme.Other;
                    }
                });
            }

            if (viewModel.MuscleContractions.HasValue && viewModel.ShowFilteredOut)
            {
                var temp = Filters.FilterMuscleContractions(allExercises.AsQueryable(), viewModel.MuscleContractions);
                allExercises.ForEach(e => {
                    if (!temp.Contains(e))
                    {
                        e.Theme = ExerciseTheme.Other;
                    }
                });
            }

            viewModel.Exercises = allExercises;

            return View(viewModel);
        }

        [Route("check")]
        public async Task<IActionResult> Check()
        {
            var allExercises = (await new ExerciseQueryBuilder(_context, ignoreGlobalQueryFilters: true)
                .WithMuscleGroups(MuscleGroups.All)
                .Query())
                .Select(r => new ExerciseViewModel(r, ExerciseTheme.Main))
                .ToList();

            var strengthExercises = (await new ExerciseQueryBuilder(_context, ignoreGlobalQueryFilters: true)
                .WithMuscleGroups(MuscleGroups.All)
                .WithRecoveryMuscle(MuscleGroups.None)
                .WithExerciseType(ExerciseType.Stability | ExerciseType.Strength)
                .Query())
                .Select(r => new ExerciseViewModel(r, ExerciseTheme.Main))
                .ToList();

            var recoveryExercises = (await new ExerciseQueryBuilder(_context, ignoreGlobalQueryFilters: true)
                .WithMuscleGroups(MuscleGroups.All)
                .WithRecoveryMuscle(MuscleGroups.All, include: true)
                .Query())
                .Select(r => new ExerciseViewModel(r, ExerciseTheme.Main))
                .ToList();

            var warmupCooldownExercises = (await new ExerciseQueryBuilder(_context, ignoreGlobalQueryFilters: true)
                .WithMuscleGroups(MuscleGroups.All)
                .WithExerciseType(ExerciseType.Flexibility | ExerciseType.Cardio)
                .WithPrefersWeights(false)
                .CapAtProficiency(true)
                .Query())
                .Select(r => new ExerciseViewModel(r, ExerciseTheme.Main))
                .ToList();

            var missingExercises = _context.Variations
                .IgnoreQueryFilters()
                .Where(v => v.DisabledReason == null)
                // Left outer join
                .GroupJoin(_context.ExerciseVariations,
                    o => o.Id,
                    i => i.Variation.Id,
                    (o, i) => new { Variation = o, ExerciseVariations = i })
                .SelectMany(
                    oi => oi.ExerciseVariations.DefaultIfEmpty(),
                    (o, i) => new { o.Variation, ExerciseVariation = i })
                .Where(v => v.ExerciseVariation == null)
                .Select(v => v.Variation.Name)
                .ToList();

            var missing100PProgressionRange = allExercises
                .Where(e => e.ExerciseVariation.IsBonus == false)
                .GroupBy(e => e.Exercise.Name)
                .Where(e => e.Min(i => i.ExerciseVariation.Progression.GetMinOrDefault) > 0
                    || e.Max(i => i.ExerciseVariation.Progression.GetMaxOrDefault) < 100)
                .Select(e => e.Key)
                .ToList();

            var emptyDisabledString = allExercises
                .Where(e => e.Exercise.DisabledReason == string.Empty || e.Variation.DisabledReason == string.Empty)
                .Select(e => e.Exercise.Name)
                .ToList();

            var missingRepRange = allExercises
                .Where(e => e.Variation.Intensities.Any(p => (p.Proficiency.MinReps != null && p.Proficiency.MaxReps == null) || (p.Proficiency.MinReps == null && p.Proficiency.MaxReps != null)))
                .Select(e => e.Variation.Name)
                .ToList();

            var missingProficiencyStrength = strengthExercises
                .Where(e => e.Variation.Intensities.All(p =>
                    p.IntensityLevel != IntensityLevel.Maintain
                    && p.IntensityLevel != IntensityLevel.Obtain
                    && p.IntensityLevel != IntensityLevel.Gain
                    //&& p.IntensityLevel != IntensityLevel.Endurance
                ))
                .Select(e => e.Variation.Name)
                .ToList();

            var missingProficiencyRecovery = recoveryExercises
                .Where(e => e.Variation.Intensities.All(p =>
                    p.IntensityLevel != IntensityLevel.Recovery
                ))
                .Select(e => e.Variation.Name)
                .ToList();

            var missingProficiencyWarmupCooldown = warmupCooldownExercises
                .Where(e => e.Variation.Intensities.All(p =>
                    p.IntensityLevel != IntensityLevel.WarmupCooldown
                ))
                .Select(e => e.Variation.Name)
                .ToList();

            var viewModel = new CheckViewModel()
            {
                Missing100PProgressionRange = missing100PProgressionRange,
                MissingProficiencyStrength = missingProficiencyStrength,
                MissingProficiencyRecovery = missingProficiencyRecovery,
                MissingProficiencyWarmupCooldown = missingProficiencyWarmupCooldown,
                MissingRepRange = missingRepRange,
                EmptyDisabledString = emptyDisabledString,
                MissingExercises = missingExercises
            };

            return View(viewModel);
        }
    }
}
