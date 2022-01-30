namespace WarHub.ArmouryModel;

/// <summary>
/// Defines profile type.
/// BS ProfileType.
/// WHAM <see cref="Source.ProfileTypeNode" />.
/// </summary>
public interface IProfileTypeSymbol : IResourceDefinitionSymbol
{
    ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
}
