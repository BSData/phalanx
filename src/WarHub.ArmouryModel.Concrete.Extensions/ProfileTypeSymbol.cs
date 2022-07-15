using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

/// <summary>
/// Defines profile type.
/// BS ProfileType.
/// WHAM <see cref="ProfileTypeNode" />.
/// </summary>
internal class ProfileTypeSymbol : ResourceDefinitionBaseSymbol, INodeDeclaredSymbol<ProfileTypeNode>, IResourceDefinitionSymbol
{
    public ProfileTypeSymbol(
        ICatalogueSymbol containingSymbol,
        ProfileTypeNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        Definitions = declaration.CharacteristicTypes
            .Select(x => new CharacteristicTypeSymbol(this, x, diagnostics))
            .ToImmutableArray();
    }

    public override ProfileTypeNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<CharacteristicTypeSymbol> Definitions { get; }

    ImmutableArray<IResourceDefinitionSymbol> IResourceDefinitionSymbol.Definitions =>
        Definitions.Cast<CharacteristicTypeSymbol, IResourceDefinitionSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(Definitions.Cast<CharacteristicTypeSymbol, Symbol>());
}
