namespace WarHub.ArmouryModel;

/// <summary>
/// The general interface for all symbols to derive from.
/// </summary>
public interface ISymbol
{
    SymbolKind Kind { get; }

    /// <summary>
    /// Identifier of the symbol, or <see langword="null"/>.
    /// </summary>
    string? Id { get; }

    /// <summary>
    /// Name of given symbol, or empty string if the symbol has no name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Data author comment.
    /// </summary>
    string? Comment { get; }

    /// <summary>
    /// Gets the <see cref="ISymbol"/> for the immediately containing (parent) symbol.
    /// </summary>
    ISymbol? ContainingSymbol { get; }

    /// <summary>
    /// Gets the <see cref="ICatalogueSymbol"/> for the containing catalogue. Returns null if the symbol
    /// is not from a catalogue.
    /// </summary>
    ICatalogueSymbol? ContainingCatalogue { get; }

    /// <summary>
    /// Gets the <see cref="IGamesystemNamespaceSymbol"/> for the containing namespace symbol. Returns null
    /// if the symbol doesn't belong to a namespace (e.g. is a namespace itself).
    /// </summary>
    IGamesystemNamespaceSymbol? ContainingNamespace { get; }
}
