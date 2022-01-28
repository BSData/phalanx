using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class ProfileSymbol : EntrySymbol, IProfileSymbol
{
    private IProfileTypeSymbol? lazyType;
    internal new ProfileNode Declaration { get; }

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

    public IProfileTypeSymbol Type => GetBoundField(ref lazyType);

    public override SymbolKind Kind => SymbolKind.Resource;

    public ResourceKind ResourceKind => ResourceKind.Profile;

    public ImmutableArray<ICharacteristicSymbol> Characteristics { get; }

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

    protected override void BindReferencesCore(Binding.Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyType = binder.BindProfileTypeSymbol(Declaration, diagnosticBag);

        foreach (var child in Characteristics)
        {
            if (child is Symbol { RequiresCompletion: true } toComplete)
                toComplete.ForceComplete();
        }
    }
}
