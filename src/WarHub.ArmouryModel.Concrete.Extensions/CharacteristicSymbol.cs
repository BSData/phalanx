using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CharacteristicSymbol : SimpleResourceEntrySymbol, ICharacteristicSymbol, INodeDeclaredSymbol<CharacteristicNode>
{
    private ICharacteristicTypeSymbol? lazyType;

    public CharacteristicSymbol(
        IProfileSymbol containingSymbol,
        CharacteristicNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override CharacteristicNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;

    public ICharacteristicTypeSymbol Type => GetBoundField(ref lazyType);

    public string Value => Declaration.Value ?? string.Empty;

    protected override IResourceDefinitionSymbol? BaseType => Type;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyType = binder.BindCharacteristicTypeSymbol(Declaration, diagnostics);
    }
}
