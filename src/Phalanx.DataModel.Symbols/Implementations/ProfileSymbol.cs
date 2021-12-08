using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class ProfileSymbol : EntrySymbol, IProfileSymbol
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

    public IProfileTypeSymbol Type
    {
        get
        {
            ForceComplete();
            return lazyType ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    public override SymbolKind Kind => SymbolKind.Resource;

    public ImmutableArray<ICharacteristicSymbol> Characteristics { get; }

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => Type;

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;

    protected override void BindReferencesCore(Binding.Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyType = binder.BindProfileTypeSymbol(Declaration.TypeId);

        foreach (var child in Characteristics)
        {
            if (child is Symbol { RequiresCompletion: true } toComplete)
                toComplete.ForceComplete();
        }
    }
}
