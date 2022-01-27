namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Describes a kind of <see cref="IContainerEntrySymbol"/>.
/// </summary>
public enum ContainerEntryKind
{
    /// <summary>
    /// A selectable entry, <see cref="ISelectionEntrySymbol"/>.
    /// </summary>
    Selection,

    /// <summary>
    /// A choice of selectable entries, <see cref="ISelectionEntryGroupSymbol"/>.
    /// </summary>
    SelectionGroup,

    /// <summary>
    /// A tag or attribute concept, <see cref="ICategoryEntrySymbol"/>.
    /// </summary>
    Category,

    /// <summary>
    /// A roster level selection grouping associated with a single catalogue, <see cref="IForceEntrySymbol"/>.
    /// </summary>
    Force,
}
