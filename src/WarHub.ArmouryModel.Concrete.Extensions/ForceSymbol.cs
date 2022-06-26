using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class ForceSymbol : RosterEntryBasedSymbol, IForceSymbol, INodeDeclaredSymbol<ForceNode>
{
    private IForceEntrySymbol? lazyForceEntry;

    public ForceSymbol(
        ISymbol? containingSymbol,
        ForceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Resources = CreateRosterEntryResources(diagnostics).ToImmutableArray();
        Categories = declaration.Categories.Select(x => new CategorySymbol(this, x , diagnostics)).ToImmutableArray<ICategorySymbol>();
        Publications = declaration.Publications.Select(x => new PublicationSymbol(this, x, diagnostics)).ToImmutableArray<IPublicationSymbol>();
        ChildForces = declaration.Forces.Select(x => new ForceSymbol(this, x, diagnostics)).ToImmutableArray<IForceSymbol>();
        ChildSelections = declaration.Selections.Select(x => new SelectionSymbol(this, x, diagnostics)).ToImmutableArray<ISelectionSymbol>();
    }

    public override ForceNode Declaration { get; }

    public override SymbolKind Kind => SymbolKind.Force;

    public override IForceEntrySymbol SourceEntry => GetBoundField(ref lazyForceEntry);

    public ImmutableArray<IForceSymbol> ChildForces { get; }

    public ImmutableArray<ISelectionSymbol> ChildSelections { get; }

    public override ImmutableArray<IResourceEntrySymbol> Resources { get; }

    public ImmutableArray<ICategorySymbol> Categories { get; }

    public ImmutableArray<IPublicationSymbol> Publications { get; }

    protected override void BindReferencesCore(Binder binder, DiagnosticBag diagnosticBag)
    {
        base.BindReferencesCore(binder, diagnosticBag);
        // TODO bind lazyForceEntry
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        // Resources are done in base.Invoke... call
        InvokeForceComplete(Categories);
        InvokeForceComplete(Publications);
        InvokeForceComplete(ChildForces);
        InvokeForceComplete(ChildSelections);
    }
}
