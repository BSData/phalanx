using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ContainerSymbol : EntryInstanceSymbol, IContainerEntryInstanceSymbol
{
    protected ContainerSymbol(
        ISymbol? containingSymbol,
        RosterElementBaseNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
        Declaration = declaration;
        Resources = CreateRosterEntryResources().ToImmutableArray();

        IEnumerable<RosterResourceBaseSymbol> CreateRosterEntryResources()
        {
            foreach (var item in declaration.Rules)
            {
                yield return new RosterRuleSymbol(this, item, diagnostics);
            }
            foreach (var item in declaration.Profiles)
            {
                yield return new RosterProfileSymbol(this, item, diagnostics);
            }
        }
    }

    public new RosterElementBaseNode Declaration { get; }

    public sealed override SymbolKind Kind => SymbolKind.Container;

    public abstract ContainerKind ContainerKind { get; }

    public string? CustomName => Declaration.CustomName;

    public string? CustomNotes => Declaration.CustomNotes;

    public override ImmutableArray<RosterResourceBaseSymbol> Resources { get; }

    public sealed override void Accept(SymbolVisitor visitor) =>
        visitor.VisitContainerEntryInstance(this);

    public sealed override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitContainerEntryInstance(this);

    public sealed override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitContainerEntryInstance(this, argument);
}
