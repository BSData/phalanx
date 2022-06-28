namespace WarHub.ArmouryModel;

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/InternalUtilities/ISetExtensions.cs
/// </summary>
internal static class ISetExtensions
{
    public static bool AddAll<T>(this ISet<T> set, IEnumerable<T> values)
    {
        var result = false;
        foreach (var v in values)
        {
            result |= set.Add(v);
        }

        return result;
    }

    public static bool AddAll<T>(this ISet<T> set, ImmutableArray<T> values)
    {
        var result = false;
        foreach (var v in values)
        {
            result |= set.Add(v);
        }

        return result;
    }

    public static bool RemoveAll<T>(this ISet<T> set, IEnumerable<T> values)
    {
        var result = false;
        foreach (var v in values)
        {
            result |= set.Remove(v);
        }

        return result;
    }

    public static bool RemoveAll<T>(this ISet<T> set, ImmutableArray<T> values)
    {
        var result = false;
        foreach (var v in values)
        {
            result |= set.Remove(v);
        }

        return result;
    }
}
