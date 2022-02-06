namespace WarHub.ArmouryModel.EditorServices;

public static class RosterOperationExtensions
{
    public static IRosterOperation With(this IRosterOperation @this, IRosterOperation operation) =>
        new ChainedRosterOperation(
            (@this, operation) switch
            {
                (ChainedRosterOperation left, ChainedRosterOperation right) => left.Operations.AddRange(right.Operations),
                (ChainedRosterOperation left, _) => left.Operations.Add(operation),
                (_, ChainedRosterOperation right) => ImmutableArray.Create(@this).AddRange(right.Operations),
                _ => ImmutableArray.Create(@this, operation),
            });

    private record ChainedRosterOperation(ImmutableArray<IRosterOperation> Operations) : IRosterOperation
    {
        public RosterState Apply(RosterState baseState) =>
            Operations.Aggregate(baseState, (acc, op) => op.Apply(acc));
    }

}
