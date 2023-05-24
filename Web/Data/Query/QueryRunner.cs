﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Web.Code.Extensions;
using Web.Data.Query.Options;
using Web.Entities.Exercise;
using Web.Entities.User;
using Web.Models.Exercise;
using Web.Models.User;

namespace Web.Data.Query;

/// <summary>
/// Builds and runs an EF Core query for selecting exercises.
/// </summary>
public class QueryRunner
{
    [DebuggerDisplay("{Exercise}")]
    private class ExercisesQueryResults
    {
        public Exercise Exercise { get; init; } = null!;
        public UserExercise UserExercise { get; init; } = null!;
    }

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
        public bool IsMinProgressionInRange { get; init; }
        public bool IsMaxProgressionInRange { get; init; }
        public bool AllCurrentVariationsIgnored { get; set; }
        public bool AllCurrentVariationsMissingEquipment { get; set; }
        public Tuple<Variation?, string?>? HarderVariation { get; set; }
        public Tuple<Variation?, string?>? EasierVariation { get; set; }
        public int? NextProgression { get; set; }
    }

    [DebuggerDisplay("{Variation}")]
    private class VariationsQueryResults
    {
        public Variation Variation { get; init; } = null!;
        public UserVariation UserVariation { get; init; } = null!;
    }

    [DebuggerDisplay("{Exercise}: {Variation}")]
    private class ExerciseVariationsQueryResults
        : IExerciseVariationCombo
    {
        public Exercise Exercise { get; init; } = null!;
        public Variation Variation { get; init; } = null!;
        public ExerciseVariation ExerciseVariation { get; init; } = null!;
        public UserExercise UserExercise { get; init; } = null!;
        public UserExerciseVariation UserExerciseVariation { get; init; } = null!;
        public UserVariation UserVariation { get; init; } = null!;
        public bool IsMinProgressionInRange { get; init; }
        public bool IsMaxProgressionInRange { get; init; }
        public bool UserOwnsEquipment { get; init; }
    }

    private class ExerciseComparer : IEqualityComparer<InProgressQueryResults>
    {
        public bool Equals(InProgressQueryResults? x, InProgressQueryResults? y)
        {
            return x?.Exercise.Id == y?.Exercise.Id;
        }

        public int GetHashCode([DisallowNull] InProgressQueryResults obj)
        {
            return HashCode.Combine(obj.Exercise.Id);
        }
    }

    public readonly CoreContext Context;
    public readonly bool IgnoreGlobalQueryFilters = false;

    public required User? User;

    public required SelectionOptions SelectionOptions { get; init; }
    public required ExclusionOptions ExclusionOptions { get; init; }
    public required ExerciseOptions ExerciseOptions { get; init; }
    public required ProficiencyOptions Proficiency { get; init; }
    public required MovementPatternOptions MovementPattern { get; init; }
    public required MuscleGroupOptions MuscleGroup { get; init; }
    public required WeightOptions WeightOptions { get; init; }
    public required ExerciseTypeOptions ExerciseTypeOptions { get; init; }
    public required OrderByOptions OrderByOptions { get; init; }
    public required JointsOptions JointsOptions { get; init; }
    public required SportsOptions SportsOptions { get; init; }
    public required EquipmentOptions EquipmentOptions { get; init; }
    public required ExerciseFocusOptions ExerciseFocusOptions { get; init; }
    public required MuscleContractionsOptions MuscleContractionsOptions { get; init; }
    public required MuscleMovementOptions MuscleMovementOptions { get; init; }

    public QueryRunner(CoreContext context, bool ignoreGlobalQueryFilters = false)
    {
        Context = context;
        IgnoreGlobalQueryFilters = ignoreGlobalQueryFilters;
    }

    // TODO Pass in what should be included or shouldn't be (via enum?)
    private IQueryable<ExercisesQueryResults> CreateExercisesQuery(bool includes)
    {
        var query = Context.Exercises.AsNoTracking().TagWith(nameof(CreateExercisesQuery));

        if (includes)
        {
            query = query
                .Include(e => e.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteExercise)
                        .ThenInclude(e => e.UserExercises.Where(ue => ue.User == User))
                .Include(e => e.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteExercise)
                        .ThenInclude(e => e.ExerciseVariations)
                            .ThenInclude(e => e.UserExerciseVariations.Where(ev => ev.User == User))
                .Include(e => e.Prerequisites)
                    .ThenInclude(p => p.PrerequisiteExercise)
                        .ThenInclude(e => e.ExerciseVariations)
                            .ThenInclude(e => e.Variation)
                                .ThenInclude(v => v.UserVariations.Where(ev => ev.User == User));
        }

        return query.Select(i => new ExercisesQueryResults()
        {
            Exercise = i,
            UserExercise = i.UserExercises.First(ue => ue.User == User)
        });
    }

    // TODO Pass in what should be included or shouldn't be (via enum?)
    private IQueryable<VariationsQueryResults> CreateVariationsQuery(bool includes)
    {
        var query = Context.Variations.AsNoTracking().TagWith(nameof(CreateVariationsQuery));

        if (includes)
        {
            query = query
                .Include(i => i.Intensities)
                .Include(i => i.DefaultInstruction)
                // Instruction equipment is auto included
                .Include(i => i.Instructions.Where(eg => eg.Parent == null && eg.Equipment.Any()))
                    .ThenInclude(eg => eg.Children.Where(ceg => ceg.Equipment.Any()));
        }

        return query.Select(v => new VariationsQueryResults()
        {
            Variation = v,
            UserVariation = v.UserVariations.First(uv => uv.User == User)
        });
    }

    // TODO Pass in what should be included or shouldn't be (via enum?)
    private IQueryable<ExerciseVariationsQueryResults> CreateExerciseVariationsQuery(bool includes)
    {
        return Context.ExerciseVariations.AsNoTracking().TagWith(nameof(CreateExerciseVariationsQuery))
            .Join(CreateExercisesQuery(includes), o => o.ExerciseId, i => i.Exercise.Id, (o, i) => new
            {
                ExerciseVariation = o,
                i.Exercise,
                i.UserExercise
            })
            .Join(CreateVariationsQuery(includes), o => o.ExerciseVariation.VariationId, i => i.Variation.Id, (o, i) => new
            {
                o.ExerciseVariation,
                o.Exercise,
                o.UserExercise,
                i.Variation,
                i.UserVariation
            })
            .Select(a => new ExerciseVariationsQueryResults()
            {
                ExerciseVariation = a.ExerciseVariation,
                Exercise = a.Exercise,
                Variation = a.Variation,
                UserExercise = a.UserExercise,
                UserVariation = a.UserVariation,
                UserExerciseVariation = a.ExerciseVariation.UserExerciseVariations.First(uev => uev.User == User),
                //UserExercise = a.ExerciseVariation.Variation.UserExercises.First(uev => uev.User == User),
                // Out of range when the exercise is too difficult for the user
                IsMinProgressionInRange = User == null
                    // This exercise variation has no minimum 
                    || a.ExerciseVariation.Progression.Min == null
                    // Compare the exercise's progression range with the user's exercise progression
                    || a.ExerciseVariation.Progression.Min <= (Proficiency.DoCapAtProficiency ? Math.Min(a.ExerciseVariation.Exercise.Proficiency, a.UserExercise.Progression) : a.UserExercise.Progression),
                // Out of range when the exercise is too easy for the user
                IsMaxProgressionInRange = User == null
                    // This exercise variation has no maximum
                    || a.ExerciseVariation.Progression.Max == null
                    // Compare the exercise's progression range with the user's exercise progression
                    || (a.UserExercise.Progression < a.ExerciseVariation.Progression.Max),
                // User owns at least one equipment in at least one of the optional equipment groups
                UserOwnsEquipment = User == null
                    // There is an instruction that does not require any equipment
                    || a.Variation.Instructions.Any(eg => !eg.Equipment.Any())
                    // Out of the instructions that require equipment, the user owns the equipment for the root instruction and the root instruction can be done on its own, or the user own the equipment of the child instructions. 
                    || a.Variation.Instructions.Where(eg => eg.Equipment.Any()).Any(peg =>
                        // User owns equipment for the root instruction 
                        peg.Equipment.Any(e => User.EquipmentIds.Contains(e.Id))
                        && (
                            // Root instruction can be done on its own
                            peg.Link != null
                            // Or the user owns the equipment for the child instructions
                            || peg.Children.Any(ceg => ceg.Equipment.Any(e => User.EquipmentIds.Contains(e.Id)))
                        )
                    )
            });
    }

    /// <summary>
    /// Queries the db for the data
    /// </summary>
    public async Task<IList<QueryResults>> Query()
    {
        var filteredQuery = CreateExerciseVariationsQuery(includes: true)
            // Filter down to variations the user owns equipment for
            .Where(i => i.UserOwnsEquipment)
            // Don't grab exercises that the user wants to ignore
            .Where(i => i.UserExercise.Ignore != true)
            // Don't grab variations that the user wants to ignore
            .Where(i => i.UserVariation.Ignore != true)
            // Don't grab groups that we want to ignore
            .Where(vm => (ExclusionOptions.ExerciseGroups & vm.Exercise.Groups) == 0)
            // Don't grab exercises that we want to ignore
            .Where(vm => !ExclusionOptions.ExerciseIds.Contains(vm.Exercise.Id))
            // Don't grab variations that we want to ignore.
            .Where(vm => !ExclusionOptions.VariationIds.Contains(vm.Variation.Id));

        if (IgnoreGlobalQueryFilters)
        {
            filteredQuery = filteredQuery.IgnoreQueryFilters();
        }

        // Filters here will also apply to prerequisites.
        filteredQuery = Filters.FilterExerciseType(filteredQuery, ExerciseTypeOptions.PrerequisiteExerciseType);

        // Grab a half-filtered list of exercises to check the prerequisites from.
        // We don't want to see a rehab exercise as a prerequisite when strength training.
        // We do want to see Planks (isometric) and Dynamic Planks (isotonic) as a prereq for Mountain Climbers (plyo).
        var checkPrerequisitesFrom = await filteredQuery.AsNoTracking().Select(e => e.Exercise.Id).ToListAsync();

        // Filters here will not apply to prerequisites.
        filteredQuery = Filters.FilterExerciseType(filteredQuery, ExerciseTypeOptions.ExerciseType);
        filteredQuery = Filters.FilterJoints(filteredQuery, JointsOptions.Joints);
        filteredQuery = Filters.FilterExercises(filteredQuery, ExerciseOptions.ExerciseIds);
        filteredQuery = Filters.FilterVariations(filteredQuery, ExerciseOptions.VariationIds);
        filteredQuery = Filters.FilterExerciseFocus(filteredQuery, ExerciseFocusOptions.ExerciseFocus);
        filteredQuery = Filters.FilterSportsFocus(filteredQuery, SportsOptions.SportsFocus);
        filteredQuery = Filters.FilterMovementPattern(filteredQuery, MovementPattern.MovementPatterns);
        filteredQuery = Filters.FilterMuscleGroup(filteredQuery, MuscleGroup.MuscleGroups, include: true, MuscleGroup.MuscleTarget);
        filteredQuery = Filters.FilterMuscleGroup(filteredQuery, MuscleGroup.ExcludeRecoveryMuscle, include: false, v => v.Variation.StrengthMuscles | v.Variation.StabilityMuscles);
        filteredQuery = Filters.FilterEquipmentIds(filteredQuery, EquipmentOptions.EquipmentIds);
        filteredQuery = Filters.FilterMuscleContractions(filteredQuery, MuscleContractionsOptions.MuscleContractions);
        filteredQuery = Filters.FilterMuscleMovement(filteredQuery, MuscleMovementOptions.MuscleMovement);
        filteredQuery = Filters.FilterOnlyWeights(filteredQuery, WeightOptions.OnlyWeights);

        var queryResults = await filteredQuery.AsNoTracking().TagWithCallSite()
            .Select(a => new InProgressQueryResults()
            {
                UserExercise = a.UserExercise,
                UserVariation = a.UserVariation,
                UserExerciseVariation = a.UserExerciseVariation,
                Exercise = a.Exercise,
                Variation = a.Variation,
                ExerciseVariation = a.ExerciseVariation,
                IsMinProgressionInRange = a.IsMinProgressionInRange,
                IsMaxProgressionInRange = a.IsMaxProgressionInRange,
            }).ToListAsync();

        if (User != null)
        {
            // Require the prerequisites show first
            queryResults = queryResults.Where(inProgressQueryResult =>
                inProgressQueryResult.Exercise.Prerequisites.Select(exercisePrereq => new
                {
                    exercisePrereq.PrerequisiteExercise,
                    UserExercise = exercisePrereq.PrerequisiteExercise.UserExercises.First(up => up.UserId == User.Id),
                    // All the UserVariations that should contribute to the prerequisite check. Match these with the UserExerciseVariations.
                    UserVariations = exercisePrereq.PrerequisiteExercise.ExerciseVariations.SelectMany(ev => 
                        ev.Variation.UserVariations.Where(uev => uev.UserId == User.Id
                            && ev.Progression.MinOrDefault <= exercisePrereq.PrerequisiteExercise.Proficiency
                            && ev.Progression.MaxOrDefault > exercisePrereq.PrerequisiteExercise.Proficiency)
                        ),
                    // All the UserExerciseVariations that should contribute to the prerequisite check. Match these with the UserVariations.
                    UserExerciseVariations = exercisePrereq.PrerequisiteExercise.ExerciseVariations.SelectMany(ev => 
                        ev.UserExerciseVariations.Where(uev => uev.UserId == User.Id 
                            && ev.Progression.MinOrDefault <= exercisePrereq.PrerequisiteExercise.Proficiency 
                            && ev.Progression.MaxOrDefault > exercisePrereq.PrerequisiteExercise.Proficiency)
                        )
                })
                // The prerequisite is in the list of filtered exercises, so that we don't see a rehab exercise as a prerequisite when strength training.
                .Where(prereq => checkPrerequisitesFrom.Contains(prereq.PrerequisiteExercise.Id))
                // All of the prerequisite exercise/variations are ignored, or have been seen and the user is proficient.
                .All(prereq =>
                    // The prerequisite exercise was ignored.
                    prereq.UserExercise.Ignore
                    // All of the prerequisite's proficiency variations variations were ignored.
                    || prereq.UserVariations.All(uv => uv.Ignore)
                    // User is at or past the required proficiency level.
                    || (prereq.UserExercise.Progression >= prereq.PrerequisiteExercise.Proficiency
                        // The prerequisite exercise has been seen in the past.
                        // We don't want to show Handstand Pushups before the user has seen Pushups.
                        && prereq.UserExercise.LastSeen > DateOnly.MinValue
                        // All of the prerequisite's proficiency variations have been seen in the past.
                        // We don't want to show Handstand Pushups before the user has seen Full Pushups.
                        && prereq.UserExerciseVariations.All(ev => ev.LastSeen > DateOnly.MinValue)
                    )
                )
            ).ToList();

            // Grab a list of non-filtered variations for all the exercises we grabbed.
            var eligibleExerciseIds = queryResults.Select(qr => qr.Exercise.Id).ToList();
            var allExercisesVariations = await CreateExerciseVariationsQuery(includes: false)
                // We only need exercise variations for the exercises in our query result set.
                .Where(ev => eligibleExerciseIds.Contains(ev.Exercise.Id))
                .ToListAsync();
            foreach (var queryResult in queryResults)
            {
                if (queryResult.UserExercise == null || queryResult.UserExerciseVariation == null || queryResult.UserVariation == null)
                {
                    throw new NullReferenceException("User* values must be set before querying");
                }

                // Grab variations that are in the user's progression range. Use the non-filtered list when checking these so we can see if we need to grab an out-of-range progression.
                queryResult.AllCurrentVariationsIgnored = allExercisesVariations
                    .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                    .Where(ev => ev.IsMinProgressionInRange && ev.IsMaxProgressionInRange)
                    .All(ev => ev.UserVariation.Ignore);

                // Grab variations that the user owns the necessary equipment for. Use the non-filtered list when checking these so we can see if we need to grab an out-of-range progression.
                queryResult.AllCurrentVariationsMissingEquipment = allExercisesVariations
                    .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                    .Where(ev => ev.IsMinProgressionInRange && ev.IsMaxProgressionInRange)
                    .All(ev => !ev.UserOwnsEquipment);

                queryResult.EasierVariation = Tuple.Create(allExercisesVariations
                    .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                    // Don't show ignored variations? (untested)
                    //.Where(ev => ev.Variation.UserVariations.FirstOrDefault(uv => uv.User == User)!.Ignore != true)
                    .OrderByDescending(ev => ev.ExerciseVariation.Progression.Max)
                    // Choose the variation that is ignored if all the current variations are ignored, otherwise choose the un-ignored variation
                    .ThenBy(ev => queryResult.AllCurrentVariationsIgnored ? ev.UserVariation?.Ignore == true : ev.UserVariation?.Ignore == false)
                    .FirstOrDefault(ev => ev.ExerciseVariation.Id != queryResult.ExerciseVariation.Id
                        && (
                            // Current progression is in range, choose the previous progression by looking at the user's current progression level
                            (queryResult.IsMinProgressionInRange && queryResult.IsMaxProgressionInRange && ev.ExerciseVariation.Progression.Max != null && ev.ExerciseVariation.Progression.Max <= queryResult.UserExercise.Progression)
                            // Current progression is out of range, choose the previous progression by looking at current exercise's min progression
                            || (!queryResult.IsMinProgressionInRange && ev.ExerciseVariation.Progression.Max != null && ev.ExerciseVariation.Progression.Max <= queryResult.ExerciseVariation.Progression.Min)
                        ))?
                    .Variation, !queryResult.IsMinProgressionInRange ? (queryResult.AllCurrentVariationsIgnored ? "Ignored" : "Missing Equipment") : null);

                queryResult.HarderVariation = Tuple.Create(allExercisesVariations
                    .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                    // Don't show ignored variations? (untested)
                    //.Where(ev => ev.Variation.UserVariations.FirstOrDefault(uv => uv.User == User)!.Ignore != true)
                    .OrderBy(ev => ev.ExerciseVariation.Progression.Min)
                    // Choose the variation that is ignored if all the current variations are ignored, otherwise choose the un-ignored variation
                    .ThenBy(ev => queryResult.AllCurrentVariationsIgnored ? ev.UserVariation?.Ignore == true : ev.UserVariation?.Ignore == false)
                    .FirstOrDefault(ev => ev.ExerciseVariation.Id != queryResult.ExerciseVariation.Id
                        && (
                            // Current progression is in range, choose the next progression by looking at the user's current progression level
                            (queryResult.IsMinProgressionInRange && queryResult.IsMaxProgressionInRange && ev.ExerciseVariation.Progression.Min != null && ev.ExerciseVariation.Progression.Min > queryResult.UserExercise.Progression)
                            // Current progression is out of range, choose the next progression by looking at current exercise's min progression
                            || (!queryResult.IsMaxProgressionInRange && ev.ExerciseVariation.Progression.Min != null && ev.ExerciseVariation.Progression.Min > queryResult.ExerciseVariation.Progression.Max)
                        ))?
                    .Variation, !queryResult.IsMaxProgressionInRange ? (queryResult.AllCurrentVariationsIgnored ? "Ignored" : "Missing Equipment") : null);

                // The next variation in the exercise track based on variation progression levels
                queryResult.NextProgression = allExercisesVariations
                    // Stop at the lower bounds of variations    
                    .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                        // Don't include next progression that have been ignored, so that if the first two variations are ignored, we select the third
                        .Where(ev => ev.UserVariation?.Ignore == false)
                        .Select(ev => ev.ExerciseVariation.Progression.Min)
                    // Stop at the upper bounds of variations
                    .Union(allExercisesVariations
                        .Where(ev => ev.Exercise.Id == queryResult.Exercise.Id)
                        // Don't include next progression that have been ignored, so that if the first two variations are ignored, we select the third
                        .Where(ev => ev.UserVariation?.Ignore == false)
                        .Select(ev => ev.ExerciseVariation.Progression.Max)
                    )
                    .Where(mp => mp.HasValue && mp > queryResult.UserExercise.Progression)
                    .OrderBy(mp => mp - queryResult.UserExercise.Progression)
                    .FirstOrDefault();
            }

            // Try choosing variations that have a max progression above the user's progression. Fallback to an easier variation if one does not exist.
            queryResults = queryResults.GroupBy(i => i, new ExerciseComparer()) 
                                .SelectMany(g => // LINQ is not the way to go about this...
                                    // If there is no variation in the max user progression range (say, if the harder variation requires weights), take the next easiest variation
                                    g.Where(a => a.IsMinProgressionInRange && a.IsMaxProgressionInRange).NullIfEmpty()
                                        ?? g.Where(a => !a.IsMaxProgressionInRange /*&& Proficiency.AllowLesserProgressions*/)
                                            // Only grab lower progressions when all of the current variations are ignored.
                                            // It's possible a lack of equipment causes the current variation to not show.
                                            .Where(a => a.AllCurrentVariationsIgnored || a.AllCurrentVariationsMissingEquipment)
                                            // FIXED: If two variations have the same max proficiency, should we select both? Yes
                                            .GroupBy(e => e.ExerciseVariation.Progression.MaxOrDefault).OrderByDescending(k => k.Key).Take(1).SelectMany(k => k).NullIfEmpty()
                                        // If there is no lesser progression, select the next higher variation.
                                        // We do this so the user doesn't get stuck at the beginning of an exercise track if they ignore the first variation instead of progressing.
                                        ?? g.Where(a => !a.IsMinProgressionInRange /*&& Proficiency.AllowGreaterProgressions*/)
                                            // Only grab higher progressions when all of the current variations are ignored.
                                            // It's possible a lack of equipment causes the current variation to not show.
                                            .Where(a => a.AllCurrentVariationsIgnored || a.AllCurrentVariationsMissingEquipment)
                                            // FIXED: When filtering down to something like MovementPatterns,
                                            // ...if the next highest variation that passes the MovementPattern filter is higher than the next highest variation that doesn't,
                                            // ...then we will get a twice-as-difficult next variation.
                                            .Where(a => a.ExerciseVariation.Progression.MinOrDefault <= (g.Key.NextProgression ?? UserExercise.MaxUserProgression))
                                            // FIXED: If two variations have the same min proficiency, should we select both? Yes
                                            .GroupBy(e => e.ExerciseVariation.Progression.MinOrDefault).OrderBy(k => k.Key).Take(1).SelectMany(k => k)
                                ).ToList();
        }

        // OrderBy must come after query or you get duplicates.
        var orderedResults = queryResults
            // Show exercises that the user has rarely seen
            .OrderBy(a => a.UserExercise == null ? DateOnly.MinValue : a.UserExercise.LastSeen)
            // Show variations that the user has rarely seen
            .ThenBy(a => a.UserExerciseVariation == null ? DateOnly.MinValue : a.UserExerciseVariation.LastSeen)
            // Mostly for the demo, show mostly random exercises
            .ThenBy(a => Guid.NewGuid());

        var muscleTarget = MuscleGroup.MuscleTarget.Compile();
        var finalResults = new List<QueryResults>();

        do
        {
            foreach (var exercise in orderedResults)
            {
                // Don't choose two variations of the same exercise
                if (SelectionOptions.UniqueExercises && finalResults.Select(r => r.Exercise).Contains(exercise.Exercise))
                {
                    continue;
                }

                // Don't choose exercises under our desired number of worked muscles
                if (MuscleGroup.AtLeastXMusclesPerExercise != null && BitOperations.PopCount((ulong)muscleTarget(exercise).UnsetFlag32(MuscleGroup.MusclesAlreadyWorked)) < MuscleGroup.AtLeastXMusclesPerExercise)
                {
                    continue;
                }

                // Choose exercises that cover at least X muscles in the targeted muscles set
                if (MuscleGroup.AtLeastXUniqueMusclesPerExercise != null)
                {
                    var musclesWorkedSoFar = finalResults.WorkedMuscles(addition: MuscleGroup.MusclesAlreadyWorked, muscleTarget: muscleTarget);

                    // We've already worked all unique muscles
                    if (musclesWorkedSoFar.HasFlag(MuscleGroup.MuscleGroups))
                    {
                        break;
                    }

                    if (!(BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups.UnsetFlag32(muscleTarget(exercise).UnsetFlag32(musclesWorkedSoFar))) <= BitOperations.PopCount((ulong)MuscleGroup.MuscleGroups) - MuscleGroup.AtLeastXUniqueMusclesPerExercise))
                    {
                        continue;
                    }
                }

                // Choose exercises that cover a unique movement pattern
                if (MovementPattern.MovementPatterns.HasValue && MovementPattern.IsUnique)
                {
                    var movementPatternsWorkedSoFar = finalResults.Aggregate(Models.Exercise.MovementPattern.None, (curr, next) => curr | next.Variation.MovementPattern);
                    
                    // We've already worked all unique movement patterns
                    if (movementPatternsWorkedSoFar.HasFlag(MovementPattern.MovementPatterns.Value))
                    {
                        break;
                    }

                    if (!exercise.Variation.MovementPattern.UnsetFlag32(movementPatternsWorkedSoFar).HasAnyFlag32(MovementPattern.MovementPatterns.Value))
                    {
                        continue;
                    }
                }

                finalResults.Add(new QueryResults(User, exercise.Exercise, exercise.Variation, exercise.ExerciseVariation, exercise.UserExercise, exercise.UserExerciseVariation, exercise.UserVariation, exercise.EasierVariation, exercise.HarderVariation));
            }
        }
        while (
            // If AtLeastXUniqueMusclesPerExercise is say 4 and there are 7 muscle groups, we don't want 3 isolation exercises at the end if there are no 3-muscle group compound exercises to find.
            // Choose a 3-muscle group compound exercise or a 2-muscle group compound exercise and then an isolation exercise.
            (MuscleGroup.AtLeastXUniqueMusclesPerExercise != null
                && --MuscleGroup.AtLeastXUniqueMusclesPerExercise >= 1
                // And not every unique muscle group has already been worked
                && !finalResults.WorkedMuscles(addition: MuscleGroup.MusclesAlreadyWorked, muscleTarget: muscleTarget).HasFlag(MuscleGroup.MuscleGroups)
            )
        );

        return OrderByOptions.OrderBy switch
        {
            OrderBy.None => finalResults,
            OrderBy.Name => finalResults.OrderBy(vm => vm.Variation.Name).ToList(),
            OrderBy.Progression => finalResults.Take(OrderByOptions.SkipCount).Concat(finalResults.Skip(OrderByOptions.SkipCount)
                                                   .OrderBy(vm => vm.ExerciseVariation.Progression.Min)
                                                   .ThenBy(vm => vm.ExerciseVariation.Progression.Max == null)
                                                   .ThenBy(vm => vm.ExerciseVariation.Progression.Max)
                                                   .ThenBy(vm => vm.Variation.Name))
                                                   .ToList(),
            OrderBy.CoreLast => finalResults.Take(OrderByOptions.SkipCount).Concat(finalResults.Skip(OrderByOptions.SkipCount)
                                                    .OrderBy(vm => BitOperations.PopCount((ulong)(muscleTarget(vm) & MuscleGroups.Core)))
                                                    .ThenByDescending(vm => BitOperations.PopCount((ulong)muscleTarget(vm)) - BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups)))
                                                    .ThenBy(vm => BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups))))
                                                    .ToList(),
            OrderBy.MuscleTarget => finalResults.Take(OrderByOptions.SkipCount).Concat(finalResults.Skip(OrderByOptions.SkipCount)
                                                    .OrderByDescending(vm => BitOperations.PopCount((ulong)muscleTarget(vm)) - BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups)))
                                                    .ThenBy(vm => BitOperations.PopCount((ulong)muscleTarget(vm).UnsetFlag32(MuscleGroup.MuscleGroups))))
                                                    .ToList(),
            _ => finalResults,
        };
    }
}
