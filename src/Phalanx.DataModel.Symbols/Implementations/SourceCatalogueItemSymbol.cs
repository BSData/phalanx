using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SourceCatalogueItemSymbol : SourceDeclaredSymbol, ICatalogueItemSymbol
{
    protected SourceCatalogueItemSymbol(
        ICatalogueItemSymbol containingSymbol,
        SourceNode declaration)
        : base(declaration)
    {
        ContainingSymbolCore = containingSymbol;
    }

    public sealed override ISymbol ContainingSymbol => ContainingSymbolCore;

    protected ICatalogueItemSymbol ContainingSymbolCore { get; }

    public ICatalogueSymbol ContainingCatalogue => ContainingSymbolCore.ContainingCatalogue;

    internal override Compilation DeclaringCompilation => ((CatalogueBaseSymbol)ContainingCatalogue).DeclaringCompilation;
}
