﻿using Lib.ViewModels.Exercise;
using Lib.ViewModels.User;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace Lib.ViewModels.Equipment;

/// <summary>
/// Equipment that can be switched out for one another.
/// </summary>
[DebuggerDisplay("Name = {Name}")]
public class InstructionViewModel
{
    public int Id { get; init; }

    /// <summary>
    /// Friendly name.
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// A link to show the user how to complete the exercise w/ this equipment.
    /// </summary>
    public string? Link { get; init; }

    public string? DisabledReason { get; init; } = null;

    [JsonInclude]
    public IList<InstructionLocationViewModel> Locations { get; init; } = new List<InstructionLocationViewModel>();

    [JsonInclude]
    public ICollection<InstructionViewModel> Children { private get; init; } = new List<InstructionViewModel>();

    public bool HasChildInstructions => Children.Any();

    public IOrderedEnumerable<InstructionViewModel> GetChildInstructions(UserNewsletterViewModel? user)
    {
        return Children
            // Only show the optional equipment groups that the user owns equipment out of
            .Where(eg => user == null || user.EquipmentIds.Intersect(eg.Equipment.Select(e => e.Id)).Any())
            // Keep the order consistent across newsletters
            .OrderBy(eg => eg.Id);
    }

    [JsonInclude]
    public InstructionViewModel? Parent { get; init; } = null!;

    [JsonInclude]
    public ICollection<EquipmentViewModel> Equipment { get; init; } = new List<EquipmentViewModel>();

    [JsonInclude]
    public VariationViewModel Variation { get; init; } = null!;
}
