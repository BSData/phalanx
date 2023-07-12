namespace WarHub.ArmouryModel;

/// <summary>
/// Represents a result of lookup operation over a 0 or 1 symbol (as opposed to a scope).
/// The typical use is to represent that a particular symbol is good/bad/unavailable.
/// 
/// For more exmplanation of Kind, Symbol, Error - see <see cref="LookupResult"/>.
/// </summary>
/// <param name="Kind"><see cref="LookupResult.Kind"/></param>
/// <param name="Symbol"><see cref="LookupResult.Symbols"/></param>
/// <param name="Error"><see cref="LookupResult.Error"/></param>
internal readonly record struct SingleLookupResult(
    LookupResultKind Kind,
    ISymbol? Symbol,
    DiagnosticInfo? Error);
