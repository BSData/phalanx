namespace WarHub.ArmouryModel;

/// <summary>
/// Category entry.
/// BS CategoryEntry.
/// WHAM <see cref="Source.CategoryEntryNode" />.
/// </summary>
public interface ICategoryEntrySymbol : IContainerEntrySymbol
{
    bool IsPrimaryCategory { get; }

    new ICategoryEntrySymbol? ReferencedEntry { get; }
}
