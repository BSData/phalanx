namespace WarHub.ArmouryModel;

/// <summary>
/// Roster root.
/// BS Roster.
/// WHAM <see cref="Source.RosterNode" />.
/// </summary>
public interface IRosterSymbol : ISymbol
{
    string? CustomNotes { get; }
    ICatalogueSymbol Gamesystem { get; }
    ImmutableArray<IRosterCostSymbol> Costs { get; }
    ImmutableArray<IForceSymbol> Forces { get; }
}
