namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// A symbol contained in a <see cref="ICatalogueSymbol" />.
    /// </summary>
    public interface ICatalogueItemSymbol : ISymbol
    {
        ICatalogueSymbol ContainingCatalogue { get; }
    }
}
