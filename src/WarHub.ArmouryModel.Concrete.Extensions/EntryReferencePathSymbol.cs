using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class EntryReferencePathBaseSymbol : SourceDeclaredSymbol, IEntryReferencePathSymbol
{
    private ImmutableArray<IEntrySymbol> lazySourceEntries;

    public EntryReferencePathBaseSymbol(ISymbol? containingSymbol, SourceNode declaration)
        : base(containingSymbol, declaration)
    {
    }

    public static EntryReferencePathBaseSymbol Create(ISymbol? containingSymbol, SourceNode declaration) => declaration switch
    {
        SelectionNode node => new SelectionEntryReferencePathSymbol(containingSymbol, node),
        ProfileNode node => new ProfileEntryReferencePathSymbol(containingSymbol, node),
        RuleNode node => new RuleEntryReferencePathSymbol(containingSymbol, node),
        _ when containingSymbol is EntryInstanceSymbol rosterEntry => new SingleEntryReferencePathSymbol(rosterEntry, declaration),
        _ => throw new InvalidOperationException("Unknown reference path declaration.")
    };

    public sealed override SymbolKind Kind => SymbolKind.Link;

    public ImmutableArray<IEntrySymbol> SourceEntries => GetBoundField(ref lazySourceEntries);

    protected sealed override void BindReferencesCore(Binder binder, BindingDiagnosticBag diagnostics)
    {
        base.BindReferencesCore(binder, diagnostics);
        lazySourceEntries = BindSourceEntries(binder, diagnostics);
    }

    protected abstract ImmutableArray<IEntrySymbol> BindSourceEntries(Binder binder, BindingDiagnosticBag diagnostics);

    internal class SingleEntryReferencePathSymbol : EntryReferencePathBaseSymbol
    {
        public SingleEntryReferencePathSymbol(EntryInstanceSymbol containingSymbol, SourceNode declaration)
            : base(containingSymbol, declaration)
        {
            ContainingSymbol = containingSymbol;
        }

        public new EntryInstanceSymbol ContainingSymbol { get; }

        protected override ImmutableArray<IEntrySymbol> BindSourceEntries(Binder binder, BindingDiagnosticBag diagnostics) =>
            ImmutableArray.Create(ContainingSymbol.SourceEntry);
    }

    internal class SelectionEntryReferencePathSymbol : EntryReferencePathBaseSymbol
    {
        public SelectionEntryReferencePathSymbol(
            ISymbol? containingSymbol,
            SelectionNode declaration)
            : base(containingSymbol, declaration)
        {
            Declaration = declaration;
        }

        public override SelectionNode Declaration { get; }

        protected override ImmutableArray<IEntrySymbol> BindSourceEntries(Binder binder, BindingDiagnosticBag diagnostics) =>
            binder.BindSelectionSourcePathSymbol(Declaration, diagnostics);
    }

    internal class ProfileEntryReferencePathSymbol : EntryReferencePathBaseSymbol
    {
        public ProfileEntryReferencePathSymbol(
            ISymbol? containingSymbol,
            ProfileNode declaration)
            : base(containingSymbol, declaration)
        {
            Declaration = declaration;
        }

        public override ProfileNode Declaration { get; }

        protected override ImmutableArray<IEntrySymbol> BindSourceEntries(Binder binder, BindingDiagnosticBag diagnostics) =>
            binder.BindProfileSourcePathSymbol(Declaration, diagnostics);
    }

    internal class RuleEntryReferencePathSymbol : EntryReferencePathBaseSymbol
    {
        public RuleEntryReferencePathSymbol(
            ISymbol? containingSymbol,
            RuleNode declaration)
            : base(containingSymbol, declaration)
        {
            Declaration = declaration;
        }

        public override RuleNode Declaration { get; }

        protected override ImmutableArray<IEntrySymbol> BindSourceEntries(Binder binder, BindingDiagnosticBag diagnostics) =>
            binder.BindRuleSourcePathSymbol(Declaration, diagnostics);
    }
}
