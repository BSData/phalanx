using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CharacteristicTypeSymbol : SourceCatalogueItemSymbol, ICharacteristicTypeSymbol
{
    private readonly CharacteristicTypeNode declaration;

    public CharacteristicTypeSymbol(
        IProfileTypeSymbol containingSymbol,
        CharacteristicTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        this.declaration = declaration;
        ContainingProfileType = containingSymbol;
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;

    public IProfileTypeSymbol ContainingProfileType { get; }
}
