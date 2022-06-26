using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterSymbol : SourceDeclaredSymbol, IRosterSymbol, INodeDeclaredSymbol<RosterNode>
{
    private ICatalogueSymbol? lazyGamesystem;

    public RosterSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        RosterNode declaration,
        DiagnosticBag diagnostics) : base(containingSymbol, declaration)
    {
        ContainingNamespace = containingSymbol;
        Declaration = declaration;
        Costs = CreateCosts().ToImmutableArray();
        Forces = declaration.Forces.Select(x => new ForceSymbol(this, x, diagnostics)).ToImmutableArray<IForceSymbol>();

        IEnumerable<IRosterCostSymbol> CreateCosts()
        {
            foreach (var cost in declaration.Costs)
            {
                var limits = declaration.CostLimits.Where(x => x.TypeId == cost.TypeId).ToList();
                if (limits.Count > 1)
                {
                    diagnostics.Add(
                        ErrorCode.ERR_GenericError,
                        cost.GetLocation(),
                        symbols: ImmutableArray.Create<Symbol>(this),
                        args: "There are multiple Cost Limits with the TypeId of this Cost value.");
                }
                var limit = limits.FirstOrDefault();
                yield return new RosterCostSymbol(this, cost, limit, diagnostics);
            }
        }
    }

    public override RosterNode Declaration { get; }

    public override ICatalogueSymbol? ContainingCatalogue => null;

    public override SourceGlobalNamespaceSymbol ContainingNamespace { get; }

    public override SymbolKind Kind => SymbolKind.Roster;

    public string? CustomNotes => Declaration.CustomNotes;

    public ICatalogueSymbol Gamesystem => GetBoundField(ref lazyGamesystem);

    public ImmutableArray<IRosterCostSymbol> Costs { get; }

    public ImmutableArray<IForceSymbol> Forces { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);
        lazyGamesystem = binder.BindGamesystemSymbol(Declaration, diagnosticBag);
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        InvokeForceComplete(Costs);
        InvokeForceComplete(Forces);
    }
}
