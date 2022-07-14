using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterProfileSymbol : RosterResourceBaseSymbol, IRosterProfileSymbol, INodeDeclaredSymbol<ProfileNode>
{
    private IProfileTypeSymbol? lazyProfileType;

    public RosterProfileSymbol(ISymbol? containingSymbol, ProfileNode declaration, DiagnosticBag diagnostics) : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Characteristics = declaration.Characteristics.Select(x => new CharacteristicSymbol(this, x, diagnostics)).ToImmutableArray();
    }

    public new ProfileNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Profile;

    public IProfileTypeSymbol Type => GetBoundField(ref lazyProfileType);

    public ImmutableArray<CharacteristicSymbol> Characteristics { get; }

    public override IProfileSymbol SourceEntry => (IProfileSymbol)SourceEntryPath.SourceEntries.Last();

    ImmutableArray<ICharacteristicSymbol> IRosterProfileSymbol.Characteristics =>
        Characteristics.Cast<CharacteristicSymbol, ICharacteristicSymbol>();

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyProfileType = binder.BindProfileTypeSymbol(Declaration, diagnostics);
    }

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(Characteristics.Cast<CharacteristicSymbol, Symbol>());
}
