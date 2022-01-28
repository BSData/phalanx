using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal abstract class Symbol : ISymbol
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

    private int bindingDone = 0;

    private bool BindingDone => bindingDone > 0;

    internal void ForceComplete()
    {
        if (RequiresCompletion && !BindingDone)
        {
            var compilation = DeclaringCompilation ?? throw new InvalidOperationException("Binding requires declaring compilation.");
            if (Interlocked.CompareExchange(ref bindingDone, 1, 0) == 0)
            {
                var diagnostics = DiagnosticBag.GetInstance();
                BindReferences(compilation, diagnostics);
                compilation.AddBindingDiagnostics(diagnostics);
            }
            // TODO consider a spin-wait in else?
        }
    }

    protected virtual void BindReferences(Compilation compilation, DiagnosticBag diagnostics) { }
}
