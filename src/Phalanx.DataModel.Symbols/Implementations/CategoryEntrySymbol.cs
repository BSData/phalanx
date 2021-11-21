using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CategoryEntrySymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol
{
    public CategoryEntrySymbol(
        ICatalogueItemSymbol containingSymbol,
        CategoryEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public bool IsPrimaryCategory => false;

    public ICategoryEntrySymbol? ReferencedEntry => null;

    internal new CategoryEntryNode Declaration { get; }
}
