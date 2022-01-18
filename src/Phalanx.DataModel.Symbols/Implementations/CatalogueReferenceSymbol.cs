using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class CatalogueReferenceSymbol : SourceDeclaredSymbol, ICatalogueReferenceSymbol
{
    private ICatalogueSymbol? lazyCatalogue;

    public CatalogueReferenceSymbol(ICatalogueSymbol containingSymbol, CatalogueLinkNode declaration)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public bool ImportsRootEntries => Declaration.ImportRootEntries;

    public ICatalogueSymbol Catalogue
    {
        get
        {
            ForceComplete();
            return lazyCatalogue ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    internal new CatalogueLinkNode Declaration { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyCatalogue = binder.BindCatalogueSymbol(Declaration.TargetId, Declaration.Type);
    }
}
