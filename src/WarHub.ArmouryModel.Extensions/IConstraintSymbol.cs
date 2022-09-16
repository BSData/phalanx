namespace WarHub.ArmouryModel;

/// <summary>
/// Applies a validation condition on a parent.
/// 
/// The constraint is described by the <see cref="Query"/>:
/// The query result is compared using <see cref="IQuerySymbol.Comparison"/>
/// to the value of <see cref="IQuerySymbol.ReferenceValue"/>.
/// 
/// BS Constraint.
/// WHAM <see cref="Source.ConstraintNode" />.
/// </summary>
public interface IConstraintSymbol : ILogicSymbol
{
    /// <summary>
    /// Query that calculates value the constraint applies to.
    /// The constraint is described by the query:
    /// The query result is compared using <see cref="IQuerySymbol.Comparison"/>
    /// to the value of <see cref="IQuerySymbol.ReferenceValue"/>.
    /// </summary>
    IQuerySymbol Query { get; }
}
