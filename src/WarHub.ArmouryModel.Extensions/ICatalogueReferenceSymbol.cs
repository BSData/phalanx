namespace WarHub.ArmouryModel;

/// <summary>
/// A reference of an imported catalogue.
/// WHAM <see cref="Source.CatalogueLinkNode" />.
/// </summary>
public interface ICatalogueReferenceSymbol : ISymbol
{
    bool ImportsRootEntries { get; }

    ICatalogueSymbol Catalogue { get; }
}
