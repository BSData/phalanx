using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RepeatEffectSymbol : LogicBaseSymbol, IEffectSymbol, INodeDeclaredSymbol<RepeatNode>
{
    public RepeatEffectSymbol(
        ISymbol? containingSymbol,
        RepeatNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        RepetitionQuery = QueryBaseSymbol.Create(this, declaration, diagnostics);
    }

    public new RepeatNode Declaration { get; }

    public sealed override SymbolKind Kind => SymbolKind.Effect;

    public int Repetitions => Declaration.RepeatCount;

    public QueryBaseSymbol RepetitionQuery { get; }

    IConditionSymbol? IEffectSymbol.Condition => null;

    EffectTargetKind IEffectSymbol.TargetKind => EffectTargetKind.Effect;

    ISymbol? IEffectSymbol.TargetMember => null;

    EffectOperation IEffectSymbol.FunctionKind => EffectOperation.RepeatEffect;

    string? IEffectSymbol.OperandValue => null;

    ISymbol? IEffectSymbol.OperandSymbol => null;

    IQuerySymbol? IEffectSymbol.RepetitionQuery => RepetitionQuery;

    ImmutableArray<IEffectSymbol> IEffectSymbol.Effects =>
        ImmutableArray<IEffectSymbol>.Empty;

    ImmutableArray<IEffectSymbol> IEffectSymbol.ChildrenWhenSatisfied =>
        ImmutableArray<IEffectSymbol>.Empty;

    ImmutableArray<IEffectSymbol> IEffectSymbol.ChildrenWhenUnsatisfied =>
        ImmutableArray<IEffectSymbol>.Empty;

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitEffect(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitEffect(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitEffect(this, argument);

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .Add(RepetitionQuery);
}
