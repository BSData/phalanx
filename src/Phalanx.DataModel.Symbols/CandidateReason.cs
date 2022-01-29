namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Indicates the reasons why a candidate (or set of candidate) symbols were not considered
/// correct semantic result. Higher values take precedence over lower values, so if,
/// for example, there's a symbol with a correct ID that was inaccessible, and the other that
/// is not shared, only the not shared would be reported in the semantic result.
/// </summary>
public enum CandidateReason
{
    /// <summary>
    /// No candidate symbols.
    /// </summary>
    None = 0,

    /// <summary>
    /// The candidate symbols are from an unreferenced catalogue.
    /// </summary>
    Unreferenced = 1,

    /// <summary>
    /// Only shared entry was valid, but this entry was not shared.
    /// </summary>
    NotShared = 2,

    /// <summary>
    /// The candidate symbols are from a referenced catalogue but marked as not-imported.
    /// </summary>
    NotImported = 3,

    /// <summary>
    /// Multiple ambiguous symbols were available with the same name. This can occur
    /// if there are multiple items with the same ID either in parent catalogue and/or gamesystem,
    /// or in imported catalogues.
    /// </summary>
    Ambiguous = 4,
}
