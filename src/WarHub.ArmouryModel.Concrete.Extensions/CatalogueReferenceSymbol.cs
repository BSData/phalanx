using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CatalogueReferenceSymbol : SourceDeclaredSymbol, ICatalogueReferenceSymbol, INodeDeclaredSymbol<CatalogueLinkNode>
{
    private ICatalogueSymbol? lazyCatalogue;

    public CatalogueReferenceSymbol(ICatalogueSymbol containingSymbol, CatalogueLinkNode declaration)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public bool ImportsRootEntries => Declaration.ImportRootEntries;

    public ICatalogueSymbol Catalogue => GetBoundField(ref lazyCatalogue);

    public override CatalogueLinkNode Declaration { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyCatalogue = binder.BindCatalogueSymbol(Declaration, diagnosticBag);
    }
}