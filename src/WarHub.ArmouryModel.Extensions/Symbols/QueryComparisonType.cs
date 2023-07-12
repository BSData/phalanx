namespace WarHub.ArmouryModel;

/// <summary>
/// Declares what type of comparison is performed on the query numeric result.
/// </summary>
public enum QueryComparisonType
{
    /// <summary>
    /// There's no comparison. The query result is the original numeric result.
    /// </summary>
    None,

    /// <summary>
    /// Query has a successful outcome when the result is equal to <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    Equal,

    /// <summary>
    /// Query has a successful outcome when the result is not equal to <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    NotEqual,

    /// <summary>
    /// Query has a successful outcome when the result is less than <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    LessThan,

    /// <summary>
    /// Query has a successful outcome when the result is less than or equal to <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Query has a successful outcome when the result is greater than <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Query has a successful outcome when the result is greater than or equal to <see cref="IQuerySymbol.ReferenceValue"/>.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    GreaterThanOrEqual,

    /// <summary>
    /// Special query execution: the query's scope is compared to the filter value.
    /// Outcome is successful when scope symbol is "an instance of" filter symbol.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    InstanceOf,

    /// <summary>
    /// Special query execution: the query's scope is compared to the filter value.
    /// Outcome is successful when scope symbol is not "an instance of" filter symbol.
    /// Otherwise the outcome is unsuccessful.
    /// </summary>
    NotInstanceOf,
}
