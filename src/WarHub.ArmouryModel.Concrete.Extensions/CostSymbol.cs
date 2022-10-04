using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CostSymbol : ResourceEntryBaseSymbol, ICostSymbol, INodeDeclaredSymbol<CostNode>
{
    private IResourceDefinitionSymbol? lazyTypeSymbol;

    public CostSymbol(
        ISymbol containingSymbol,
        CostNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public override CostNode Declaration { get; }

    public override ResourceKind ResourceKind => ResourceKind.Cost;

    public override IResourceDefinitionSymbol Type => GetBoundField(ref lazyTypeSymbol);

    public decimal Value => Declaration.Value;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyTypeSymbol = binder.BindCostTypeSymbol(Declaration, diagnostics);
    }
}
