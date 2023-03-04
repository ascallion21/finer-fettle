﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using Web.Entities.Equipment;
using Web.Entities.User;
using Web.Models.Exercise;

namespace Web.Entities.Exercise;

// TODO: Implement IValidateableObject and setup model validation instead of using the /exercises/check route
/// <summary>
/// Intensity level of an exercise variation
/// </summary>
[Table("variation"), Comment("Variations of exercises")]
[DebuggerDisplay("{Name,nq}")]
public class Variation
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    /// <summary>
    /// Friendly name.
    /// </summary>
    [Required]
    public string Name { get; private init; } = null!;

    /// <summary>
    /// The filename.ext of the static content image
    /// </summary>
    [Required]
    public string StaticImage { get; private init; } = null!;

    /// <summary>
    /// The filename.ext of the animated content image
    /// </summary>
    public string? AnimatedImage { get; set; }

    /// <summary>
    /// Does this variation work one side at a time or both sides at once?
    /// </summary>
    [Required]
    public bool Unilateral { get; set; }

    /// <summary>
    /// Works against gravity. 
    /// 
    /// A pullup, a squat, a deadlift, a row....
    /// </summary>
    [Required]
    public bool AntiGravity { get; set; }

    /// <summary>
    /// Can the variation be performed with weights?
    /// 
    /// This controls whether the Pounds selector shows to the user.
    /// </summary>
    [Required]
    public bool IsWeighted { get; set; }

    /// <summary>
    /// Does this variation work muscles by moving weights or holding them in place?
    /// </summary>
    [Required]
    public MuscleContractions MuscleContractions { get; private init; }

    /// <summary>
    /// Does this variation work muscles by moving weights or holding them in place?
    /// </summary>
    [Required]
    public MuscleMovement MuscleMovement { get; private init; }

    /// <summary>
    /// What functional movement patterns does this variation work?
    /// </summary>
    [Required]
    public MovementPattern MovementPattern { get; private init; }

    /// <summary>
    /// Primary muscles strengthened by the exercise
    /// </summary>
    [Required]
    public MuscleGroups StrengthMuscles { get; private init; }

    /// <summary>
    /// Primary muscles stretched by the exercise
    /// </summary>
    [Required]
    public MuscleGroups StretchMuscles { get; private init; }

    /// <summary>
    /// Secondary (usually stabilizing) muscles worked by the exercise
    /// </summary>
    [Required]
    public MuscleGroups StabilityMuscles { get; private init; }

    public string? DisabledReason { get; private init; } = null;

    /// <summary>
    /// Notes about the variation (externally shown)
    /// </summary>
    public string? Notes { get; private init; } = null;

    /// <summary>
    /// Combination of this variations Strength, Stretch and Stability muscles worked.
    /// </summary>
    [NotMapped]
    public MuscleGroups AllMuscles => StrengthMuscles | StretchMuscles | StabilityMuscles;

    public int? DefaultInstructionId { get; private init; }

    //[InverseProperty(nameof(Instruction.Variation))]
    public virtual Instruction? DefaultInstruction { get; private init; }

    [UIHint(nameof(Instruction))] //[InverseProperty(nameof(Instruction.Variation))]
    public virtual ICollection<Instruction> Instructions { get; private init; } = new List<Instruction>();

    [InverseProperty(nameof(UserVariation.Variation))]
    public virtual ICollection<UserVariation> UserVariations { get; private init; } = null!;

    [InverseProperty(nameof(Intensity.Variation))]
    public virtual List<Intensity> Intensities { get; private init; } = null!;

    [InverseProperty(nameof(ExerciseVariation.Variation))]
    public virtual ICollection<ExerciseVariation> ExerciseVariations { get; private init; } = null!;

    [InverseProperty(nameof(Newsletter.NewsletterVariation.Variation))]
    public virtual ICollection<Newsletter.NewsletterVariation> NewsletterVariations { get; private init; } = null!;

    public override int GetHashCode() => HashCode.Combine(Id);

    public override bool Equals(object? obj) => obj is Variation other
        && other.Id == Id;
}
