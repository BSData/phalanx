using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class EntryInstanceSymbol : SourceDeclaredSymbol, ICustomizableEntryInstanceSymbol
{
    protected EntryInstanceSymbol(
        ISymbol? containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
        SourceEntryPath = EntryReferencePathBaseSymbol.Create(this, declaration);
        PublicationReference = PublicationReferenceSymbol.Create(this, declaration, diagnostics);
        Resources = CreateRosterEntryResources().ToImmutableArray();

        IEnumerable<RosterResourceBaseSymbol> CreateRosterEntryResources()
        {
            if (declaration is RosterElementBaseNode rosterElement)
            {
                foreach (var item in rosterElement.Rules)
                {
                    yield return new RosterRuleSymbol(this, item, diagnostics);
                }
                foreach (var item in rosterElement.Profiles)
                {
                    yield return new RosterProfileSymbol(this, item, diagnostics);
                }
            }
        }
    }

    public abstract IEntrySymbol SourceEntry { get; }

    public EntryReferencePathBaseSymbol SourceEntryPath { get; }

    public string? CustomName => (Declaration as RosterElementBaseNode)?.CustomName;

    public string? CustomNotes => (Declaration as RosterElementBaseNode)?.CustomNotes;

    public PublicationReferenceSymbol? PublicationReference { get; }

    public ImmutableArray<RosterResourceBaseSymbol> Resources { get; }

    IEntrySymbol IEntryInstanceSymbol.SourceEntry => SourceEntry;

    IEntryReferencePathSymbol IEntryInstanceSymbol.SourceEntryPath => SourceEntryPath;

    IPublicationReferenceSymbol? IEntryInstanceSymbol.PublicationReference => PublicationReference;

    ImmutableArray<IResourceSymbol> IEntryInstanceSymbol.Resources =>
        Resources.Cast<RosterResourceBaseSymbol, IResourceSymbol>();

    protected override ImmutableArray<Symbol> MakeAllMembers(BindingDiagnosticBag diagnostics)
    {
        var members = base.MakeAllMembers(diagnostics);
        return (PublicationReference is null ? members : members.Add(PublicationReference))
            .Add(SourceEntryPath)
            .AddRange(Resources.Cast<RosterResourceBaseSymbol, Symbol>());
    }
}
