using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Defines group of resources.
    /// BS InfoGroup/InfoLink@type=group.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.InfoGroupNode" />.
    /// </summary>
    public interface IResourceGroupSymbol : IResourceEntrySymbol
    {
        ImmutableArray<IResourceEntrySymbol> Resources { get; }
    }
}
