using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CatalogueReferenceSymbol : SourceCatalogueItemSymbol, ICatalogueReferenceSymbol
{
    public CatalogueReferenceSymbol(ICatalogueSymbol containingSymbol, CatalogueLinkNode declaration)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        Catalogue = null!; // TODO bind
    }

    public override SymbolKind Kind => SymbolKind.Link;

    public bool ImportsRootEntries => Declaration.ImportRootEntries;

    public ICatalogueSymbol Catalogue { get; }

    internal CatalogueLinkNode Declaration { get; }
}
