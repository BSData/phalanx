using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicTypeSymbol : SourceDeclaredSymbol, ICharacteristicTypeSymbol
{
    internal new CharacteristicTypeNode Declaration { get; }

    public CharacteristicTypeSymbol(
        IProfileTypeSymbol containingSymbol,
        CharacteristicTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        ContainingProfileType = containingSymbol;
    }

    public override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public ResourceKind ResourceKind => ResourceKind.Characteristic;

    public IProfileTypeSymbol ContainingProfileType { get; }
}
