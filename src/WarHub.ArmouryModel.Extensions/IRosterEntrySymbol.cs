namespace WarHub.ArmouryModel;

/// <summary>
/// Roster element based on an entry from catalogue.
/// BS BaseSelectable.
/// WHAM <see cref="Source.RosterElementBaseNode"/>.
/// </summary>
public interface IRosterEntrySymbol : ISymbol
{
    IEntrySymbol SourceEntry { get; }
    string? CustomName { get; }
    string? CustomNotes { get; }
    IPublicationReferenceSymbol? PublicationReference { get; }
    ImmutableArray<IResourceEntrySymbol> Resources { get; }
}
