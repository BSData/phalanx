using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Depending on a condition, one of two effect sets should apply to a parent.
    /// BS Condition/ConditionGroup/more abstract view over modifier(group)->condition(group)s
    /// </summary>
    public interface IConditionalEffectSymbol : IEffectSymbol
    {
        IConditionSymbol Condition { get; }

        ImmutableArray<IEffectSymbol> SatisfiedEffects { get; }
        
        ImmutableArray<IEffectSymbol> UnsatisfiedEffects{ get; }
    }
}
