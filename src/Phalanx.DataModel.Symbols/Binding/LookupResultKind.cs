namespace Phalanx.DataModel.Symbols.Binding;

/// <summary>
/// Kind of a lookup result.
/// </summary>
/// <remarks>
/// Order is important, higher values take precedence over lower values.
/// </remarks>
internal enum LookupResultKind
{
    Empty,
    Unreferenced,
    NotShared,
    NotImported,
    Ambiguous,

    /// <summary>
    /// Indicates a single symbol is totally fine.
    /// </summary>
    Viable,
}
