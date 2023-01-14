﻿using Web.Entities.Exercise;
using Web.Entities.User;
using Web.Models.Exercise;
using Web.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Numerics;
using static Web.Data.QueryBuilder.ExerciseQueryBuilder;
using Web.Code.Extensions;

namespace Web.Data.QueryBuilder;

public class ExerciseQueryer
{
    [DebuggerDisplay("{Exercise}: {Variation}")]
    public record QueryResults(
        User? User,
        Exercise Exercise,
        Variation Variation,
        ExerciseVariation ExerciseVariation,
        UserExercise? UserExercise,
        UserExerciseVariation? UserExerciseVariation,
        UserVariation? UserVariation,
        Variation? EasierVariation,
        Variation? HarderVariation
    ) : IExerciseVariationCombo;

    [DebuggerDisplay("{Exercise}: {Variation}")]
    private class InProgressQueryResults :
        IExerciseVariationCombo
    {
        public Exercise Exercise { get; init; } = null!;
        public Variation Variation { get; init; } = null!;
        public ExerciseVariation ExerciseVariation { get; init; } = null!;
        public UserExercise? UserExercise { get; init; }
        public UserExerciseVariation? UserExerciseVariation { get; init; }
        public UserVariation? UserVariation { get; init; }
        public Variation? HarderVariation { get; init; }
        public Variation? EasierVariation { get; init; }
        public bool IsMaxProgressionInRange { get; init; }
    }

    public readonly CoreContext Context;
    public readonly bool IgnoreGlobalQueryFilters = false;

    public required User? User;

    public required ExerciseType? ExerciseType;
    public required MuscleGroups? RecoveryMuscle;
    public required MuscleGroups MusclesAlreadyWorked = MuscleGroups.None;
    public required MuscleContractions? MuscleContractions;
    public required MuscleMovement? MuscleMovement;
    public required OrderByEnum OrderBy = OrderByEnum.None;
    public required SportsFocus? SportsFocus;
    public required int SkipCount = 0;
    public required bool? Unilateral = null;
    public required bool? AntiGravity = null;
    public required IEnumerable<int>? EquipmentIds;

    public required ExclusionOptions ExclusionOptions { get; init; }
    public required BonusOptions BonusOptions { get; init; }
    public required ProficiencyOptions Proficiency { get; init; }
    public required MovementPatternOptions MovementPattern { get; init; }
    public required MuscleGroupOptions MuscleGroup { get; init; }
    public required WeightOptions WeightOptions { get; init; }

    public ExerciseQueryer(CoreContext context, bool ignoreGlobalQueryFilters = false)
    {
        Context = context;
        IgnoreGlobalQueryFilters = ignoreGlobalQueryFilters;
    }


