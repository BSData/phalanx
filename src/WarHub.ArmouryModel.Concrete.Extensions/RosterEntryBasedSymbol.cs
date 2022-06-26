using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class RosterEntryBasedSymbol : SourceDeclaredSymbol, IRosterEntrySymbol, INodeDeclaredSymbol<RosterElementBaseNode>
{
    protected RosterEntryBasedSymbol(
        ISymbol? containingSymbol,
        RosterElementBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        Declaration = declaration;
        PublicationReference = PublicationReferenceSymbol.Create(this, declaration, diagnostics);
    }

    public override RosterElementBaseNode Declaration { get; }

    public abstract IEntrySymbol SourceEntry { get; }

    public string? CustomName => Declaration.CustomName;

    public string? CustomNotes => Declaration.CustomNotes;

    public PublicationReferenceSymbol? PublicationReference { get; }

    IPublicationReferenceSymbol? IRosterEntrySymbol.PublicationReference => PublicationReference;

    public abstract ImmutableArray<IResourceEntrySymbol> Resources { get; }

    protected IEnumerable<IResourceEntrySymbol> CreateRosterEntryResources(DiagnosticBag diagnostics)
    {
        foreach (var item in Declaration.Rules)
        {
            yield return new RuleSymbol(this, item, diagnostics);
        }
        foreach (var item in Declaration.Profiles)
        {
            yield return new ProfileSymbol(this, item, diagnostics);
        }
    }

    protected override void InvokeForceCompleteOnChildren()
    {
        base.InvokeForceCompleteOnChildren();
        PublicationReference?.ForceComplete();
        InvokeForceComplete(Resources);
    }
}
