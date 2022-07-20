namespace WarHub.ArmouryModel;

/// <summary>
/// Describes a process of counting/summing and filtering some elements in some scope.
/// BS Query/FilterBy parts of Constraint/Condition/Repeat.
/// WHAM <see cref="Source.QueryBaseNode" />.
/// </summary>
public interface IQuerySymbol : ILogicSymbol
{
    /// <summary>
    /// The comparison performed between result and <see cref="ReferenceValue"/>.
    /// </summary>
    QueryComparisonType Comparison { get; }

    /// <summary>
    /// A value that this query's result is compared to.
    /// </summary>
    decimal? ReferenceValue { get; }

    /// <summary>
    /// Describes which kind of value is collected.
    /// </summary>
    QueryValueKind ValueKind { get; }

    /// <summary>
    /// Specifies which type of value is collected (e.g. <see cref="IResourceDefinitionSymbol"/>).
    /// </summary>
    ISymbol? ValueTypeSymbol { get; }

    /// <summary>
    /// Scope in which the query collects values.
    /// </summary>
    QueryScopeKind ScopeKind { get; }

    /// <summary>
    /// An optional resolved scope - a symbol in which the query collects values.
    /// </summary>
    ISymbol? ScopeSymbol { get; }

    /// <summary>
    /// Filters which symbols are collected by the query.
    /// </summary>
    QueryFilterKind ValueFilterKind { get; }

    /// <summary>
    /// A specific symbol to filter for.
    /// </summary>
    ISymbol? FilterSymbol { get; }

    /// <summary>
    /// Additional options changing query behavior.
    /// </summary>
    QueryOptions Options { get; }
}
