namespace WarHub.ArmouryModel;

/// <summary>
/// Describes a change to be applied on an ancestor entry symbol.
/// Effect application means performing a <see cref="FunctionKind"/> function
/// with argument <see cref="OperandValue"/> (or <see cref="OperandSymbol"/>)
/// on the <see cref="TargetKind"/> field of this effect's ancestor entry symbol.
/// 
/// When the <see cref="Condition"/> is satisfied (or <see langword="null"/>), the effect and <see cref="ChildrenWhenSatisfied"/>
/// are applied .
/// When the <see cref="Condition"/> is unsatisfied, <see cref="ChildrenWhenUnsatisfied"/> are applied once.
/// BS Modifier/ModifierGroup/Repeat.
/// WHAM <see cref="Source.ModifierNode" />/<see cref="Source.ModifierGroupNode"/>/<see cref="Source.RepeatNode" />.
/// </summary>
public interface IEffectSymbol : ILogicSymbol
{
    /// <summary>
    /// Condition necessary for the effect to apply.
    /// If <see langword="null"/>, the condition is considered to be satisfied.
    /// </summary>
    IConditionSymbol? Condition { get; }

    /// <summary>
    /// Specifies which field or descendant symbol of an entry this effect applies to.
    /// </summary>
    EffectTargetKind TargetKind { get; }

    /// <summary>
    /// Specifies which member of the affected symbol is the target.
    /// Used for constraints, costs and profile characteristics.
    /// </summary>
    ISymbol? TargetMember { get; }

    /// <summary>
    /// Specifies what kind of function this effect applies to the ancestor target symbol.
    /// </summary>
    EffectOperation FunctionKind { get; }

    /// <summary>
    /// Specifies what value is used as the argument of the <see cref="FunctionKind"/>.
    /// If the value is resolvable as a symbol, the symbol is available in <see cref="OperandSymbol"/>.
    /// </summary>
    string? OperandValue { get; }

    /// <summary>
    /// Specifies what symbol is used as the argument of the <see cref="FunctionKind"/>.
    /// This is available when the <see cref="OperandValue"/> is resolvable as a symbol.
    /// </summary>
    ISymbol? OperandSymbol { get; }

    /// <summary>
    /// Number of repetitions this effect should be applied for every
    /// multiplication of <see cref="RepetitionQuery"/>'s <see cref="IQuerySymbol.ReferenceValue"/>
    /// in the result value. Has no meaning when <see cref="RepetitionQuery"/> is <see langword="null"/>.
    /// </summary>
    int Repetitions { get; }

    /// <summary>
    /// Query used to calculate effect application repetitions.
    /// If <see langword="null"/>, the effect is always applied once.
    /// </summary>
    IQuerySymbol? RepetitionQuery { get; }

    /// <summary>
    /// Effects that apply to this effect (e.g. repeats).
    /// </summary>
    ImmutableArray<IEffectSymbol> Effects { get; }

    /// <summary>
    /// Additional effects that will be applied together with this one.
    /// This means they'll be applied only if this effect's condition outcome
    /// is positive, and all effects will repeated according to <see cref="RepetitionQuery"/>.
    /// </summary>
    ImmutableArray<IEffectSymbol> ChildrenWhenSatisfied { get; }

    /// <summary>
    /// Additional effects that will be applied when this effect doesn't apply.
    /// This means they'll be applied only if this effect's condition outcome
    /// is negative, and the effects are only applied once (no repetitions).
    /// </summary>
    ImmutableArray<IEffectSymbol> ChildrenWhenUnsatisfied { get; }
}
