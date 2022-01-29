namespace Phalanx.DataModel.Symbols.Binding;

internal static class LookupResultKindExtensions
{
    /// <summary>
    /// Maps a <see cref="LookupResultKind"/> to a <see cref="CandidateReason"/>. Should not be called
    /// on <see cref="LookupResultKind.Viable"/>.
    /// </summary>
    /// <param name="resultKind"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static CandidateReason ToCandidateReason(this LookupResultKind resultKind) => resultKind switch
    {
        LookupResultKind.Empty => CandidateReason.None,
        LookupResultKind.Unreferenced => CandidateReason.Unreferenced,
        LookupResultKind.NotShared => CandidateReason.NotShared,
        LookupResultKind.NotImported => CandidateReason.NotImported,
        LookupResultKind.Ambiguous => CandidateReason.Ambiguous,
        LookupResultKind.Viable => CandidateReason.None,
        _ => throw new ArgumentOutOfRangeException(nameof(resultKind)),
    };

    /// <summary>
    /// Return the lowest non-empty result kind.
    /// </summary>
    public static LookupResultKind WorseResultKind(this LookupResultKind kind1, LookupResultKind kind2)
    {
        if (kind1 == LookupResultKind.Empty)
            return kind2;
        if (kind2 == LookupResultKind.Empty)
            return kind1;
        return kind1 < kind2 ? kind1 : kind2;
    }
}
