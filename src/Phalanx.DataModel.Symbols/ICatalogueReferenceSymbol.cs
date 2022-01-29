namespace Phalanx.DataModel.Symbols;

/// <summary>
/// A reference of an imported catalogue.
/// WHAM <see cref="WarHub.ArmouryModel.Source.CatalogueLinkNode" />.
/// </summary>
public interface ICatalogueReferenceSymbol : ISymbol
{
    bool ImportsRootEntries { get; }

    ICatalogueSymbol Catalogue { get; }
}
