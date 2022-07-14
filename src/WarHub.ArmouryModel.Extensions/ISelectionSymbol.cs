using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// Selection instance in a roster.
/// BS Selection.
/// WHAM <see cref="Source.SelectionNode" />.
/// </summary>
public interface ISelectionSymbol : ISelectionContainerSymbol
{
    /// <summary>
    /// Selection count (number of times that selection is "taken").
    /// </summary>
    int Count { get; }

    SelectionEntryKind EntryKind { get; }

    new ISelectionEntrySymbol SourceEntry { get; }

    ICategorySymbol? PrimaryCategory { get; }

    ImmutableArray<ICategorySymbol> Categories { get; }

    /// <summary>
    /// Costs for this selection (with <see cref="Count"/> taken into account).
    /// Doesn't include costs of <see cref="ISelectionContainerSymbol.Selections"/>.
    /// </summary>
    ImmutableArray<ICostSymbol> Costs { get; }
}
