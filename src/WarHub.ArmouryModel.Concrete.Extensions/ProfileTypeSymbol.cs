using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ProfileTypeSymbol : SourceDeclaredSymbol, IProfileTypeSymbol, INodeDeclaredSymbol<ProfileTypeNode>
{
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

    public override ProfileTypeNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<ICharacteristicTypeSymbol> CharacteristicTypes { get; }
}
