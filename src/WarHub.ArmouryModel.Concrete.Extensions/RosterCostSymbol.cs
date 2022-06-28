using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterCostSymbol : SourceDeclaredSymbol, IRosterCostSymbol
{
    private ICostTypeSymbol? lazyCostType;

    public RosterCostSymbol(
        ISymbol? containingSymbol,
        CostNode costDeclaration,
        CostLimitNode? limitDeclaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, costDeclaration)
    {
        CostDeclaration = costDeclaration;
        LimitDeclaration = limitDeclaration; // TODO consider nesting another symbol, or how to declare multiple IDs.
        if (limitDeclaration?.TypeId is { } typeId && typeId != costDeclaration.TypeId)
        {
            diagnostics.Add(
                ErrorCode.ERR_GenericError,
                costDeclaration.GetLocation(),
                symbols: ImmutableArray.Create<Symbol>(this),
                args: "Cost limit has a different TypeId than Cost value.");
        }
    }

    public CostNode CostDeclaration { get; }

    public CostLimitNode? LimitDeclaration { get; }

    public override SymbolKind Kind => SymbolKind.Resource;

    public decimal Value => CostDeclaration.Value;

    public decimal? Limit
    {
        get
        {
            // explainer: BS behavior is that a -1 limit value represents absence of limit
            return LimitDeclaration is { Value: var val and >= 0 } ? val : null;
        }
    }

    public ICostTypeSymbol CostType => GetBoundField(ref lazyCostType);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyCostType = binder.BindCostTypeSymbol(CostDeclaration, diagnostics);
    }
}
