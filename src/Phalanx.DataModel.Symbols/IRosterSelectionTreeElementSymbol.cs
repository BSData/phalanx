using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Roster element that contains selections.
/// BS Force/Selection.
/// WHAM <see cref="WarHub.ArmouryModel.Source.ForceNode" />
/// and <see cref="WarHub.ArmouryModel.Source.SelectionNode" />.
/// </summary>
public interface IRosterSelectionTreeElementSymbol : IRosterEntrySymbol
{

    ImmutableArray<ISelectionSymbol> ChildSelections { get; }
}
