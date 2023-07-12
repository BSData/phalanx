namespace WarHub.ArmouryModel;

/// <summary>
/// Profile with named characteristics with values.
/// BS ProfileEntry/InfoLink@type=profile.
/// WHAM <see cref="Source.ProfileNode" />.
/// </summary>
public interface IProfileSymbol : IResourceEntrySymbol
{
    ImmutableArray<ICharacteristicSymbol> Characteristics { get; }
}
