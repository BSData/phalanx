namespace WarHub.ArmouryModel;

/// <summary>
/// Describes scope in which <see cref="IQuerySymbol"/> collects values.
/// </summary>
public enum QueryScopeKind
{
    /// <summary>
    /// An invalid kind.
    /// </summary>
    Unknown,

    /// <summary>
    /// Values should be collected only within an entry containing the <see cref="IQuerySymbol"/>.
    /// </summary>
    Self,

    /// <summary>
    /// Values should be collected only within a parent entry of an entry containing the <see cref="IQuerySymbol"/>.
    /// </summary>
    Parent,

    /// <summary>
    /// Values should be collected withing an ancestor of the <see cref="IQuerySymbol"/>.
    /// TODO not sure what's the behavior.
    /// </summary>
    ContainingAncestor,

    /// <summary>
    /// Values should be collected within a primary category of the root selection of this symbol.
    /// </summary>
    PrimaryCategory,

    /// <summary>
    /// Values should be collected within a catalogue of the root selection of this symbol.
    /// </summary>
    PrimaryCatalogue,

    /// <summary>
    /// Values should be collected within a force containing the selection of this symbol.
    /// </summary>
    ContainingForce,

    /// <summary>
    /// Values should be collected within the whole roster.
    /// </summary>
    ContainingRoster,

    /// <summary>
    /// Values should be collected withing an entry specified by <see cref="IQuerySymbol.ScopeSymbol"/>.
    /// 
    /// This might be a shared selection entry, a category entry, or a force entry.
    /// </summary>
    ReferencedEntry,
}
