namespace Phalanx.DataModel.Symbols
{
    public interface ICharacteristicSymbol : IResourceEntrySymbol
    {
        new ICharacteristicTypeSymbol Type { get; }
    }
}
