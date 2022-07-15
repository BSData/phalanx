using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ProfileSymbol : ResourceEntryBaseSymbol, IProfileSymbol, INodeDeclaredSymbol<ProfileNode>
{
    private IResourceDefinitionSymbol? lazyType;

    public ProfileSymbol(
        ISymbol containingSymbol,
        ProfileNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Characteristics = CreateCharacteristics().ToImmutableArray();

        IEnumerable<CharacteristicSymbol> CreateCharacteristics()
        {
            foreach (var item in declaration.Characteristics)
            {
                yield return new CharacteristicSymbol(this, item, diagnostics);
            }
        }
    }

    public override ProfileNode Declaration { get; }

    public override IResourceDefinitionSymbol Type => GetBoundField(ref lazyType);

    public override ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<CharacteristicSymbol> Characteristics { get; }

    ImmutableArray<ICharacteristicSymbol> IProfileSymbol.Characteristics =>
        Characteristics.Cast<CharacteristicSymbol, ICharacteristicSymbol>();

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyType = binder.BindProfileTypeSymbol(Declaration, diagnostics);
    }

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics).AddRange(Characteristics);
}
