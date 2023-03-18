namespace WarHub.ArmouryModel;

/// <summary>
/// An operator applicable to logical value(s).
/// </summary>
public enum LogicalOperator
{
    /// <summary>
    /// An invalid value.
    /// </summary>
    Unknown,

    /// <summary>
    /// Unary operator that returns the same value.
    /// </summary>
    Identity,

    /// <summary>
    /// Unary operator that returns the negated value.
    /// </summary>
    Negation,

    /// <summary>
    /// Tuple operator that returns <see langword="true"/> if all values are <see langword="true"/>.
    /// For an empty set of values, resolves as <see langword="true"/>.
    /// AKA "AND".
    /// </summary>
    Conjunction,

    /// <summary>
    /// Tuple operator that returns <see langword="true"/> if any value is <see langword="true"/>.
    /// For an empty set of values, resolves as <see langword="false"/>.
    /// AKA "OR".
    /// </summary>
    Disjunction,
}
