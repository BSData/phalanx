using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Applies a validation condition on a parent.
/// BS Constraint.
/// WHAM <see cref="WarHub.ArmouryModel.Source.ConstraintNode" />.
/// </summary>
public interface IConstraintSymbol : ILogicSymbol
{
    IConditionSymbol Condition { get; }
    ImmutableArray<IEffectSymbol> Effects { get; }
}
