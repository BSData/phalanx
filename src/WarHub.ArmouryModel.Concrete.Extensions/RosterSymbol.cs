using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class RosterSymbol : SourceDeclaredSymbol, IRosterSymbol
{
    private ICatalogueSymbol? lazyGamesystem;

    internal new RosterNode Declaration { get; }

    public RosterSymbol(
        SourceGlobalNamespaceSymbol containingSymbol,
        RosterNode declaration,
        DiagnosticBag diagnostics) : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        // TODO implement costs/forces
    }

    public override SymbolKind Kind => SymbolKind.Roster;

    public string? CustomNotes => Declaration.CustomNotes;

    public ICatalogueSymbol Gamesystem => GetBoundField(ref lazyGamesystem);

    public ImmutableArray<IRosterCostSymbol> Costs => ImmutableArray<IRosterCostSymbol>.Empty;

    public ImmutableArray<IForceSymbol> Forces => ImmutableArray<IForceSymbol>.Empty;

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);
        lazyGamesystem = binder.BindGamesystemSymbol(Declaration, diagnosticBag);
    }
}
