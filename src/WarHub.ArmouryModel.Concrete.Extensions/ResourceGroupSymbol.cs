using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ResourceGroupSymbol : ResourceEntryBaseSymbol, INodeDeclaredSymbol<InfoGroupNode>
{
    public ResourceGroupSymbol(
        ISymbol containingSymbol,
        InfoGroupNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override InfoGroupNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Group;
}
