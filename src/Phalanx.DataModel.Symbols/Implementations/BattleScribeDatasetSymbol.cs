using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class BattleScribeDatasetSymbol : Symbol, IDatasetSymbol
{
    public BattleScribeDatasetSymbol(ImmutableArray<CatalogueBaseNode> rootNodes)
    {
        // TODO
        Binder binder = null!; // TODO create binder
        BindingDiagnosticContext diagnostics = null!;
        Catalogues = CreateCatalogues().ToImmutableArray();

        IEnumerable<ICatalogueSymbol> CreateCatalogues()
        {
            foreach (var item in rootNodes)
            {
                if (item is CatalogueNode catalogueNode)
                {
                    yield return new CatalogueSymbol(this, catalogueNode, binder, diagnostics);
                }
                else if (item is GamesystemNode gamesystemNode)
                {
                    yield return new GamesystemSymbol(this, gamesystemNode, binder, diagnostics);
                }
                else
                {
                    throw new ArgumentException("Unrecognized root node type.", nameof(rootNodes));
                }
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Catalogue;

    public override string Name => string.Empty;

    public override string? Comment => null;

    public override ISymbol ContainingSymbol => this;

    public ImmutableArray<ICatalogueSymbol> Catalogues { get; }
}
