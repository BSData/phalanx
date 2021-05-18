using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Applies a boolean operator to a list of child conditions.
    /// BS ConditionGroup
    /// </summary>
    public interface ITupleOperationConditionSymbol : IConditionSymbol
    {
        OperationType Type { get; }

        ImmutableArray<IConditionSymbol> Conditions { get; }

        enum OperationType
        {
            And, // conjuction
            Or, // disjunction
        }
    }
}
