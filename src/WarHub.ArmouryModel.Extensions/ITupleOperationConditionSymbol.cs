namespace WarHub.ArmouryModel;

/// <summary>
/// Applies a boolean operator to a list of child conditions.
/// BS ConditionGroup.
/// WHAM <see cref="Source.ConditionGroupNode" />.
/// </summary>
public interface ITupleOperationConditionSymbol : IConditionSymbol
{
    TupleOperation Operation { get; }

    ImmutableArray<IConditionSymbol> Conditions { get; }
}
