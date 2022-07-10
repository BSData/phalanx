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
        Forces = declaration.Forces.Select(x => new ForceSymbol(this, x, diagnostics)).ToImmutableArray();

        IEnumerable<RosterCostSymbol> CreateCosts()
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

    public override SourceGlobalNamespaceSymbol ContainingNamespace { get; }

    public override IModuleSymbol? ContainingModule => null;

    public override SymbolKind Kind => SymbolKind.Roster;

    public string? CustomNotes => Declaration.CustomNotes;

    public ICatalogueSymbol Gamesystem => GetBoundField(ref lazyGamesystem);

    public ImmutableArray<RosterCostSymbol> Costs { get; }

    public ImmutableArray<ForceSymbol> Forces { get; }

    ImmutableArray<IRosterCostSymbol> IRosterSymbol.Costs =>
        Costs.Cast<RosterCostSymbol, IRosterCostSymbol>();

    ImmutableArray<IForceSymbol> IForceContainerSymbol.Forces =>
        Forces.Cast<ForceSymbol, IForceSymbol>();

    protected override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazyGamesystem = binder.BindGamesystemSymbol(Declaration, diagnostics);
    }

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics) =>
        base.MakeAllMembers(diagnostics)
        .AddRange(Costs.Cast<RosterCostSymbol, Symbol>())
        .AddRange(Forces.Cast<ForceSymbol, Symbol>());
}
