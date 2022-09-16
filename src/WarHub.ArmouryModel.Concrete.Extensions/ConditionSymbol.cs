using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ConditionSymbol : ConditionBaseSymbol, IConditionSymbol, INodeDeclaredSymbol<ConditionNode>
{
    public ConditionSymbol(
        ISymbol containingSymbol,
        ConditionNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        Query = QueryBaseSymbol.Create(this, declaration, diagnostics);
    }

    public new ConditionNode Declaration { get; }

    public override QueryBaseSymbol Query { get; }

    public override LogicalOperator ChildrenOperator => LogicalOperator.Identity;

    public override ImmutableArray<ConditionBaseSymbol> Children =>
        ImmutableArray<ConditionBaseSymbol>.Empty;
}
