namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Represents a selectable container entry that can have constraints and resources.
/// WHAM <see cref="WarHub.ArmouryModel.Source.ContainerEntryBaseNode" />
/// </summary>
public interface IContainerEntrySymbol : IEntrySymbol
{
    ImmutableArray<IConstraintSymbol> Constraints { get; }
    ImmutableArray<IResourceEntrySymbol> Resources { get; }
}
