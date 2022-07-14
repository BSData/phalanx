namespace WarHub.ArmouryModel;

/// <summary>
/// Roster element that contains selections.
/// BS Force/Selection.
/// WHAM <see cref="Source.ForceNode" />
/// and <see cref="Source.SelectionNode" />.
/// </summary>
public interface ISelectionContainerSymbol : ICustomizableEntryInstanceSymbol
{
    /// <summary>
    /// Child selections of this symbol.
    /// </summary>
    ImmutableArray<ISelectionSymbol> Selections { get; }
}
