namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// A symbol contained in a <see cref="IRosterSymbol" />.
    /// </summary>
    public interface IRosterItemSymbol : ISymbol
    {
        IRosterSymbol ContainingRoster { get; }
    }
}
