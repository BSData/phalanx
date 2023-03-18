namespace WarHub.ArmouryModel;

/// <summary>
/// Recursive selection container.
/// BS SelectionEntry/SelectionEntryGroup/EntryLink
/// WHAM <see cref="Source.SelectionEntryNode" />.
/// </summary>
public interface ISelectionEntryContainerSymbol : IContainerEntrySymbol
{
    ICategoryEntrySymbol? PrimaryCategory { get; }

    ImmutableArray<ICategoryEntrySymbol> Categories { get; }

    ImmutableArray<ISelectionEntryContainerSymbol> ChildSelectionEntries { get; }

    new ISelectionEntryContainerSymbol? ReferencedEntry { get; }
}
