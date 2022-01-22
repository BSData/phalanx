namespace Phalanx.DataModel.Symbols;

/// <summary>
/// An <see cref="IErrorSymbol"/> is used when the compiler cannot determine a symbol object to return
/// because of an error. For example, if an entry Foo references another via ID 'bar' and an entry with
/// such ID cannot be found, an <see cref="IErrorSymbol"/> is returned when asking what Foo references.
/// </summary>
public interface IErrorSymbol : ISymbol
{
    /// <summary>
    /// When constructing this type, there may have been symbols that seemed to be what the user intended,
    /// but were unsuitable. For example, an entry might've been found, but it was not Shared, or be found
    /// in a catalogue not yet referenced by the referencing entry's containing one. This property returns
    /// the possible symbols that the user might have intended. It will return no symbols if no possible
    /// symbols were found. See the <see cref="CandidateReason"/> property to understand why the symbols
    /// were unsuitable.
    /// </summary>
    ImmutableArray<ISymbol> CandidateSymbols { get; }

    /// <summary>
    /// If <see cref="CandidateSymbols"/> returns one or more symbols, returns the reason that those
    /// symbols were not chosen. Otherwise, returns <see cref="CandidateReason.None"/>.
    /// </summary>
    CandidateReason CandidateReason { get; }
}
