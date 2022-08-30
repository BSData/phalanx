namespace WarHub.ArmouryModel;

/// <summary>
/// Selection entry group.
/// BS SelectionEntryGroup/EntryLink@type=group.
/// WHAM <see cref="Source.SelectionEntryGroupNode" />.
/// </summary>
public interface ISelectionEntryGroupSymbol : ISelectionEntryContainerSymbol
{
    ISelectionEntryContainerSymbol? DefaultSelectionEntry { get; }
}
