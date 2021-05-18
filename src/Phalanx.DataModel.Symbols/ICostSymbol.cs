namespace Phalanx.DataModel.Symbols
{
    public interface ICostSymbol : IResourceEntrySymbol
    {
        new ICostTypeSymbol Type { get; }
    }
}
