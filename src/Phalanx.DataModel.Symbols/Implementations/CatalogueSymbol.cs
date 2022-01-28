using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class CatalogueSymbol : CatalogueBaseSymbol
{
    private ICatalogueSymbol? lazyGamesystem;

    internal new CatalogueNode Declaration { get; }

    public CatalogueSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        CatalogueNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Imports = CreateLinks().ToImmutableArray();

        IEnumerable<ICatalogueReferenceSymbol> CreateLinks()
        {
            foreach (var item in declaration.CatalogueLinks)
            {
                yield return new CatalogueReferenceSymbol(this, item);
            }
        }
    }

    public override bool IsLibrary => Declaration.IsLibrary;

    public override bool IsGamesystem => false;

    public override ICatalogueSymbol Gamesystem => GetBoundField(ref lazyGamesystem);

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyGamesystem = binder.BindGamesystemSymbol(Declaration.GamesystemId);
    }
}
