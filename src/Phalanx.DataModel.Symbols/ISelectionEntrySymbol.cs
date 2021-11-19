using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Selection entry.
/// BS SelectionEntry/EntryLink@type=entry.
/// WHAM <see cref="SelectionEntryNode" />.
/// </summary>
public interface ISelectionEntrySymbol : ISelectionEntryContainerSymbol
{
    SelectionEntryKind EntryKind { get; }
}