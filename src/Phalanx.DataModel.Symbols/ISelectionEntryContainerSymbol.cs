using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Recursive selection container.
    /// BS SelectionEntry/SelectionEntryGroup/EntryLink
    /// WHAM <see cref="WarHub.ArmouryModel.Source.SelectionEntryNode" />.
    /// </summary>
    public interface ISelectionEntryContainerSymbol : IContainerEntrySymbol
    {
        ICategoryEntrySymbol? PrimaryCategory { get; }

        ImmutableArray<ICategoryEntrySymbol> Categories { get; }

        ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries { get; }

        new ISelectionEntryContainerSymbol? ReferencedEntry { get; }
    }
}
