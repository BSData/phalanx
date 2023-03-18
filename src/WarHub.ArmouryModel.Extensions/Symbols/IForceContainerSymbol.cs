namespace WarHub.ArmouryModel;

/// <summary>
/// Symbol that contains child <see cref="IForceSymbol"/>s.
/// </summary>
public interface IForceContainerSymbol : ISymbol
{

    /// <summary>
    /// Gets child forces of this symbol.
    /// </summary>
    // TODO research how child forces interact
    ImmutableArray<IForceSymbol> Forces { get; }
}
