namespace WarHub.ArmouryModel;

/// <summary>
/// Profile with named characteristics with values.
/// Instance of <see cref="IProfileSymbol"/> in <see cref="SourceEntry"/>.
/// BS Profile in Roster.
/// WHAM <see cref="Source.ProfileNode" />.
/// </summary>
public interface IRosterProfileSymbol : IResourceSymbol
{
    /// <summary>
    /// The type of profile that defines name and characteristic types for this profile.
    /// </summary>
    IProfileTypeSymbol Type { get; }

    ImmutableArray<ICharacteristicSymbol> Characteristics { get; }

    new IProfileSymbol SourceEntry { get; }
}
