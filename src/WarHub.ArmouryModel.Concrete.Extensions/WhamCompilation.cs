using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

public class WhamCompilation : Compilation
{
    private SourceGlobalNamespaceSymbol? lazyGlobalNamespace;
    private Binder? lazyGlobalNamespaceBinder;
    private DiagnosticBag? lazyBindingDiagnostics;

    internal WhamCompilation(string? name, ImmutableArray<SourceTree> sourceTrees, CompilationOptions options)
        : base(name, sourceTrees, options)
    {
    }

    public override IGamesystemNamespaceSymbol GlobalNamespace => SourceGlobalNamespace;

    internal SourceGlobalNamespaceSymbol SourceGlobalNamespace => GetGlobalNamespace();

    public static WhamCompilation Create()
    {
        return Create(ImmutableArray<SourceTree>.Empty);
    }

    public static WhamCompilation Create(
        ImmutableArray<SourceTree> sourceTrees,
        WhamCompilationOptions? options = null)
    {
        return new WhamCompilation(null, sourceTrees, options ?? new WhamCompilationOptions());
    }

    public override SemanticModel GetSemanticModel(SourceTree tree)
    {
        throw new NotImplementedException();
    }

    public override ImmutableArray<Diagnostic> GetDiagnostics(CancellationToken cancellationToken = default)
    {
        var declarationDiagnostics = SourceGlobalNamespace.DeclarationDiagnostics;
        SourceGlobalNamespace.ForceComplete();
        var bindingDiagnostics = BindingDiagnostics;
        var builder = ImmutableArray.CreateBuilder<Diagnostic>(declarationDiagnostics.Count + bindingDiagnostics.Count);
        builder.AddRange(declarationDiagnostics.AsEnumerable());
        builder.AddRange(bindingDiagnostics.AsEnumerable());
        return builder.MoveToImmutable();
    }

    public override WhamCompilation AddSourceTrees(params SourceTree[] trees) =>
        new(Name, SourceTrees.AddRange(trees), Options);

    internal override ICatalogueSymbol CreateMissingGamesystemSymbol(DiagnosticBag diagnostics)
    {
        diagnostics.Add(ErrorCode.ERR_MissingGamesystem, Location.None);
        return new ErrorSymbols.ErrorGamesystemSymbol();
    }

    internal Binder GetBinder(SourceNode node, ISymbol? containingSymbol)
    {
        // TODO node has to have SourceTree property...
        // return GetBinderFactory(node.SourceTree).GetBinder(node);
        var rootNode = node.AncestorsAndSelf().Last();
        return GetBinderFactory(SourceTrees.First(x => x.GetRoot() == rootNode)).GetBinder(node, containingSymbol);
    }

    internal BinderFactory GetBinderFactory(SourceTree tree)
    {
        return new BinderFactory(this, tree);
    }

    internal Binder GlobalNamespaceBinder => GetGlobalNamespaceBinder();

    private Binder GetGlobalNamespaceBinder()
    {
        if (lazyGlobalNamespaceBinder is not null)
        {
            return lazyGlobalNamespaceBinder;
        }
        var binder = new GamesystemNamespaceBinder(new BuckStopsHereBinder(this), SourceGlobalNamespace);
        Interlocked.CompareExchange(ref lazyGlobalNamespaceBinder, binder, null);
        return lazyGlobalNamespaceBinder;
    }

    private SourceGlobalNamespaceSymbol GetGlobalNamespace()
    {
        if (lazyGlobalNamespace is not null)
        {
            return lazyGlobalNamespace;
        }
        var newSymbol = CreateGlobalNamespace();
        Interlocked.CompareExchange(ref lazyGlobalNamespace, newSymbol, null);
        return lazyGlobalNamespace;
    }

    private SourceGlobalNamespaceSymbol CreateGlobalNamespace()
    {
        var nodes = SourceTrees.Select(x => x.GetRoot()).ToImmutableArray();
        return new SourceGlobalNamespaceSymbol(nodes, this);
    }

    private DiagnosticBag BindingDiagnostics
    {
        get
        {
            var bag = DiagnosticBag.GetInstance();
            var oldBag = Interlocked.CompareExchange(ref lazyBindingDiagnostics, bag, null);
            if (oldBag is not null)
            {
                bag.Free();
                return oldBag;
            }
            else
            {
                return bag;
            }
        }
    }

    internal override void AddBindingDiagnostics(DiagnosticBag toAdd)
    {
        BindingDiagnostics.AddRange(toAdd);
    }
}
