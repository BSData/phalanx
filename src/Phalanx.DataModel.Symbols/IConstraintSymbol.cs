namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Applies a validation condition on a parent.
    /// BS Constraint
    /// </summary>
    public interface IConstraintSymbol : ILogicSymbol
    {
        IConditionSymbol Condition { get; }
    }
}
