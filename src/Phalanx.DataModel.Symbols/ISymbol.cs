namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// The general interface for all symbols to derive from.
    /// </summary>
    public interface ISymbol
    {
        SymbolKind Kind { get; }
        string Name { get; }

        /// <summary>
        /// The parent symbol.
        /// </summary>
        ISymbol ContainingSymbol { get; }
    }
}
