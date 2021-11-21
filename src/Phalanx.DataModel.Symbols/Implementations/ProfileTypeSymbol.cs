using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ProfileTypeSymbol : SourceCatalogueItemSymbol, IProfileTypeSymbol
{
    private readonly ProfileTypeNode declaration;

    public ProfileTypeSymbol(
        ICatalogueSymbol containingSymbol,
        ProfileTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        this.declaration = declaration;
        CharacteristicTypes = declaration.CharacteristicTypes
            .Select(x => new CharacteristicTypeSymbol(this, x, diagnostics))
            .ToImmutableArray<ICharacteristicTypeSymbol>();
    }

    public override SymbolKind Kind => SymbolKind.ResourceType;

    public ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
}
