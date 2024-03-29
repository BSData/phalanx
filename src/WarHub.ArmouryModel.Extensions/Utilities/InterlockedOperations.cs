using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace WarHub.ArmouryModel;

/// <summary>
/// From https://sourceroslyn.io/#Microsoft.CodeAnalysis/InternalUtilities/InterlockedOperations.cs
/// </summary>
internal static class InterlockedOperations
{
    /// <summary>
    /// Initialize the value referenced by <paramref name="target"/> in a thread-safe manner.
    /// The value is changed to <paramref name="value"/> only if the current value is null.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    /// <param name="target">Reference to the target location.</param>
    /// <param name="value">The value to use if the target is currently null.</param>
    /// <returns>The new value referenced by <paramref name="target"/>. Note that this is
    /// nearly always more useful than the usual return from <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
    /// because it saves another read to <paramref name="target"/>.</returns>
    public static T Initialize<T>([NotNull] ref T? target, T value) where T : class
    {
        Debug.Assert(value is not null);
        return Interlocked.CompareExchange(ref target, value, null) ?? value;
    }

    /// <summary>
    /// Initialize the value referenced by <paramref name="target"/> in a thread-safe manner.
    /// The value is changed to <paramref name="initializedValue"/> only if the current value
    /// is <paramref name="uninitializedValue"/>.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    /// <param name="target">Reference to the target location.</param>
    /// <param name="initializedValue">The value to use if the target is currently uninitialized.</param>
    /// <param name="uninitializedValue">The uninitialized value.</param>
    /// <returns>The new value referenced by <paramref name="target"/>. Note that this is
    /// nearly always more useful than the usual return from <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
    /// because it saves another read to <paramref name="target"/>.</returns>
    [return: NotNullIfNotNull(parameterName: "initializedValue")]
    public static T Initialize<T>(ref T target, T initializedValue, T uninitializedValue) where T : class?
    {
        Debug.Assert(!ReferenceEquals(initializedValue, uninitializedValue));
        var oldValue = Interlocked.CompareExchange(ref target, initializedValue, uninitializedValue);
        return ReferenceEquals(oldValue, uninitializedValue) ? initializedValue : oldValue;
    }

    /// <summary>
    /// Initialize the immutable array referenced by <paramref name="target"/> in a thread-safe manner.
    /// </summary>
    /// <typeparam name="T">Elemental type of the array.</typeparam>
    /// <param name="target">Reference to the target location.</param>
    /// <param name="initializedValue">The value to use if the target is currently uninitialized (default).</param>
    /// <returns>The new value referenced by <paramref name="target"/>. Note that this is
    /// nearly always more useful than the usual return from <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
    /// because it saves another read to <paramref name="target"/>.</returns>
    public static ImmutableArray<T> Initialize<T>(ref ImmutableArray<T> target, ImmutableArray<T> initializedValue)
    {
        Debug.Assert(!initializedValue.IsDefault);
        var oldValue = ImmutableInterlocked.InterlockedCompareExchange(ref target, initializedValue, default(ImmutableArray<T>));
        return oldValue.IsDefault ? initializedValue : oldValue;
    }
}
