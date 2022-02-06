using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class CostSymbol : SimpleResourceEntrySymbol, ICostSymbol, INodeDeclaredSymbol<CostNode>
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

    public ICostTypeSymbol Type => GetBoundField(ref lazyCostTypeSymbol);

    public decimal Value => Declaration.Value;

    protected override IResourceDefinitionSymbol? BaseType => Type;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyCostTypeSymbol = binder.BindCostTypeSymbol(Declaration, diagnosticBag);
    }
}
