﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Web.Entities.User;
using Web.Models.Exercise;
using Web.Models.User;

namespace Web.Entities.Exercise;

/// <summary>
/// Exercises listed on the website
/// </summary>
[Table("exercise"), Comment("Exercises listed on the website")]
[DebuggerDisplay("{Name,nq}")]
public class Exercise
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    [Required]
    public string Name { get; private init; } = null!;

    /// <summary>
    /// The progression level needed to attain proficiency in the exercise
    /// </summary>
    [Required, Range(UserExercise.MinUserProgression, UserExercise.MaxUserProgression)]
    public int Proficiency { get; private init; }

    /// <summary>
    /// Primary muscles (usually strengthening) worked by the exercise
    /// </summary>
    [Required]
    public MuscleGroups RecoveryMuscle { get; private init; }

    [Required]
    public SportsFocus SportsFocus { get; private init; }

    /// <summary>
    /// Notes about the variation (externally shown)
    /// </summary>
    public string? Notes { get; private init; } = null;

    public string? DisabledReason { get; private init; } = null;

    [InverseProperty(nameof(ExercisePrerequisite.Exercise))]
    public virtual ICollection<ExercisePrerequisite> Prerequisites { get; private init; } = null!;

    [InverseProperty(nameof(ExercisePrerequisite.PrerequisiteExercise))]
    public virtual ICollection<ExercisePrerequisite> PrerequisiteExercises { get; private init; } = null!;

    [InverseProperty(nameof(ExerciseVariation.Exercise))]
    public virtual ICollection<ExerciseVariation> ExerciseVariations { get; private init; } = null!;

    [InverseProperty(nameof(UserExercise.Exercise))]
    public virtual ICollection<UserExercise> UserExercises { get; private init; } = null!;

    public bool IsPlainExercise => SportsFocus == SportsFocus.None && RecoveryMuscle == MuscleGroups.None;

    public override int GetHashCode() => HashCode.Combine(Id);

    public override bool Equals(object? obj) => obj is Exercise other
        && other.Id == Id;
}
