namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class CatalogueItemSymbol : Symbol, ICatalogueItemSymbol
{
    protected CatalogueItemSymbol(ICatalogueItemSymbol containingSymbol)
    {
        ContainingSymbolCore = containingSymbol;
    }

    public sealed override ISymbol ContainingSymbol => ContainingSymbolCore;

    protected ICatalogueItemSymbol ContainingSymbolCore { get; }

    public ICatalogueSymbol ContainingCatalogue => ContainingSymbolCore.ContainingCatalogue;

    internal override Compilation DeclaringCompilation => ((CatalogueBaseSymbol)ContainingCatalogue).DeclaringCompilation;
}
