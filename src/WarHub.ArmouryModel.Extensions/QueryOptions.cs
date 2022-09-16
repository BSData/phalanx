namespace WarHub.ArmouryModel;

/// <summary>
/// Additional options for processing <see cref="IQuerySymbol"/>.
/// </summary>
[Flags]
public enum QueryOptions
{
    /// <summary>
    /// No additional options.
    /// </summary>
    None = 0,

    /// <summary>
    /// Count query result of this constraint across all instances of the shared entry.
    /// Otherwise, only entries taken using the same "selection path" will be counted.
    /// Explainer: https://github.com/BSData/wh40k-7th-edition/issues/2880
    /// </summary>
    SharedConstraint = 1 << 0,

    /// <summary>
    /// Count query result taking into account descendant selections deeper than one level.
    /// </summary>
    IncludeDescendantSelections = 1 << 1,

    /// <summary>
    /// Count query result taking into account descendant forces deeper than one level.
    /// </summary>
    IncludeDescendantForces = 1 << 2,

    /// <summary>
    /// Treats <see cref="IQuerySymbol.ReferenceValue"/> as percentage.
    /// </summary>
    ValuePercentage = 1 << 3,

    /// <summary>
    /// Rounds up query result value when non-integral.
    /// </summary>
    ValueRoundUp = 1 << 4,
}
