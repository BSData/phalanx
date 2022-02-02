using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CatalogueSymbol : CatalogueBaseSymbol, INodeDeclaredSymbol<CatalogueNode>
{
    private ICatalogueSymbol? lazyGamesystem;

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

    public override CatalogueNode Declaration { get; }

    public override bool IsLibrary => Declaration.IsLibrary;

    public override bool IsGamesystem => false;

    public override ICatalogueSymbol Gamesystem => GetBoundField(ref lazyGamesystem);

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    CatalogueNode INodeDeclaredSymbol<CatalogueNode>.Declaration => Declaration;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyGamesystem = binder.BindGamesystemSymbol(Declaration, diagnosticBag);
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        InvokeForceComplete(Imports);
    }
}
