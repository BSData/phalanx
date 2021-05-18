using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Recursive selection container.
    /// BS SelectionEntry/SelectionEntryGroup/EntryLink
    /// </summary>
    public interface ISelectionEntryContainerSymbol : ICoreEntrySymbol
    {
        ImmutableArray<ISelectionEntryContainerSymbol> Children { get; }
    }
}
