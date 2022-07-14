namespace WarHub.ArmouryModel;

/// <summary>
/// Roster element based on an entry from catalogue.
/// BS BaseSelectable.
/// WHAM <see cref="Source.RosterElementBaseNode"/>.
/// </summary>
public interface ICustomizableEntryInstanceSymbol : IEntryInstanceSymbol
{
    string? CustomName { get; }

    string? CustomNotes { get; }
}
