using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class RosterResourceBaseSymbol : EntryInstanceSymbol, IResourceSymbol
{
    protected RosterResourceBaseSymbol(
        ISymbol? containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.Resource;

    public abstract ResourceKind ResourceKind { get; }

    IResourceEntrySymbol IResourceSymbol.SourceEntry => (IResourceEntrySymbol)SourceEntry;

    public override ImmutableArray<RosterResourceBaseSymbol> Resources =>
        ImmutableArray<RosterResourceBaseSymbol>.Empty;

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitResource(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitResource(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitResource(this, argument);
}
