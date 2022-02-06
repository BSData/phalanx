using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicTypeSymbol : SourceDeclaredSymbol, ICharacteristicTypeSymbol, INodeDeclaredSymbol<CharacteristicTypeNode>
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

    public override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public ResourceKind ResourceKind => ResourceKind.Characteristic;

    public IProfileTypeSymbol ContainingProfileType { get; }
}
