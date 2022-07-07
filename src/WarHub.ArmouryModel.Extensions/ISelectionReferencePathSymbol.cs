namespace WarHub.ArmouryModel;

/// <summary>
/// A path that leads to the selection entry, from force node.
/// This is used for disambiguating by which "path" the entry was selected.
/// </summary>
public interface ISelectionReferencePathSymbol : ISymbol
{
    /// <summary>
    /// This property contains an ordered list of entry links which were
    /// "taken" to find the final selection entry. The list includes that entry as the last element.
    /// </summary>
    ImmutableArray<ISelectionEntryContainerSymbol> SourceEntries { get; }
}
