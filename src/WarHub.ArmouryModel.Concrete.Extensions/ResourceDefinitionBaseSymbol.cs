using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal abstract class ResourceDefinitionBaseSymbol : SourceDeclaredSymbol, IResourceDefinitionSymbol
{
    protected ResourceDefinitionBaseSymbol(ISymbol? containingSymbol, SourceNode declaration) : base(containingSymbol, declaration)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.ResourceDefinition;

    public abstract ResourceKind ResourceKind { get; }
}
