using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicSymbol : SimpleResourceEntrySymbol, ICharacteristicSymbol
{
    private ICharacteristicTypeSymbol? lazyType;

    internal new CharacteristicNode Declaration { get; }

    public CharacteristicSymbol(
        IProfileSymbol containingSymbol,
        CharacteristicNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;

    public ICharacteristicTypeSymbol Type => GetBoundField(ref lazyType);

    public string Value => Declaration.Value ?? string.Empty;

    protected override IResourceDefinitionSymbol? BaseType => Type;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyType = binder.BindCharacteristicTypeSymbol(Declaration, diagnosticBag);
    }
}
