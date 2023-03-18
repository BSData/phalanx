using System.Diagnostics.CodeAnalysis;

namespace WarHub.ArmouryModel;

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/Collections/CollectionsExtensions.cs
/// </summary>
internal static class CollectionsExtensions
{
    internal static bool IsNullOrEmpty<T>([NotNullWhen(returnValue: false)] this ICollection<T>? collection)
    {
        return collection == null || collection.Count == 0;
    }

    internal static bool IsNullOrEmpty<T>([NotNullWhen(returnValue: false)] this IReadOnlyCollection<T>? collection)
    {
        return collection == null || collection.Count == 0;
    }

    internal static bool IsNullOrEmpty<T>([NotNullWhen(returnValue: false)] this ImmutableHashSet<T>? hashSet)
    {
        return hashSet == null || hashSet.Count == 0;
    }
}
