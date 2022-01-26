using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal class CharacteristicSymbol : SimpleResourceEntrySymbol, ICharacteristicSymbol
{
    private ICharacteristicTypeSymbol? lazyType;
    private readonly IProfileSymbol profileSymbol;

    internal new CharacteristicNode Declaration { get; }

    public CharacteristicSymbol(
        IProfileSymbol containingSymbol,
        CharacteristicNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        profileSymbol = containingSymbol;
        Declaration = declaration;
    }

    public override ResourceKind ResourceKind => ResourceKind.Characteristic;

    public ICharacteristicTypeSymbol Type
    {
        get
        {
            ForceComplete();
            return lazyType ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    public string Value => Declaration.Value ?? string.Empty;

    protected override IResourceDefinitionSymbol? BaseType => Type;

    protected override void BindReferencesCore(Binding.Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyType = binder.BindCharacteristicTypeSymbol(profileSymbol.Type, Declaration);
    }
}
