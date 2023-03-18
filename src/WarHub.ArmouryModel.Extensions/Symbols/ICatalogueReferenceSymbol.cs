namespace WarHub.ArmouryModel;

/// <summary>
/// A reference of an imported catalogue.
/// WHAM <see cref="Source.CatalogueLinkNode" />.
/// </summary>
public interface ICatalogueReferenceSymbol : ISymbol
{
    bool ImportsRootEntries { get; }

    /// <summary>
    /// Revision of the <see cref="Catalogue"/> with which the <see cref="IForceSymbol"/> was created with.
    /// Not used by <see cref="Source.CatalogueLinkNode"/>s in catalogues.
    /// </summary>
    int CatalogueRevision { get; }

    ICatalogueSymbol Catalogue { get; }
}
