namespace WarHub.ArmouryModel;

/// <summary>
/// Roster root.
/// BS Roster.
/// WHAM <see cref="Source.RosterNode" />.
/// </summary>
public interface IRosterSymbol : IModuleSymbol, IForceContainerSymbol
{
    string? CustomNotes { get; }
    ImmutableArray<IRosterCostSymbol> Costs { get; }
}
