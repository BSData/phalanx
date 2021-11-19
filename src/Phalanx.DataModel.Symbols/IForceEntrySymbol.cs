using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Defines force entry.
/// BS ForceEntry.
/// WHAM <see cref="WarHub.ArmouryModel.Source.ForceEntryNode" />.
/// </summary>
public interface IForceEntrySymbol : IContainerEntrySymbol
{
    ImmutableArray<IForceEntrySymbol> ChildForces { get; }

    ImmutableArray<ICategoryEntrySymbol> Categories { get; }
}
