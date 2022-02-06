namespace WarHub.ArmouryModel;

/// <summary>
/// Defines group of resources.
/// BS InfoGroup/InfoLink@type=group.
/// WHAM <see cref="Source.InfoGroupNode" />.
/// </summary>
public interface IResourceGroupSymbol : IResourceEntrySymbol
{
    ImmutableArray<IResourceEntrySymbol> Resources { get; }
}