    /// <summary>
    /// Queries the db for the data
    /// </summary>
    public async Task<IList<QueryResults>> Query()
    {
        var eligibleExercisesQuery = Context.Exercises
            .Include(e => e.Prerequisites) // TODO Only necessary for the /exercises list, not the newsletter
                .ThenInclude(p => p.PrerequisiteExercise)
            .Select(i => new
            {
                Exercise = i,
                UserExercise = i.UserExercises.FirstOrDefault(ue => ue.User == User)
            })
            // Don't grab exercises that the user wants to ignore
            .Where(i => i.UserExercise == null || !i.UserExercise.Ignore)
            // Only show these exercises if the user has completed the previous reqs
            .Where(i => i.Exercise.Prerequisites
                    .Select(r => new
                    {
                        r.PrerequisiteExercise.Proficiency,
                        UserExercise = r.PrerequisiteExercise.UserExercises.FirstOrDefault(up => up.User == User)
                    })
                    .All(p => User == null
                        || /* Require the prerequisites show first */ p.UserExercise != null
                            && (p.UserExercise.Ignore || p.UserExercise.Progression >= p.Proficiency)
                    )
            );

        var baseQuery = Context.Variations
            .AsNoTracking() // Don't update any entity
            .Include(i => i.Intensities)
            // If OnlyWeights is false, filter down the included equipment groups to only those not using any weight
            .Include(i => i.EquipmentGroups.Where(eg => eg.Parent == null).Where(eg => WeightOptions.OnlyWeights != false || !eg.IsWeight && (!eg.Children.Any() || eg.Children.Any(c => !c.IsWeight) || eg.Instruction != null)))
                // To display the equipment required for the exercise in the newsletter
                .ThenInclude(eg => eg.Equipment.Where(e => e.DisabledReason == null))
            .Include(i => i.EquipmentGroups.Where(eg => eg.Parent == null).Where(eg => WeightOptions.OnlyWeights != false || !eg.IsWeight && (!eg.Children.Any() || eg.Children.Any(c => !c.IsWeight) || eg.Instruction != null)))
                .ThenInclude(eg => eg.Children)
                    // To display the equipment required for the exercise in the newsletter
                    .ThenInclude(eg => eg.Equipment.Where(e => e.DisabledReason == null))
            .Include(i => i.EquipmentGroups.Where(eg => eg.Parent == null).Where(eg => WeightOptions.OnlyWeights != false || !eg.IsWeight && (!eg.Children.Any() || eg.Children.Any(c => !c.IsWeight) || eg.Instruction != null)))
            .Include(i => i.EquipmentGroups.Where(eg => eg.Parent == null).Where(eg => WeightOptions.OnlyWeights != false || !eg.IsWeight && (!eg.Children.Any() || eg.Children.Any(c => !c.IsWeight) || eg.Instruction != null)))
            .ThenInclude(eg => eg.Children)
            .Join(Context.ExerciseVariations, o => o.Id, i => i.Variation.Id, (o, i) => new
            {
                Variation = o,
                ExerciseVariation = i
            })
            .Join(eligibleExercisesQuery, o => o.ExerciseVariation.Exercise.Id, i => i.Exercise.Id, (o, i) => new
            {
                o.Variation,
                o.ExerciseVariation,
                i.Exercise,
                i.UserExercise
            })
            .Where(vm => !ExclusionOptions.ExerciseIds.Contains(vm.Exercise.Id))
            .Where(vm => !ExclusionOptions.VariationIds.Contains(vm.Variation.Id))
            .Where(vm => !Proficiency.DoCapAtProficiency || vm.ExerciseVariation.Progression.Min == null || vm.ExerciseVariation.Progression.Min <= vm.ExerciseVariation.Exercise.Proficiency)
            .Where(vm => Proficiency.CapAtUsersProficiencyPercent == null || vm.ExerciseVariation.Progression.Min == null || vm.UserExercise == null || vm.ExerciseVariation.Progression.Min <= (vm.UserExercise.Progression * Proficiency.CapAtUsersProficiencyPercent))
            .Select(a => new InProgressQueryResults()
            {
                UserExercise = a.UserExercise,
                UserVariation = a.Variation.UserVariations.FirstOrDefault(uv => uv.User == User),
                UserExerciseVariation = a.ExerciseVariation.UserExerciseVariations.FirstOrDefault(uv => uv.User == User),
                Exercise = a.Exercise,
                Variation = a.Variation,
                ExerciseVariation = a.ExerciseVariation,
                EasierVariation = Context.ExerciseVariations
                    .Where(ev => ev.ExerciseId == a.Exercise.Id)
                    .OrderByDescending(ev => ev.Progression.Max)
                    .First(ev => ev.Progression.Max != null && ev != a.ExerciseVariation && ev.Progression.Max <= (a.UserExercise == null ? UserExercise.MinUserProgression : a.UserExercise.Progression))
                    .Variation,
                HarderVariation = Context.ExerciseVariations
                    .Where(ev => ev.ExerciseId == a.Exercise.Id)
                    .OrderBy(ev => ev.Progression.Min)
                    .First(ev => ev.Progression.Min != null && ev != a.ExerciseVariation && ev.Progression.Min > (a.UserExercise == null ? UserExercise.MinUserProgression : a.UserExercise.Progression))
                    .Variation,
                IsMaxProgressionInRange = User != null && (
                    a.ExerciseVariation.Progression.Max == null
                    // User hasn't ever seen this exercise before. Show it so an ExerciseUserExercise record is made.
                    || a.UserExercise == null && UserExercise.MinUserProgression < a.ExerciseVariation.Progression.Max
                    // Compare the exercise's progression range with the user's exercise progression
                    || a.UserExercise != null && a.UserExercise!.Progression < a.ExerciseVariation.Progression.Max
                )
            });

        if (IgnoreGlobalQueryFilters)
        {
            baseQuery = baseQuery.IgnoreQueryFilters();
        }

        if (User != null)
        {
            baseQuery = baseQuery.Where(i => i.ExerciseVariation.Progression.Min == null
                            // User hasn't ever seen this exercise before. Show it so an ExerciseUserExercise record is made.
                            || i.UserExercise == null && UserExercise.MinUserProgression >= i.ExerciseVariation.Progression.Min
                            // Compare the exercise's progression range with the user's exercise progression
                            || i.UserExercise != null && i.UserExercise!.Progression >= i.ExerciseVariation.Progression.Min);

            baseQuery = baseQuery.Where(i =>
                            // User owns at least one equipment in at least one of the optional equipment groups
                            i.Variation.EquipmentGroups.Any(eg => !eg.Equipment.Any())
                            || i.Variation.EquipmentGroups.Where(eg => eg.Equipment.Any()).Any(peg =>
                                peg.Equipment.Any(e => User.EquipmentIds.Contains(e.Id))
                                && (
                                    !peg.Children.Any()
                                    || peg.Instruction != null // Exercise can be done without child equipment
                                    || peg.Children.Any(ceg => ceg.Equipment.Any(e => User.EquipmentIds.Contains(e.Id)))
                                )
                            )
                        );
        }

        baseQuery = Filters.FilterMovementPattern(baseQuery, MovementPattern.MovementPatterns);
        baseQuery = Filters.FilterMuscleGroup(baseQuery, MuscleGroup.MuscleGroups, include: true, MuscleGroup.MuscleTarget);
        baseQuery = Filters.FilterMuscleGroup(baseQuery, MuscleGroup.ExcludeMuscleGroups, include: false, v => v.Variation.StrengthMuscles | v.Variation.StabilityMuscles);
        baseQuery = Filters.FilterEquipmentIds(baseQuery, EquipmentIds);
        baseQuery = Filters.FilterRecoveryMuscle(baseQuery, RecoveryMuscle);
        baseQuery = Filters.FilterSportsFocus(baseQuery, SportsFocus);
        baseQuery = Filters.FilterIncludeBonus(baseQuery, BonusOptions.Bonus, BonusOptions.OnlyBonus);
        baseQuery = Filters.FilterAntiGravity(baseQuery, AntiGravity);
        baseQuery = Filters.FilterMuscleContractions(baseQuery, MuscleContractions);
        baseQuery = Filters.FilterMuscleMovement(baseQuery, MuscleMovement);
        baseQuery = Filters.FilterExerciseType(baseQuery, ExerciseType);
        baseQuery = Filters.FilterIsUnilateral(baseQuery, Unilateral);
        baseQuery = Filters.FilterOnlyWeights(baseQuery, WeightOptions.OnlyWeights);        

        var queryResults = (await baseQuery.ToListAsync()).AsEnumerable();

        if (User != null)
        {
            // Try choosing variations that have a max progression above the user's progression. Fallback to an easier variation if one does not exist.
            queryResults = queryResults.GroupBy(i => new { i.Exercise.Id })
                                .Select(g => new
                                {
                                    g.Key,
                                    // If there is no variation in the max user progression range (say, if the harder variation requires weights), take the next easiest variation
                                    Variations = g.Where(a => a.IsMaxProgressionInRange).NullIfEmpty() 
                                        ?? g.Where(a => !a.IsMaxProgressionInRange && Proficiency.AllowLesserProgressions)
                                            .OrderByDescending(a => a.ExerciseVariation.Progression.GetMaxOrDefault)
                                            .Take(1) // FIXME? If two variations have the same max proficiency, should we select both?
                                })
                                .SelectMany(g => g.Variations);
        }

        // OrderBy must come after query or you get duplicates.
        // No longer ordering by weighted exercises, since that was to prioritize free weights over advanced calisthenics.
        // Now all advanced calisthenics shoulsd be bonus exercises.
        var orderedResults = queryResults
            // Show exercises that the user has rarely seen
            .OrderBy(a => a.UserExercise == null ? DateOnly.MinValue : a.UserExercise.LastSeen)
            // Show variations that the user has rarely seen
            .ThenBy(a => a.UserExerciseVariation == null ? DateOnly.MinValue : a.UserExerciseVariation.LastSeen)
            // Mostly for the demo, show mostly random exercises
            .ThenBy(a => Guid.NewGuid());

        var muscleTarget = MuscleGroup.MuscleTarget.Compile();
        var finalResults = new List<QueryResults>();
        if (MuscleGroup.AtLeastXUniqueMusclesPerExercise != null)
        {
            // Yikes
            while (MuscleGroup.AtLeastXUniqueMusclesPerExercise > 1)
            {
                if (OrderBy == OrderByEnum.UniqueMuscles)
                {
                    for (var i = 0; i < orderedResults.Count(); i++)
                    {
                        var musclesWorkedSoFar = finalResults.WorkedMuscles(addition: MusclesAlreadyWorked, muscleTarget: muscleTarget);
                        var stack = orderedResults
                            // The variation works at least x unworked muscles. `Where` preserves order.
                            .Where(vm => BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups.UnsetFlag32(muscleTarget(vm).UnsetFlag32(musclesWorkedSoFar))) <= BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups) - MuscleGroup.AtLeastXUniqueMusclesPerExercise)
                            // Order by how many unique primary muscles the exercise works. After the least seen exercises, choose the optimal routine
                            .OrderBy(vm => /*least seen:*/ i < SkipCount ? 0 : BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups.UnsetFlag32(muscleTarget(vm).UnsetFlag32(musclesWorkedSoFar))))
                            .ToList();

                        var exercise = stack.SkipWhile(e =>
                            // Ignore two variations from the same exercise, or two of the same variation
                            finalResults.Select(r => r.Exercise).Contains(e.Exercise)
                            // Two variations work the same muscles, ignore those
                            || finalResults.Any(fr => muscleTarget(fr) == muscleTarget(e))
                        ).FirstOrDefault();

                        if (exercise != null)
                        {
                            finalResults.Add(new QueryResults(User, exercise.Exercise, exercise.Variation, exercise.ExerciseVariation, exercise.UserExercise, exercise.UserExerciseVariation, exercise.UserVariation, exercise.EasierVariation, exercise.HarderVariation));
                        }
                    }
                }
                else
                {
                    foreach (var exercise in orderedResults)
                    {
                        if (finalResults.Select(r => r.Exercise).Contains(exercise.Exercise))
                        {
                            continue;
                        }

                        var musclesWorkedSoFar = finalResults.WorkedMuscles(addition: MusclesAlreadyWorked, muscleTarget: muscleTarget);
                        // Choose either compound exercises that cover at least X muscles in the targeted muscles set
                        if (BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups.UnsetFlag32(muscleTarget(exercise).UnsetFlag32(musclesWorkedSoFar))) <= BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups) - MuscleGroup.AtLeastXUniqueMusclesPerExercise)
                        {
                            finalResults.Add(new QueryResults(User, exercise.Exercise, exercise.Variation, exercise.ExerciseVariation, exercise.UserExercise, exercise.UserExerciseVariation, exercise.UserVariation, exercise.EasierVariation, exercise.HarderVariation));
                        }
                    }
                }

                // If AtLeastXUniqueMusclesPerExercise is say 4 and there are 7 muscle groups, we don't want 3 isolation exercises at the end if there are no 3-muscle group compound exercises to find.
                // Choose a 3-muscle group compound exercise or a 2-muscle group compound exercise and then an isolation exercise.
                MuscleGroup.AtLeastXUniqueMusclesPerExercise--;
            }

            foreach (var exercise in orderedResults)
            {
                if (finalResults.Select(r => r.Exercise).Contains(exercise.Exercise))
                {
                    continue;
                }

                var musclesWorkedSoFar = finalResults.WorkedMuscles(addition: MusclesAlreadyWorked, muscleTarget: muscleTarget);
                // Grab any muscle groups we missed in the previous loops. Include isolation exercises here
                if (MuscleGroup.AtLeastXUniqueMusclesPerExercise == 0 || muscleTarget(exercise).UnsetFlag32(musclesWorkedSoFar).HasAnyFlag32(MuscleGroup.MuscleGroups))
                {
                    finalResults.Add(new QueryResults(User, exercise.Exercise, exercise.Variation, exercise.ExerciseVariation, exercise.UserExercise, exercise.UserExerciseVariation, exercise.UserVariation, exercise.EasierVariation, exercise.HarderVariation));
                }
            }
        }
        else if (MovementPattern.MovementPatterns.HasValue && MovementPattern.IsUnique)
        {
            var values = Enum.GetValues<MovementPattern>().Where(v => MovementPattern.MovementPatterns.Value.HasFlag(v));
            foreach (var movementPattern in values)
            {
                foreach (var exercise in orderedResults)
                {
                    // Choose either compound exercises that cover at least X muscles in the targeted muscles set
                    if (exercise.Variation.MovementPattern.HasAnyFlag32(movementPattern))
                    {
                        finalResults.Add(new QueryResults(User, exercise.Exercise, exercise.Variation, exercise.ExerciseVariation, exercise.UserExercise, exercise.UserExerciseVariation, exercise.UserVariation, exercise.EasierVariation, exercise.HarderVariation));
                        break;
                    }
                }
            }
        }
        else
        {
            finalResults = orderedResults.Select(a => new QueryResults(User, a.Exercise, a.Variation, a.ExerciseVariation, a.UserExercise, a.UserExerciseVariation, a.UserVariation, a.EasierVariation, a.HarderVariation)).ToList();
        }

        return OrderBy switch
        {
            OrderByEnum.None => finalResults,
            OrderByEnum.UniqueMuscles => finalResults,
            OrderByEnum.Progression => finalResults.Take(SkipCount).Concat(finalResults.Skip(SkipCount)
                                                   .OrderBy(vm => vm.ExerciseVariation.Progression.Min)
                                                   .ThenBy(vm => vm.ExerciseVariation.Progression.Max == null)
                                                   .ThenBy(vm => vm.ExerciseVariation.Progression.Max))
                                                   .ToList(),
            OrderByEnum.MuscleTarget => finalResults.Take(SkipCount).Concat(finalResults.Skip(SkipCount)
                                                    .OrderByDescending(vm => BitOperations.PopCount((ulong)muscleTarget(vm)) - BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups)))
                                                    .ThenBy(vm => BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups))))
                                                    .ToList(),
            OrderByEnum.Name => finalResults.OrderBy(vm => vm.Exercise.Name)
                                            .ThenBy(vm => vm.Variation.Name)
                                            .ToList(),
            _ => finalResults,
        };
    }
}
