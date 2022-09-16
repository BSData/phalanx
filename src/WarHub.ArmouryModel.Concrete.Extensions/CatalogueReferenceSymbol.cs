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

    public int CatalogueRevision => default;

    public ICatalogueSymbol Catalogue => GetBoundField(ref lazyCatalogue);

    public override CatalogueLinkNode Declaration { get; }

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitCatalogueReference(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitCatalogueReference(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitCatalogueReference(this, argument);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyCatalogue = binder.BindCatalogueSymbol(Declaration, diagnostics);
    }
}
