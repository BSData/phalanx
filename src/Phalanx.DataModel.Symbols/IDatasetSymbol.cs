using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Top level symbol a la Compilation, containing all child symbols.
/// </summary>
public interface IDatasetSymbol : ISymbol
{
    ImmutableArray<ICatalogueSymbol> Catalogues { get; }
}
