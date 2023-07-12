namespace WarHub.ArmouryModel;

/// <summary>
/// A roster-visible entry that has publication reference,
/// migth contain resources, and might have some associated effects.
/// WHAM <see cref="Source.EntryBaseCore" />.
/// </summary>
public interface IEntrySymbol : ISymbol
{
    bool IsHidden { get; }

    /// <summary>
    /// If true, this entry is also a reference to another entry in addition to having it's own children.
    /// <see cref="ReferencedEntry" /> then contains the entry that this one references.
    /// </summary>
    bool IsReference { get; }

    /// <summary>
    /// If <see cref="IsReference" /> is true, this is the entry that this one references.
    /// </summary>
    IEntrySymbol? ReferencedEntry { get; }

    IPublicationReferenceSymbol? PublicationReference { get; }

    ImmutableArray<IEffectSymbol> Effects { get; }

    ImmutableArray<IResourceEntrySymbol> Resources { get; }
}
