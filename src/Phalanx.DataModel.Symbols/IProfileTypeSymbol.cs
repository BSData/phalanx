using System.Collections.Immutable;

namespace Phalanx.DataModel.Symbols
{
    /// <summary>
    /// Defines profile type.
    /// BS ProfileType.
    /// WHAM <see cref="WarHub.ArmouryModel.Source.ProfileTypeNode" />.
    /// </summary>
    public interface IProfileTypeSymbol : IResourceDefinitionSymbol
    {
        ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
    }
}
