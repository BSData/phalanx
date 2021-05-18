namespace Phalanx.DataModel.Symbols
{
    public interface ICatalogueItemSymbol : ISymbol
    {
        ICatalogueSymbol ContainingCatalogue { get; }
    }
}
