using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

/// <summary>
/// A LookupResult summarizes the result of an ID lookup within a scope. It also allows
/// combining ID lookups from different scopes in an easy way.
/// </summary>
internal sealed class LookupResult
{
    internal LookupResultKind Kind { get; private set; }

    internal DiagnosticInfo? Error { get; private set; }

    internal List<ISymbol> Symbols { get; } = new();

    internal bool IsClear => Kind == LookupResultKind.Empty && Error is null;

    /// <summary>
    /// Return the single symbol if there's exactly one, otherwise <see langword="null"/>.
    /// </summary>
    internal ISymbol? SingleSymbolOrDefault => Symbols.Count == 1 ? Symbols[0] : null;

    /// <summary>
    /// Is the result viable with exactly one symbol?
    /// </summary>
    internal bool IsSingleViable => Kind == LookupResultKind.Viable && Symbols.Count == 1;

    /// <summary>
    /// Is the result viable with one or more symbols?
    /// </summary>
    internal bool IsMultiViable => Kind == LookupResultKind.Viable;

    internal void Clear()
    {
        Kind = LookupResultKind.Empty;
        Error = null;
        Symbols.Clear();
    }

    internal static SingleLookupResult Good(ISymbol symbol) =>
        new(LookupResultKind.Viable, symbol, null);

    internal static SingleLookupResult Empty() =>
        new(LookupResultKind.Empty, null, null);

    internal static SingleLookupResult NotImported(ISymbol symbol, DiagnosticInfo error) =>
        new(LookupResultKind.NotImported, symbol, error);

    internal static SingleLookupResult NotShared(ISymbol symbol, DiagnosticInfo error) =>
        new(LookupResultKind.NotShared, symbol, error);

    internal static SingleLookupResult Unreferenced(ISymbol symbol, DiagnosticInfo error) =>
        new(LookupResultKind.Unreferenced, symbol, error);

    /// <summary>
    /// Set current result according to <paramref name="other"/>.
    /// </summary>
    /// <param name="other">Result to set.</param>
    internal void SetFrom(SingleLookupResult other)
    {
        Kind = other.Kind;
        Symbols.Clear();
        if (other.Symbol is not null)
            Symbols.Add(other.Symbol);
        Error = other.Error;
    }

    /// <summary>
    /// Set current result according to <paramref name="error"/>.
    /// </summary>
    /// <param name="error">Result to set.</param>
    internal void SetFrom(LookupResult other)
    {
        Kind = other.Kind;
        Symbols.Clear();
        Symbols.AddRange(other.Symbols);
        Error = other.Error;
    }

    /// <summary>
    /// Set current result to empty with <paramref name="error"/>.
    /// </summary>
    /// <param name="error">Result to set.</param>
    internal void SetFrom(DiagnosticInfo error)
    {
        Clear();
        Error = error;
    }

    /// <summary>
    /// Overwrite this instance with <paramref name="other"/> if its <see cref="Kind"/>
    /// is greater than this.
    /// </summary>
    /// <param name="other">Result to merge.</param>
    internal void MergePrioritized(LookupResult other)
    {
        if (other.Kind > Kind)
            SetFrom(other);
    }

    /// <summary>
    /// Merge another result with this one, with the symbols combined if both this
    /// <paramref name="other"/> are Viable. Otherwise the highest Kind result wins
    /// (this one is prioritized if Kinds are equal but not Viable).
    /// </summary>
    /// <param name="other">Result to merge.</param>
    internal void MergeEqual(LookupResult other)
    {
        if (Kind > other.Kind)
        {
            return;
        }
        else if (other.Kind > Kind)
        {
            SetFrom(other);
        }
        else if (Kind != LookupResultKind.Viable)
        {
            // Kinds are equal, so both are not viable. Lets prefer current.
            return;
        }
        else
        {
            // Both are viable, merging:
            Symbols.AddRange(other.Symbols);
        }
    }

    /// <summary>
    /// Merge another result with this one, with the symbols combined if both Kinds
    /// are equal. Otherwise the highest Kind result wins.
    /// </summary>
    /// <param name="other">Result to merge.</param>
    internal void MergeEqual(SingleLookupResult other)
    {
        if (Kind > other.Kind)
        {
            return;
        }
        else if (other.Kind > Kind)
        {
            SetFrom(other);
        }
        else if (other.Symbol is { } symbol)
        {
            // Kinds are equal, merge
            Symbols.Add(symbol);
        }
    }

    internal static LookupResult GetInstance() => new();

    internal void Free()
    {
        Clear();
    }
}
