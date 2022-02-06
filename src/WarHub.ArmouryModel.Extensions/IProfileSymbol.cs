namespace WarHub.ArmouryModel;

/// <summary>
/// Profile with named characteristics with values.
/// BS ProfileEntry/InfoLink@type=profile.
/// WHAM <see cref="Source.ProfileNode" />.
/// </summary>
public interface IProfileSymbol : IResourceEntrySymbol
{
    /// <summary>
    /// The type of profile that defines name and characteristic types for this profile.
    /// </summary>
    new IProfileTypeSymbol Type { get; }

    ImmutableArray<ICharacteristicSymbol> Characteristics { get; }
}
