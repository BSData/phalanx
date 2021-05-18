namespace Phalanx.DataModel.Symbols
{
    public interface ISelectionSymbol : IForceOrSelectionSymbol
    {
        IForceOrSelectionSymbol Parent { get; }
    }
}
