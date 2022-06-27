using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ForceCatalogueReferenceSymbol : Symbol, ICatalogueReferenceSymbol
{
    private ICatalogueSymbol? lazyCatalogue;

    public ForceCatalogueReferenceSymbol(
        ISymbol? containingSymbol,
        ForceNode declaration,
        DiagnosticBag diagnostics)
    {
        Declaration = declaration;
        ContainingSymbol = containingSymbol;
    }

    public ForceNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Link;

    public override string? Id => Declaration.CatalogueId;

    public override string Name => Declaration.CatalogueName ?? string.Empty;

    public int CatalogueRevision => Declaration.CatalogueRevision;

    public override string? Comment => null;

    public override ISymbol? ContainingSymbol { get; }

    public bool ImportsRootEntries => false;

    public ICatalogueSymbol Catalogue => GetBoundField(ref lazyCatalogue);

    internal override bool RequiresCompletion => true;

    protected override void BindReferences(WhamCompilation compilation, DiagnosticBag diagnostics)
    {
        base.BindReferences(compilation, diagnostics);
        var binder = compilation.GetBinder(Declaration, ContainingSymbol);
        lazyCatalogue = binder.BindCatalogueSymbol(Declaration, diagnostics);
    }
}
