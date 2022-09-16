using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ConditionBaseSymbol : LogicBaseSymbol, IConditionSymbol
{
    protected ConditionBaseSymbol(
        ISymbol containingSymbol,
        SourceNode declaration)
        : base(containingSymbol, declaration)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.Condition;

    public abstract QueryBaseSymbol? Query { get; }

    public abstract LogicalOperator ChildrenOperator { get; }

    public abstract ImmutableArray<ConditionBaseSymbol> Children { get; }

    IQuerySymbol? IConditionSymbol.Query => Query;

    ImmutableArray<IConditionSymbol> IConditionSymbol.Children =>
        Children.Cast<ConditionBaseSymbol, IConditionSymbol>();

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitCondition(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitCondition(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitCondition(this, argument);

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddWhenNotNull(Query)
        .AddRange(Children.Cast<ConditionBaseSymbol, Symbol>());
}
