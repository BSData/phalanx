using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols.Implementation
{
    public class BattleScribeDatasetSymbol : Symbol, IDatasetSymbol
    {
        public BattleScribeDatasetSymbol()
        {
            // TODO
            //Binder binder = null!; // TODO create binder
        }

        public override SymbolKind Kind => SymbolKind.Catalogue;

        public override string Name => string.Empty;

        public override string? Comment => null;

        public override ISymbol ContainingSymbol => this;

        public ImmutableArray<ICatalogueSymbol> Catalogues { get; }
    }
}
