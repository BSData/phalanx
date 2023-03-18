namespace WarHub.ArmouryModel;

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/InternalUtilities/EnumerableExtensions.cs
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Returns the only element of specified sequence if it has exactly one, and default(TSource) otherwise.
    /// Unlike <see cref="Enumerable.SingleOrDefault{TSource}(IEnumerable{TSource})"/> doesn't throw if there is more than one element in the sequence.
    /// </summary>
    internal static TSource? AsSingleton<TSource>(this IEnumerable<TSource>? source)
    {
        if (source == null)
        {
            return default;
        }

        if (source is IList<TSource> list)
        {
            return (list.Count == 1) ? list[0] : default;
        }

        using var e = source.GetEnumerator();
        if (!e.MoveNext())
        {
            return default;
        }

        var result = e.Current;
        if (e.MoveNext())
        {
            return default;
        }

        return result;
    }
}
