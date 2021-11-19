namespace Phalanx.DataModel.Symbols;

/// <summary>
/// A non-selectable resource entry.
/// No WHAM analogue.
/// </summary>
public interface IResourceEntrySymbol : IEntrySymbol
{
    IResourceDefinitionSymbol? Type { get; }

    new IResourceEntrySymbol? ReferencedEntry { get; }
}
