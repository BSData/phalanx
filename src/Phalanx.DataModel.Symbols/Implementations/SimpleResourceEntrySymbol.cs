using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class SimpleResourceEntrySymbol : SourceDeclaredSymbol, IResourceEntrySymbol
{
    public SimpleResourceEntrySymbol(
        ISymbol containingSymbol,
        SourceNode declaration,
        DiagnosticBag diagnostics)
        : base(containingSymbol, declaration)
    {
    }

    public sealed override SymbolKind Kind => SymbolKind.Resource;

    public bool IsHidden => false;

    public bool IsReference => false;

    public IEntrySymbol? ReferencedEntry => null;

    public IPublicationReferenceSymbol? PublicationReference => null;

    public ImmutableArray<IEffectSymbol> Effects => ImmutableArray<IEffectSymbol>.Empty;

    IResourceDefinitionSymbol? IResourceEntrySymbol.Type => BaseType;

    protected abstract IResourceDefinitionSymbol? BaseType { get; }

    IResourceEntrySymbol? IResourceEntrySymbol.ReferencedEntry => null;
}
