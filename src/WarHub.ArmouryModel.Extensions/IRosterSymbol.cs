namespace WarHub.ArmouryModel;

/// <summary>
/// Roster root.
/// BS Roster.
/// WHAM <see cref="Source.RosterNode" />.
/// </summary>
public interface IRosterSymbol : IModuleSymbol
{
    string? CustomNotes { get; }
    ImmutableArray<IRosterCostSymbol> Costs { get; }
    ImmutableArray<IForceSymbol> Forces { get; }
}

/// <summary>
/// A module is a standalone data blob, such as roster or catalogue, that can be saved, loaded,
/// distributed, versioned, and referenced by other modules. It also belongs to a gamesystem.
/// </summary>
public interface IModuleSymbol : ISymbol
{
    ICatalogueSymbol Gamesystem { get; }
}
