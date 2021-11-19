using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Roster root.
/// BS Roster.
/// WHAM <see cref="WarHub.ArmouryModel.Source.RosterNode" />.
/// </summary>
public interface IRosterSymbol : ISymbol
{
    string? CustomNotes { get; }
    ICatalogueSymbol Gamesystem { get; }
    ImmutableArray<IRosterCostSymbol> Costs { get; }
    ImmutableArray<IForceSymbol> Forces { get; }
}
