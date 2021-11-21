namespace Phalanx.DataModel.Symbols;

/// <summary>
/// The general interface for all symbols to derive from.
/// </summary>
public interface ISymbol
{
    SymbolKind Kind { get; }

    /// <summary>
    /// Name of given symbol, or empty string if the symbol has no name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Data author comment.
    /// </summary>
    string? Comment { get; }

    /// <summary>
    /// The parent symbol.
    /// </summary>
    ISymbol? ContainingSymbol { get; }
}
