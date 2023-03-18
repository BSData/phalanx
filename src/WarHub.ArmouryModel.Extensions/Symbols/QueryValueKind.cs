namespace WarHub.ArmouryModel;

/// <summary>
/// Describes which kind of value is counted in a <see cref="IQuerySymbol"/>.
/// </summary>
public enum QueryValueKind
{
    /// <summary>
    /// An invalid kind.
    /// </summary>
    Unknown,

    /// <summary>
    /// Query will count selections.
    /// </summary>
    SelectionCount,

    /// <summary>
    /// Query will count forces.
    /// </summary>
    ForceCount,

    /// <summary>
    /// Query will count member values.
    /// Member to be used is declared by <see cref="IQuerySymbol.ValueTypeSymbol"/>.
    /// Used by cost values (cost type is then the symbol).
    /// </summary>
    MemberValue,

    /// <summary>
    /// Query will count limit of value type.
    /// Member to be used is declared by <see cref="IQuerySymbol.ValueTypeSymbol"/>.
    /// Used by roster cost limit (cost type is then the symbol).
    /// </summary>
    MemberValueLimit,
}
