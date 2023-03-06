namespace WarHub.ArmouryModel.Concrete;

internal class GeneratedCostSymbol : Symbol, ICostSymbol
{
    public GeneratedCostSymbol(ISymbol? containingSymbol, IResourceDefinitionSymbol type)
    {
        ContainingSymbol = containingSymbol;
        Type = type;
    }

    public override SymbolKind Kind => SymbolKind.ResourceEntry;

    public override string? Id => null;

    public override string Name => Type.Name;

    public override string? Comment => null;

    public override ISymbol? ContainingSymbol { get; }

    public decimal Value => 0m;

    public ResourceKind ResourceKind => ResourceKind.Cost;

    public IResourceDefinitionSymbol Type { get; }

    public IResourceEntrySymbol? ReferencedEntry => null;

    public bool IsHidden => false;

    public bool IsReference => false;

    public IPublicationReferenceSymbol? PublicationReference => null;

    public ImmutableArray<IEffectSymbol> Effects => ImmutableArray<IEffectSymbol>.Empty;

    public ImmutableArray<IResourceEntrySymbol> Resources => ImmutableArray<IResourceEntrySymbol>.Empty;

    IEntrySymbol? IEntrySymbol.ReferencedEntry => null;

    public override void Accept(SymbolVisitor visitor)
    {
        visitor.VisitResourceEntry(this);
    }

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor)
    {
        return visitor.VisitResourceEntry(this);
    }

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument)
    {
        return visitor.VisitResourceEntry(this, argument);
    }
}
