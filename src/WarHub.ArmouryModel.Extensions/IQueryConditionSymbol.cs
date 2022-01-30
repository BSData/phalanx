namespace WarHub.ArmouryModel;

/// <summary>
/// Boolean condition based on query results.
/// BS Condition.
/// WHAM <see cref="Source.ConditionNode" />.
/// </summary>
public interface IQueryConditionSymbol : IConditionSymbol
{
    QueryComparisonType Comparison { get; }

    decimal ComparisonValue { get; }

    IQuerySymbol Query { get; }
}
