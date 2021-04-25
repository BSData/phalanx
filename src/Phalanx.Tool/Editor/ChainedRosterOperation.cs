using System.Collections.Immutable;
using System.Linq;

namespace Phalanx.Tool.Editor
{
    public record ChainedRosterOperation(ImmutableArray<IRosterOperation> Operations) : IRosterOperation
    {
        public RosterState Apply(RosterState baseState) =>
            Operations.Aggregate(baseState, (acc, op) => op.Apply(acc));

        public ChainedRosterOperation With(IRosterOperation operation) =>
            new(Operations.Add(operation));

        public static ChainedRosterOperation Create(params IRosterOperation[] operations) =>
            new(operations.ToImmutableArray());
    }
}
