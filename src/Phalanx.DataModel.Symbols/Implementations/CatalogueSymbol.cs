using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CatalogueSymbol : CatalogueBaseSymbol
{
    private ICatalogueSymbol? lazyGamesystem;

    internal new CatalogueNode Declaration { get; }

    public CatalogueSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        CatalogueNode declaration)
        : base(containingSymbol, declaration)
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

    public override ICatalogueSymbol Gamesystem
    {
        get
        {
            ForceComplete();
            return lazyGamesystem ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyGamesystem = binder.BindGamesystemSymbol(Declaration.GamesystemId);
    }
}
