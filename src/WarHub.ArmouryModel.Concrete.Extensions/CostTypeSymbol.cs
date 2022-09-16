using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Defines cost type.
/// BS CostType.
/// WHAM <see cref="CostTypeNode" />.
/// </summary>
internal class CostTypeSymbol : ResourceDefinitionBaseSymbol, INodeDeclaredSymbol<CostTypeNode>
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
