using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CategoryLinkSymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
{
    public CategoryLinkSymbol(
        ICatalogueItemSymbol containingSymbol,
        CategoryLinkNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        ReferencedEntry = null; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public bool IsPrimaryCategory => Declaration.Primary;

    public ICategoryEntrySymbol? ReferencedEntry { get; }

    internal new CategoryLinkNode Declaration { get; }
}
