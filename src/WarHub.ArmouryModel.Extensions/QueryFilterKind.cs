namespace WarHub.ArmouryModel;

/// <summary>
/// Describes a value filter for <see cref="IQuerySymbol"/>.
/// </summary>
public enum QueryFilterKind
{
    /// <summary>
    /// Invalid kind value.
    /// </summary>
    Unknown,

    /// <summary>
    /// Everything is collected. This is effectively a null-filter.
    /// </summary>
    Anything,

    /// <summary>
    /// Only selections of <see cref="Source.SelectionEntryKind.Unit"/> kind are collected.
    /// </summary>
    UnitEntry,

    /// <summary>
    /// Only selections of <see cref="Source.SelectionEntryKind.Model"/> kind are collected.
    /// </summary>
    ModelEntry,

    /// <summary>
    /// Only selections of <see cref="Source.SelectionEntryKind.Upgrade"/> kind are collected.
    /// </summary>
    UpgradeEntry,

    /// <summary>
    /// Only selections of an entry specified by <see cref="IQuerySymbol.FilterSymbol"/> are collected.
    /// 
    /// This might be a selection entry or group (not necessarily shared), a category entry,
    /// or a force entry.
    /// </summary>
    SpecifiedEntry,
}
