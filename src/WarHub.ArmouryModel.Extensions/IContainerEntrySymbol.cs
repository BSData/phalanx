namespace WarHub.ArmouryModel;

/// <summary>
/// Represents a selectable container entry that can have constraints.
/// WHAM <see cref="Source.ContainerEntryBaseNode" />
/// </summary>
// TODO naming - it's a selection-tree participating entry (force, category, selection, group, links)
public interface IContainerEntrySymbol : IEntrySymbol
{
    /// <summary>
    /// Describes what kind of entry this is.
    /// </summary>
    ContainerEntryKind ContainerKind { get; }

    ImmutableArray<IConstraintSymbol> Constraints { get; }

    /// <summary>
    /// Costs, extracted from <see cref="Resources"/> (but still included there).
    /// </summary>
    ImmutableArray<ICostSymbol> Costs { get; }
}
