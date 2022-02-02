namespace WarHub.ArmouryModel.Concrete;

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

    internal virtual WhamCompilation? DeclaringCompilation
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
                InvokeForceCompleteOnChildren();
            }
            else
            {
                // wait until another thread finished binding
                // are we afraid of deadlock here?
                SpinWait.SpinUntil(() => BindingDone);
            }
        }
    }

    protected virtual void BindReferences(WhamCompilation compilation, DiagnosticBag diagnostics) { }

    protected virtual void InvokeForceCompleteOnChildren() { }

    protected static void InvokeForceComplete<TChild>(ImmutableArray<TChild> children)
    {
        foreach (var child in children)
        {
            if (child is Symbol { RequiresCompletion: true } toComplete)
                toComplete.ForceComplete();
        }
    }

    protected TField GetBoundField<TField>(ref TField? field) where TField : class
    {
        ForceComplete();
        return field ?? throw new InvalidOperationException("Bound field was null after binding.");
    }

    protected TField? GetOptionalBoundField<TField>(ref TField? field) where TField : class
    {
        ForceComplete();
        return field;
    }
}