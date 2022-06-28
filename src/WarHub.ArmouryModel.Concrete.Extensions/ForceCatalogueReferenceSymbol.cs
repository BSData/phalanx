using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ForceCatalogueReferenceSymbol : SourceDeclaredSymbol, ICatalogueReferenceSymbol
{
    private ICatalogueSymbol? lazyCatalogue;

    public ForceCatalogueReferenceSymbol(
        Symbol? containingSymbol,
        ForceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override ForceNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string? Id => Declaration.CatalogueId;

    public override string Name => Declaration.CatalogueName ?? string.Empty;

    public int CatalogueRevision => Declaration.CatalogueRevision;

    public bool ImportsRootEntries => false;

    public ICatalogueSymbol Catalogue => GetBoundField(ref lazyCatalogue);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyCatalogue = binder.BindCatalogueSymbol(Declaration, diagnostics);
    }
}
