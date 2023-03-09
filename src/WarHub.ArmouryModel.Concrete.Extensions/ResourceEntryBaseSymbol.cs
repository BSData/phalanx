using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ResourceEntryBaseSymbol : EntrySymbol, IResourceEntrySymbol
{
    protected ResourceEntryBaseSymbol(
        ISymbol containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration, diagnostics)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.ResourceEntry;

    public abstract ResourceKind ResourceKind { get; }

    public virtual IResourceDefinitionSymbol? Type => null;

    public override IResourceEntrySymbol? ReferencedEntry => null;

    public override ImmutableArray<ResourceEntryBaseSymbol> Resources =>
        ImmutableArray<ResourceEntryBaseSymbol>.Empty;

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitResourceEntry(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitResourceEntry(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitResourceEntry(this, argument);
}
