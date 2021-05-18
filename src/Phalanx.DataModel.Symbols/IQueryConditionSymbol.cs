namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Boolean condition based on query results.
    /// BS Condition
    /// </summary>
    public interface IQueryConditionSymbol : IConditionSymbol
    {
        ComparisonKind Comparison { get; }
        int ComparisonValue { get; }
        IQuerySymbol Query { get; }
        enum ComparisonKind
        {
            Equals,
            NotEquals,
            LessThan,
            GreaterThan,
            // etc
        }
    }
}
