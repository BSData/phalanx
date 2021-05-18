namespace Phalanx.DataModel.Symbols
{
    public interface IResourceEntrySymbol : IEntrySymbol, IResourceEntryOrContainerSymbol
    {
        IResourceTypeSymbol Type { get; }
    }
}
