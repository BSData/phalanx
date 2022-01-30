using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

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

    internal abstract ICatalogueSymbol CreateMissingGamesystemSymbol(DiagnosticBag diagnostics);

    internal abstract void AddBindingDiagnostics(DiagnosticBag toAdd);
}
