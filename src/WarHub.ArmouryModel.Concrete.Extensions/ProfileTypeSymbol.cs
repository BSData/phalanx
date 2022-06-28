using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ProfileTypeSymbol : ResourceDefinitionBaseSymbol, IProfileTypeSymbol, INodeDeclaredSymbol<ProfileTypeNode>
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
            .ToImmutableArray();
    }

    public override ProfileTypeNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<CharacteristicTypeSymbol> CharacteristicTypes { get; }

    ImmutableArray<ICharacteristicTypeSymbol> IProfileTypeSymbol.CharacteristicTypes =>
        CharacteristicTypes.Cast<CharacteristicTypeSymbol, ICharacteristicTypeSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(CharacteristicTypes.Cast<CharacteristicTypeSymbol, Symbol>());
}
