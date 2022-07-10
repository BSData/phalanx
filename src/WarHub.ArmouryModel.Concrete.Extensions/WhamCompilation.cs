using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

public class WhamCompilation : Compilation
{
    private SourceGlobalNamespaceSymbol? lazyGlobalNamespace;
    private Binder? lazyGlobalNamespaceBinder;
    private DiagnosticBag? lazyDeclarationDiagnostics;
    private ICategoryEntrySymbol? lazyNoCategoryEntrySymbol;

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
        var namespaceDiagnostics = SourceGlobalNamespace.DeclarationDiagnostics;
        SourceGlobalNamespace.ForceComplete(cancellationToken);
        var declarationDiagnostics = DeclarationDiagnostics;
        var builder = ImmutableArray.CreateBuilder<Diagnostic>(namespaceDiagnostics.Count + declarationDiagnostics.Count);
        builder.AddRange(namespaceDiagnostics.AsEnumerable());
        builder.AddRange(declarationDiagnostics.AsEnumerable());
        return builder.MoveToImmutable();
    }

    public override WhamCompilation AddSourceTrees(params SourceTree[] trees) =>
        Update(SourceTrees.AddRange(trees));

    public override WhamCompilation ReplaceSourceTree(SourceTree oldTree, SourceTree? newTree) =>
        Update(newTree is null ? SourceTrees.Remove(oldTree) : SourceTrees.Replace(oldTree, newTree));

    private WhamCompilation Update(ImmutableArray<SourceTree> trees) =>
        new(Name, trees, Options);

    internal override ICatalogueSymbol CreateMissingGamesystemSymbol(DiagnosticBag diagnostics)
    {
        diagnostics.Add(ErrorCode.ERR_MissingGamesystem, Location.None);
        return new ErrorSymbols.ErrorGamesystemSymbol();
    }

    internal Binder GetBinder(SourceNode node, ISymbol? containingSymbol)
    {
        var rootNode = node.AncestorsAndSelf().Last();
        return GetBinderFactory(rootNode.GetSourceTree(this)).GetBinder(node, containingSymbol);
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

    internal DiagnosticBag DeclarationDiagnostics
    {
        get
        {
            if (lazyDeclarationDiagnostics is null)
            {
                Interlocked.CompareExchange(ref lazyDeclarationDiagnostics, DiagnosticBag.GetInstance(), null);
            }
            return lazyDeclarationDiagnostics;
        }
    }

    public override ICategoryEntrySymbol NoCategoryEntrySymbol
    {
        get
        {
            if (lazyNoCategoryEntrySymbol is null)
            {
                Interlocked.CompareExchange(ref lazyNoCategoryEntrySymbol, CreateNoCategoryEntrySymbol(), null);
            }
            return lazyNoCategoryEntrySymbol;
        }
    }

    private CategoryEntrySymbol CreateNoCategoryEntrySymbol()
    {
        var node = NodeFactory.CategoryEntry("Uncategorised", "(No Category)");
        return new CategoryEntrySymbol(SourceGlobalNamespace, node, DeclarationDiagnostics);
    }
}
