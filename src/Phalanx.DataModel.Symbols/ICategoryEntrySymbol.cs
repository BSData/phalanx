namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Category entry.
    /// BS CategoryEntry.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.CategoryEntryNode" />.
    /// </summary>
    public interface ICategoryEntrySymbol : IContainerEntrySymbol
    {
        new ICategoryEntrySymbol? ReferencedEntry { get; }
    }
}
