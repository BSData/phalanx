using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CatalogueReferenceSymbol : CatalogueItemSymbol, ICatalogueReferenceSymbol
{
    public CatalogueReferenceSymbol(
        ICatalogueSymbol containingSymbol,
        CatalogueLinkNode declaration,
        Binder binder,
        BindingDiagnosticContext diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Catalogue = null!; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public bool ImportsRootEntries => Declaration.ImportRootEntries;

    public ICatalogueSymbol Catalogue { get; }

    internal CatalogueLinkNode Declaration { get; }
}
