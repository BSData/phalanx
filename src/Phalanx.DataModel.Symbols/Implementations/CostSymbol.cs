using Phalanx.DataModel.Symbols.Binding;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CostSymbol : SimpleResourceEntrySymbol, ICostSymbol
{
    private ICostTypeSymbol? lazyCostTypeSymbol;

    internal new CostNode Declaration { get; }

    public CostSymbol(
        ISymbol containingSymbol,
        CostNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
    }

    public ICostTypeSymbol Type
    {
        get
        {
            ForceComplete();
            return lazyCostTypeSymbol ?? throw new InvalidOperationException("Binding failed.");
        }
    }

    public decimal Value => Declaration.Value;

    protected override IResourceDefinitionSymbol? BaseType => Type;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);

        lazyCostTypeSymbol = binder.BindCostTypeSymbol(Declaration.TypeId);
    }
}
