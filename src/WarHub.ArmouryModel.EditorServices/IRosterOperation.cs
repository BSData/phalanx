namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
/// A mutation that can be applied to a roster.
/// </summary>
public interface IRosterOperation
{
    RosterOperationKind Kind => RosterOperationKind.Unknown;

    RosterState Apply(RosterState baseState);
}
