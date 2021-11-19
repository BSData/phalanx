namespace Phalanx.Tool.Editor;

/// <summary>
/// A type of single, atomic change in roster.
/// </summary>
public enum RosterOperationKind
{
    Unknown,
    CreateRoster,
    AddSelection,
    RemoveSelection,
    ModifySelectionCount,
    AddForce,
    RemoveForce,
    ModifyCostLimits,
    ModifyTextField,
}
