namespace WarHub.ArmouryModel;

/// <summary>
/// Force instance in a roster.
/// BS Force.
/// WHAM <see cref="Source.ForceNode" />.
/// </summary>
public interface IForceSymbol : IRosterSelectionTreeElementSymbol
{
    new IForceEntrySymbol SourceEntry { get; }

    ImmutableArray<IForceSymbol> ChildForces { get; }
}
