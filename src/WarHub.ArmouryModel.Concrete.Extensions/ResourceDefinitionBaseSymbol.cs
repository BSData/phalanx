using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ResourceDefinitionBaseSymbol : SourceDeclaredSymbol, IResourceDefinitionSymbol
{
    protected ResourceDefinitionBaseSymbol(ISymbol? containingSymbol, SourceNode declaration) : base(containingSymbol, declaration)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public abstract ResourceKind ResourceKind { get; }

    ImmutableArray<IResourceDefinitionSymbol> IResourceDefinitionSymbol.Definitions =>
        ImmutableArray<IResourceDefinitionSymbol>.Empty;

    public override void Accept(SymbolVisitor visitor) =>
        visitor.VisitResourceDefinition(this);

    public override TResult Accept<TResult>(SymbolVisitor<TResult> visitor) =>
        visitor.VisitResourceDefinition(this);

    public override TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument) =>
        visitor.VisitResourceDefinition(this, argument);
}
