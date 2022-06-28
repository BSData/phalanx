using System.Diagnostics.CodeAnalysis;

namespace WarHub.ArmouryModel;

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/Collections/HashSetExtensions.cs
/// </summary>
internal static class HashSetExtensions
{
    internal static bool IsNullOrEmpty<T>([NotNullWhen(returnValue: false)] this HashSet<T>? hashSet)
    {
        return hashSet == null || hashSet.Count == 0;
    }

    internal static bool InitializeAndAdd<T>([NotNullIfNotNull(parameterName: "item"), NotNullWhen(returnValue: true)] ref HashSet<T>? hashSet, [NotNullWhen(returnValue: true)] T? item)
        where T : class
    {
        if (item is null)
        {
            return false;
        }
        else if (hashSet is null)
        {
            hashSet = new HashSet<T>();
        }

        return hashSet.Add(item);
    }
}
