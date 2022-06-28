using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CostTypeSymbol : ResourceDefinitionBaseSymbol, ICostTypeSymbol, INodeDeclaredSymbol<CostTypeNode>
{
    public CostTypeSymbol(
        ICatalogueSymbol containingSymbol,
        CostTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override CostTypeNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Cost;
}
