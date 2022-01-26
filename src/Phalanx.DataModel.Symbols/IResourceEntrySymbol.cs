namespace Phalanx.DataModel.Symbols;

/// <summary>
/// A non-selectable resource entry.
/// No WHAM analogue.
/// </summary>
public interface IResourceEntrySymbol : IEntrySymbol
{
    /// <summary>
    /// Describes what kind of resource this is.
    /// </summary>
    ResourceKind ResourceKind { get; }

    IResourceDefinitionSymbol? Type { get; }

    new IResourceEntrySymbol? ReferencedEntry { get; }
}
