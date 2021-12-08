using Phalanx.DataModel.Symbols;
using Phalanx.DataModel.Symbols.Binding;
using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel;

public class DatasetCompilation : Compilation
{
    private SourceGlobalNamespaceSymbol? lazyGlobalNamespace;

    internal DatasetCompilation(string? name, ImmutableArray<SourceTree> sourceTrees, CompilationOptions options)
        : base(name, sourceTrees, options)
    {
    }

    public override IGamesystemNamespaceSymbol GlobalNamespace => GetGlobalNamespace();

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

    internal override ICatalogueSymbol CreateMissingGamesystemSymbol()
    {
        throw new NotImplementedException();
    }

    internal override BinderFactory GetBinderFactory(SourceTree tree)
    {
        throw new NotImplementedException();
    }

    internal SourceGlobalNamespaceSymbol GetGlobalNamespace()
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
}
