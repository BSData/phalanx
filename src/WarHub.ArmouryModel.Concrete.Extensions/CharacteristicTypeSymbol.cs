using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicTypeSymbol : ResourceDefinitionBaseSymbol, ICharacteristicTypeSymbol, INodeDeclaredSymbol<CharacteristicTypeNode>
{
    public CharacteristicTypeSymbol(
        IProfileTypeSymbol containingSymbol,
        CharacteristicTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        ContainingProfileType = containingSymbol;
    }

    public override CharacteristicTypeNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;

    public IProfileTypeSymbol ContainingProfileType { get; }
}
