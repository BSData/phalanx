using Phalanx.DataModel.Symbols;
using Phalanx.DataModel.Symbols.Binding;

namespace WarHub.ArmouryModel.Source;

public abstract class Compilation
{
    internal Compilation(
        string? name,
        ImmutableArray<SourceTree> sourceTrees,
        CompilationOptions options)
    {
        Name = name;
        SourceTrees = sourceTrees;
        Options = options;
    }

    public string? Name { get; }

    public ImmutableArray<SourceTree> SourceTrees { get; }

    public CompilationOptions Options { get; }

    public abstract IGamesystemNamespaceSymbol GlobalNamespace { get; }

    public abstract SemanticModel GetSemanticModel(SourceTree tree);

    public abstract ImmutableArray<Diagnostic> GetDiagnostics(CancellationToken cancellationToken = default);

    internal Binder GetBinder(SourceNode node, ISymbol? containingSymbol)
    {
        // TODO node has to have SourceTree property...
        // return GetBinderFactory(node.SourceTree).GetBinder(node);
        var rootNode = node.AncestorsAndSelf().Last();
        return GetBinderFactory(SourceTrees.First(x => x.GetRoot() == rootNode)).GetBinder(node, containingSymbol);
    }

    internal abstract ICatalogueSymbol CreateMissingGamesystemSymbol(DiagnosticBag diagnostics);

    internal abstract BinderFactory GetBinderFactory(SourceTree tree);

    internal abstract void AddBindingDiagnostics(DiagnosticBag toAdd);
}
