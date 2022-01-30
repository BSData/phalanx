namespace WarHub.ArmouryModel.EditorServices;

public static class RosterOperationExtensions
{
    public static ChainedRosterOperation With(this IRosterOperation @this, IRosterOperation operation) =>
        ChainedRosterOperation.Create(@this, operation);
}
