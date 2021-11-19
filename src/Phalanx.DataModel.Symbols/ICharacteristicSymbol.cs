namespace Phalanx.DataModel.Symbols;

/// <summary>
/// Defines characteristic.
/// BS Profile subentry (Characteristic).
/// WHAM <see cref="WarHub.ArmouryModel.Source.CharacteristicNode" />.
/// </summary>
public interface ICharacteristicSymbol : IResourceEntrySymbol
{
    string Value { get; }

    new ICharacteristicTypeSymbol Type { get; }
}
