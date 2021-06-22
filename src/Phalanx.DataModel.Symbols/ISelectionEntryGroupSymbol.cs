namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Selection entry group.
    /// BS SelectionEntryGroup/EntryLink@type=group.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.SelectionEntryGroupNode" />.
    /// </summary>
    public interface ISelectionEntryGroupSymbol : ISelectionEntryContainerSymbol
    {
        ISelectionEntrySymbol? DefaultSelectionEntry { get; }
        new ISelectionEntryGroupSymbol? ReferencedEntry { get; }
    }
}
