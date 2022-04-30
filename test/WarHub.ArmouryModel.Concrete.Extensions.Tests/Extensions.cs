namespace WarHub.ArmouryModel;

internal static class Extensions
{
    /// <summary>
    /// Initializes an out variable from the instance this method is called on.
    /// Useful for fluent APIs.
    /// </summary>
    /// <typeparam name="T">Type of this instance.</typeparam>
    /// <param name="this">This instance, assigned to <paramref name="output"/>.</param>
    /// <param name="output">Variable to which <paramref name="this"/> is assigned.</param>
    /// <returns>This instance.</returns>
    public static T Tee<T>(this T @this, out T output)
    {
        output = @this;
        return @this;
    }
}
