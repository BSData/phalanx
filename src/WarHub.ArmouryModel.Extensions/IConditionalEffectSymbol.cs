namespace WarHub.ArmouryModel;

/// <summary>
/// Depending on a condition, one of two effect sets should apply to a parent.
/// BS Condition/ConditionGroup/more abstract view over modifier(group)->condition(group)s
/// WHAM <see cref="Source.ConditionNode" />.
/// </summary>
public interface IConditionalEffectSymbol : IEffectSymbol
{
    IConditionSymbol Condition { get; }

    ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }

    ImmutableArray<IEffectSymbol> UnsatisfiedEffects { get; }
}
