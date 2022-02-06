using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ProfileSymbol : EntrySymbol, IProfileSymbol, INodeDeclaredSymbol<ProfileNode>
{
    private IProfileTypeSymbol? lazyType;

    public ProfileSymbol(
        ISymbol containingSymbol,
        ProfileNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Characteristics = CreateCharacteristics().ToImmutableArray();

        IEnumerable<ICharacteristicSymbol> CreateCharacteristics()
        {
            foreach (var item in declaration.Characteristics)
            {
                yield return new CharacteristicSymbol(this, item, diagnostics);
            }
        }
    }

    public override ProfileNode Declaration { get; }

    public IProfileTypeSymbol Type => GetBoundField(ref lazyType);

    public override SymbolKind Kind => SymbolKind.Resource;

    public ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<ICharacteristicSymbol> Characteristics { get; }

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyType = binder.BindProfileTypeSymbol(Declaration, diagnosticBag);
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        InvokeForceComplete(Characteristics);
    }
}
