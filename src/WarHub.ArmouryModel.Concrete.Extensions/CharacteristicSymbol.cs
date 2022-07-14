using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicSymbol : ResourceEntryBaseSymbol, ICharacteristicSymbol, INodeDeclaredSymbol<CharacteristicNode>
{
    private ICharacteristicTypeSymbol? lazyType;

    public CharacteristicSymbol(
        ISymbol containingSymbol,
        CharacteristicNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override CharacteristicNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;

    public override ICharacteristicTypeSymbol Type => GetBoundField(ref lazyType);

    public string Value => Declaration.Value ?? string.Empty;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyType = binder.BindCharacteristicTypeSymbol(Declaration, diagnostics);
    }
}
