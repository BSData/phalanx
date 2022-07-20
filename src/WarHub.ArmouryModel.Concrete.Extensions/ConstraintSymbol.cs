using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ConstraintSymbol : LogicBaseSymbol, IConstraintSymbol, INodeDeclaredSymbol<ConstraintNode>
{
    public ConstraintSymbol(
        ISymbol? containingSymbol,
        ConstraintNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        Query = QueryBaseSymbol.Create(this, declaration, diagnostics);
    }

    public new ConstraintNode Declaration { get; }

    public sealed override SymbolKind Kind => SymbolKind.Constraint;

    public QueryBaseSymbol Query { get; }

    IQuerySymbol IConstraintSymbol.Query => Query;

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitConstraint(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitConstraint(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitConstraint(this, argument);

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .Add(Query);
}
