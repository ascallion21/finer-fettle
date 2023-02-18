﻿using Web.Models.Exercise;
using Web.Models.Newsletter;
using Web.Models.User;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Numerics;

namespace Web.Entities.User;

/// <summary>
/// User who signed up for the newsletter.
/// </summary>
[Table("user"), Comment("User who signed up for the newsletter")]
[Index(nameof(Email), IsUnique = true)]
[DebuggerDisplay("Email = {Email}, Disabled = {Disabled}")]
public class User
{
    #region Consts

    public const string DemoUser = "demo@test.finerfettle.com";

    public const int DeloadAfterEveryXWeeksMin = 2;
    public const int DeloadAfterEveryXWeeksDefault = 10;
    public const int DeloadAfterEveryXWeeksMax = 18;

    public const int RefreshFunctionalEveryXWeeksMin = 0;
    public const int RefreshFunctionalEveryXWeeksDefault = 4;
    public const int RefreshFunctionalEveryXWeeksMax = 12;

    public const int RefreshAccessoryEveryXWeeksMin = 0;
    public const int RefreshAccessoryEveryXWeeksDefault = 1;
    public const int RefreshAccessoryEveryXWeeksMax = 12;

    #endregion


    public User() 
    {
        EmailAtUTCOffset = 0;
        RestDays = RestDays.None;
        DeloadAfterEveryXWeeks = DeloadAfterEveryXWeeksDefault;
        RefreshAccessoryEveryXWeeks = RefreshAccessoryEveryXWeeksDefault;
        RefreshFunctionalEveryXWeeks = RefreshFunctionalEveryXWeeksDefault;
        EmailVerbosity = Verbosity.Normal;
        Frequency = Frequency.PushPullLeg3Day;
        StrengtheningPreference = StrengtheningPreference.Light;
        CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public User(string email, bool acceptedTerms, bool isNewToFitness) : this()
    {
        Email = email.Trim();
        AcceptedTerms = acceptedTerms;
        IsNewToFitness = isNewToFitness;
        // User is new to fitness? Don't show the 'Adjunct' section so they don't feel overwhelmed
        IncludeAdjunct = !isNewToFitness;
    }

    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; private init; }

    [Required]
    public string Email { get; private init; } = null!;

    [Required]
    public bool AcceptedTerms { get; private init; }

    [Required]
    public bool IncludeAdjunct { get; set; }

    [Required]
    public bool PreferStaticImages { get; set; }

    [Required]
    public bool IsNewToFitness { get; set; }

    [Required, Range(0, 23)]
    public int EmailAtUTCOffset { get; set; }

    /// <summary>
    /// Don't strengthen this muscle group, but do show recovery variations for exercises
    /// </summary>
    [Required]
    public MuscleGroups RecoveryMuscle { get; set; }

    /// <summary>
    /// Include a section to boost a specific sports performance
    /// </summary>
    [Required]
    public SportsFocus SportsFocus { get; set; }

    [Required]
    public RestDays RestDays { get; set; }

    [Required]
    public DateOnly CreatedDate { get; private init; }

    [Required]
    public StrengtheningPreference StrengtheningPreference { get; set; }

    [Required]
    public Frequency Frequency { get; set; }

    [Required, Range(DeloadAfterEveryXWeeksMin, DeloadAfterEveryXWeeksMax)]
    public int DeloadAfterEveryXWeeks { get; set; }

    /// <summary>
    /// How often to refresh functional movement exercises.
    /// </summary>
    [Required, Range(RefreshFunctionalEveryXWeeksMin, RefreshFunctionalEveryXWeeksMax)]
    public int RefreshFunctionalEveryXWeeks { get; set; }

    /// <summary>
    /// How often to refresh accessory exercises.
    /// </summary>
    [Required, Range(RefreshAccessoryEveryXWeeksMin, RefreshAccessoryEveryXWeeksMax)]
    public int RefreshAccessoryEveryXWeeks { get; set; }

    [Required]
    public Verbosity EmailVerbosity { get; set; }

    public DateOnly? LastActive { get; set; } = null;

    public string? DisabledReason { get; set; } = null;


    #region NotMapped

    [NotMapped]
    public bool IsDemoUser => Email == DemoUser;

    [NotMapped]
    public bool Disabled => DisabledReason != null;

    [NotMapped]
    public int WorkoutsDays => 7 - BitOperations.PopCount((ulong)RestDays);

    [NotMapped]
    public IEnumerable<int> EquipmentIds => UserEquipments.Select(e => e.EquipmentId) ?? new List<int>();

    #endregion


    #region Navigation Properties

    [InverseProperty(nameof(UserEquipment.User))]
    public virtual ICollection<UserEquipment> UserEquipments { get; private init; } = new List<UserEquipment>();

    [InverseProperty(nameof(UserToken.User))]
    public virtual ICollection<UserToken> UserTokens { get; private init; } = new List<UserToken>();

    [InverseProperty(nameof(UserExercise.User))]
    public virtual ICollection<UserExercise> UserExercises { get; private init; } = null!;

    [InverseProperty(nameof(UserVariation.User))]
    public virtual ICollection<UserVariation> UserVariations { get; private init; } = null!;

    [InverseProperty(nameof(UserExerciseVariation.User))]
    public virtual ICollection<UserExerciseVariation> UserExerciseVariations { get; private init; } = null!;

    [InverseProperty(nameof(Newsletter.Newsletter.User))]
    public virtual ICollection<Newsletter.Newsletter> Newsletters { get; private init; } = null!;

    #endregion
}
