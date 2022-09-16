using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Defines characteristic type.
/// BS ProfileType subentry (CharacteristicType).
/// WHAM <see cref="CharacteristicTypeNode" />.
/// </summary>
internal class CharacteristicTypeSymbol : ResourceDefinitionBaseSymbol, INodeDeclaredSymbol<CharacteristicTypeNode>
{
    public CharacteristicTypeSymbol(
        ISymbol containingSymbol,
        CharacteristicTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
    }

    public override CharacteristicTypeNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;
}
