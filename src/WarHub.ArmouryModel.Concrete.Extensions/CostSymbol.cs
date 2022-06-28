using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CostSymbol : ResourceEntryBaseSymbol, ICostSymbol, INodeDeclaredSymbol<CostNode>
{
    private ICostTypeSymbol? lazyCostTypeSymbol;

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

    public override ICostTypeSymbol Type => GetBoundField(ref lazyCostTypeSymbol);

    public decimal Value => Declaration.Value;

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);

        lazyCostTypeSymbol = binder.BindCostTypeSymbol(Declaration, diagnostics);
    }
}
