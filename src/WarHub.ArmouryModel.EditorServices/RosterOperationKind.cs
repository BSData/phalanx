namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
/// A type of single, atomic change in roster.
/// </summary>
public enum RosterOperationKind
{
    Unknown,
    Identity,
    CreateRoster,
    AddSelection,
    RemoveSelection,
    ModifySelectionCount,
    AddForce,
    RemoveForce,
    ModifyCostLimits,
    ModifyTextField,
}
