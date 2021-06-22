namespace Phalanx.Tool.Editor
{
    /// <summary>
    /// A mutation that can be applied to a roster.
    /// </summary>
    public interface IRosterOperation
    {
        RosterState Apply(RosterState baseState);
    }
}
