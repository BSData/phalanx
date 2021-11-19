namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Roster element based on an entry from catalogue.
/// </summary>
public interface IRosterEntrySymbol : IRosterItemSymbol
{
    IEntrySymbol SourceEntry { get; }
    string? CustomName { get; }
    string? CustomNotes { get; }

}
