namespace WarHub.ArmouryModel;

/// <summary>
/// An element that evaluates to a boolean value.
/// 
/// Boolean condition based on query results.
/// BS Condition/ConditionGroup.
/// WHAM <see cref="Source.ConditionNode" />/<see cref="Source.ConditionGroupNode" />.
/// 
/// Both <see cref="Query"/> (if existing) and <see cref="Children"/> (if not empty)
/// have to resolve to <see langword="true"/> for the condition to resolve as <see langword="true"/>.
/// </summary>
public interface IConditionSymbol : ILogicSymbol
{
    /// <summary>
    /// A query that has to have a successful outcome for this condition
    /// to resolve as <see langword="true"/> value.
    /// </summary>
    IQuerySymbol? Query { get; }

    /// <summary>
    /// A logical operator to apply to <see cref="Children"/>.
    /// </summary>
    LogicalOperator ChildrenOperator { get; }

    /// <summary>
    /// Child conditions. <see cref="ChildrenOperator"/> is applied to their results.
    /// The aggregate result needs to be <see langword="true"/> for this condition
    /// to resolve as <see langword="true"/> value.
    /// </summary>
    ImmutableArray<IConditionSymbol> Children { get; }
}
