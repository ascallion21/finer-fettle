﻿using App.Dtos.Equipment;
using App.ViewModels.User;
using Core.Models.Newsletter;
using System.ComponentModel.DataAnnotations;

namespace App.ViewModels.Newsletter;

/// <summary>
/// Viewmodel for Debug.cshtml
/// </summary>
public class DebugViewModel
{
    /// <summary>
    /// The number of footnotes to show in the newsletter
    /// </summary>
    public readonly int FootnoteCount = 2;

    public DebugViewModel(Dtos.User.User user, string token)
    {
        User = new UserNewsletterViewModel(user, token);
        Verbosity = user.EmailVerbosity;
    }

    public UserNewsletterViewModel User { get; }

    /// <summary>
    /// How much detail to show in the newsletter.
    /// </summary>
    public Verbosity Verbosity { get; private init; }

    public required IList<ExerciseViewModel> DebugExercises { get; init; }

    /// <summary>
    /// Display which equipment the user does not have.
    /// </summary>
    [UIHint(nameof(EquipmentDto))]
    public EquipmentViewModel AllEquipment { get; init; } = null!;
}
