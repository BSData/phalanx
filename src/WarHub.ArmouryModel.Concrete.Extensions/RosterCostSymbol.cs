using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterCostSymbol : SourceDeclaredSymbol, IRosterCostSymbol
{
    private IResourceDefinitionSymbol? lazyType;

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

    public override SymbolKind Kind => SymbolKind.ResourceEntry;

    public decimal Value => CostDeclaration.Value;

    public decimal? Limit
    {
        get
        {
            // explainer: BS behavior is that a -1 limit value represents absence of limit
            return LimitDeclaration is { Value: var val and >= 0 } ? val : null;
        }
    }

    public IResourceDefinitionSymbol CostType => GetBoundField(ref lazyType);

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitRosterCost(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitRosterCost(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitRosterCost(this, argument);

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyType = binder.BindCostTypeSymbol(CostDeclaration, diagnostics);
    }
}
