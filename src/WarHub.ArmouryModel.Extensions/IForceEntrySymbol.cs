namespace WarHub.ArmouryModel;

/// <summary>
/// Defines force entry.
/// BS ForceEntry.
/// WHAM <see cref="Source.ForceEntryNode" />.
/// </summary>
public interface IForceEntrySymbol : IContainerEntrySymbol
{
    ImmutableArray<IForceEntrySymbol> ChildForces { get; }

    ImmutableArray<ICategoryEntrySymbol> Categories { get; }
}
