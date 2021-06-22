using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// A roster-visible entry that has publication reference and might have some effects associated with it.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.EntryBaseCore" />.
    /// </summary>
    public interface IEntrySymbol : ICatalogueItemSymbol
    {
        string? Id { get; }
        bool IsHidden{ get; }

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
    }
}
