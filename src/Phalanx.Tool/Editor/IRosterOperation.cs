namespace Phalanx.Tool.Editor
{
    public interface IRosterOperation
    {
        RosterOperationKind Kind { get; }

        RosterState Apply();
    }
}
