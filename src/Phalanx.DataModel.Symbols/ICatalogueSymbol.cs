using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    public interface ICatalogueSymbol : ISymbol
    {
        bool IsLibrary { get; }
        ImmutableArray<ICatalogueItemSymbol> Items { get; }
    }
}
