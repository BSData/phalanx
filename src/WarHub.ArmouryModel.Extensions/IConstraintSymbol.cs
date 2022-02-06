namespace WarHub.ArmouryModel;

/// <summary>
/// Applies a validation condition on a parent.
/// BS Constraint.
/// WHAM <see cref="Source.ConstraintNode" />.
/// </summary>
public interface IConstraintSymbol : ILogicSymbol
{
    IConditionSymbol Condition { get; }
    ImmutableArray<IEffectSymbol> Effects { get; }
}
