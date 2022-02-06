namespace WarHub.ArmouryModel;

/// <summary>
/// Selection instance in a roster.
/// BS Selection.
/// WHAM <see cref="Source.SelectionNode" />.
/// </summary>
public interface ISelectionSymbol : IRosterSelectionTreeElementSymbol
{
    /// <summary>
    /// Selection count, or the number of times that selection is "taken".
    /// </summary>
    int Count { get; }

    new ISelectionEntrySymbol SourceEntry { get; }

    IRosterSelectionTreeElementSymbol Parent { get; }
}
