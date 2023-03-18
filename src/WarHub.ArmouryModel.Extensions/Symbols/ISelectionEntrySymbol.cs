using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

/// <summary>
/// Selection entry.
/// BS SelectionEntry/EntryLink@type=entry.
/// WHAM <see cref="SelectionEntryNode" />.
/// </summary>
public interface ISelectionEntrySymbol : ISelectionEntryContainerSymbol
{
    SelectionEntryKind EntryKind { get; }
}
