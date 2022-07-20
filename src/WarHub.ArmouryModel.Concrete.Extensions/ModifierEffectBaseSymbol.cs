using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ModifierEffectBaseSymbol : LogicBaseSymbol, IEffectSymbol, INodeDeclaredSymbol<ModifierBaseNode>
{
    protected ModifierEffectBaseSymbol(
        ISymbol? containingSymbol,
        ModifierBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        Condition = declaration switch
        {
            { Conditions.Count: 0, ConditionGroups.Count: 0 } => null,
            _ => ConditionGroupingBaseSymbol.Create(this, declaration, diagnostics)
        };
        Effects = declaration.Repeats.Select(x => new RepeatEffectSymbol(this, x, diagnostics)).ToImmutableArray();
    }

    public new ModifierBaseNode Declaration { get; }

    public sealed override SymbolKind Kind => SymbolKind.Effect;

    public ConditionGroupingBaseSymbol? Condition { get; }

    public ImmutableArray<RepeatEffectSymbol> Effects { get; }

    public abstract ImmutableArray<ModifierEffectBaseSymbol> ChildrenWhenSatisfied { get; }

    public abstract EffectTargetKind TargetKind { get; }

    public abstract ISymbol? TargetMember { get; }

    public abstract EffectOperation FunctionKind { get; }

    public abstract string? OperandValue { get; }

    public abstract ISymbol? OperandSymbol { get; }

    IConditionSymbol? IEffectSymbol.Condition => Condition;

    int IEffectSymbol.Repetitions => 0;

    IQuerySymbol? IEffectSymbol.RepetitionQuery => null;

    ImmutableArray<IEffectSymbol> IEffectSymbol.Effects =>
        Effects.Cast<RepeatEffectSymbol, IEffectSymbol>();

    ImmutableArray<IEffectSymbol> IEffectSymbol.ChildrenWhenSatisfied =>
        ChildrenWhenSatisfied.Cast<ModifierEffectBaseSymbol, IEffectSymbol>();

    ImmutableArray<IEffectSymbol> IEffectSymbol.ChildrenWhenUnsatisfied =>
        ImmutableArray<IEffectSymbol>.Empty;

    public static ModifierEffectBaseSymbol Create(
        ISymbol? containingSymbol,
        ModifierBaseNode declaration,
        DiagnosticBag diagnostics) => declaration switch
        {
            ModifierNode x => new ModifierEffectSymbol(containingSymbol, x, diagnostics),
            ModifierGroupNode x => new ModifierGroupEffectSymbol(containingSymbol, x, diagnostics),
            _ => throw new InvalidOperationException("Unknown declaration type.")
        };

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitEffect(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitEffect(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitEffect(this, argument);

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddWhenNotNull(Condition)
        .AddRange(Effects)
        .AddRange(ChildrenWhenSatisfied);
}
