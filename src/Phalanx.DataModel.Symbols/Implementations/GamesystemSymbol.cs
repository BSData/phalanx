using System.Collections.Immutable;
using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class GamesystemSymbol : CatalogueBaseSymbol
{
    public GamesystemSymbol(
        IDatasetSymbol containingSymbol,
        GamesystemNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, binder, diagnostics)
    {
    }

    public override bool IsLibrary => false;

    public override bool IsGamesystem => true;

    public override ICatalogueSymbol Gamesystem => this;

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports =>
        ImmutableArray<ICatalogueReferenceSymbol>.Empty;
}
