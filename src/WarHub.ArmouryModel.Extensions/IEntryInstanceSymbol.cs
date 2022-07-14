namespace WarHub.ArmouryModel;

/// <summary>
/// A symbol that's an instance of an entry,
/// identified by <see cref="SourceEntry"/> which was instantiated by traversing
/// links contained in <see cref="SourceEntryPath"/>.
/// </summary>
public interface IEntryInstanceSymbol : ISymbol
{
    IEntrySymbol SourceEntry { get; }

    IEntryReferencePathSymbol SourceEntryPath { get; }

    IPublicationReferenceSymbol? PublicationReference { get; }

    ImmutableArray<IResourceSymbol> Resources { get; }
}
