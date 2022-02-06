using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CategoryEntrySymbol : ContainerEntryBaseSymbol, ICategoryEntrySymbol, INodeDeclaredSymbol<CategoryEntryNode>
{
    public CategoryEntrySymbol(
        ISymbol containingSymbol,
        CategoryEntryNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override ContainerEntryKind ContainerKind => ContainerEntryKind.Category;

    public bool IsPrimaryCategory => false;

    public ICategoryEntrySymbol? ReferencedEntry => null;

    public override CategoryEntryNode Declaration { get; }
}
