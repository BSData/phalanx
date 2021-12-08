namespace Phalanx.DataModel.Symbols.Implementation;

public abstract class Symbol : ISymbol
{
    public abstract SymbolKind Kind { get; }

    public abstract string? Id { get; }

    public abstract string Name { get; }

    public abstract string? Comment { get; }

    public abstract ISymbol? ContainingSymbol { get; }

    public virtual ICatalogueSymbol? ContainingCatalogue
    {
        get
        {
            if (ContainingSymbol is null)
                return null;
            return ContainingSymbol.Kind switch
            {
                SymbolKind.Catalogue => (ICatalogueSymbol)ContainingSymbol,
                _ => ContainingSymbol.ContainingCatalogue
            };
        }
    }

    public virtual IGamesystemNamespaceSymbol? ContainingNamespace =>
        ContainingSymbol?.ContainingNamespace;

    internal virtual Compilation? DeclaringCompilation
    {
        get
        {
            return Kind switch
            {
                SymbolKind.Namespace => throw new InvalidOperationException("Namespace must override DeclaringCompilation"),
                _ => (ContainingNamespace as SourceGlobalNamespaceSymbol)?.DeclaringCompilation
            };
        }
    }

    internal virtual bool RequiresCompletion => false;

    internal virtual void ForceComplete() { }
}
