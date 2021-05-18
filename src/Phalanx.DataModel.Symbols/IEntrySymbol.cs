using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    public interface IEntrySymbol : ICatalogueItemSymbol
    {
        ImmutableArray<IEffectSymbol> Effects { get; }
    }
}
