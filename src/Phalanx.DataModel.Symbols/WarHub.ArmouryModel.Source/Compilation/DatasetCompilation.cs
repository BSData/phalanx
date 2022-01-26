using Phalanx.DataModel.Symbols;
using Phalanx.DataModel.Symbols.Binding;
using Phalanx.DataModel.Symbols.Implementation;

namespace WarHub.ArmouryModel.Source;

public class DatasetCompilation : Compilation
{
    private SourceGlobalNamespaceSymbol? lazyGlobalNamespace;
    private Binder? lazyGlobalNamespaceBinder;
    private DiagnosticBag? lazyBindingDiagnostics;

    internal DatasetCompilation(string? name, ImmutableArray<SourceTree> sourceTrees, CompilationOptions options)
        : base(name, sourceTrees, options)
    {
    }

    public override IGamesystemNamespaceSymbol GlobalNamespace => SourceGlobalNamespace;

    internal SourceGlobalNamespaceSymbol SourceGlobalNamespace => GetGlobalNamespace();

    public static DatasetCompilation Create()
    {
        return Create(ImmutableArray<SourceTree>.Empty);
    }

    public static DatasetCompilation Create(
        ImmutableArray<SourceTree> sourceTrees,
        DatasetCompilationOptions? options = null)
    {
        return new DatasetCompilation(null, sourceTrees, options ?? new DatasetCompilationOptions());
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

    internal override ICatalogueSymbol CreateMissingGamesystemSymbol(DiagnosticBag diagnostics)
    {
        diagnostics.Add(ErrorCode.ERR_MissingGamesystem, Location.None);
        return new ErrorSymbols.ErrorGamesystemSymbol();
    }

    internal override BinderFactory GetBinderFactory(SourceTree tree)
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
        var nodes = SourceTrees.Select(x => (CatalogueBaseNode)x.GetRoot()).ToImmutableArray();
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
