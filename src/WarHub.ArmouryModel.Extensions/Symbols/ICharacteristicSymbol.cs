namespace WarHub.ArmouryModel;

/// <summary>
/// Defines characteristic.
/// BS Profile subentry (Characteristic).
/// WHAM <see cref="Source.CharacteristicNode" />.
/// </summary>
public interface ICharacteristicSymbol : IResourceEntrySymbol
{
    string Value { get; }
}
