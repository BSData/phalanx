using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ProfileTypeSymbol : SourceDeclaredSymbol, IProfileTypeSymbol
{
    internal new ProfileTypeNode Declaration { get; }

    public ProfileTypeSymbol(
        ICatalogueSymbol containingSymbol,
        ProfileTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        CharacteristicTypes = declaration.CharacteristicTypes
            .Select(x => new CharacteristicTypeSymbol(this, x, diagnostics))
            .ToImmutableArray<ICharacteristicTypeSymbol>();
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;

    public ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
}
