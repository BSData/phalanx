namespace WarHub.ArmouryModel;

/// <summary>
/// Category instance.
/// BS Category.
/// WHAM <see cref="Source.CategoryNode"/>.
/// </summary>
public interface ICategorySymbol : IContainerEntryInstanceSymbol
{
    bool IsPrimaryCategory { get; }

    new ICategoryEntrySymbol SourceEntry { get; }
}
