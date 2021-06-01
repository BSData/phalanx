using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IRootNodeClosure
    {
        ImmutableArray<Diagnostic> Diagnostics { get; }
        ImmutableArray<CatalogueBaseNode> References { get; }
        ImmutableArray<CatalogueBaseNode> ReferencesAndRoot { get; }
        CatalogueBaseNode Root { get; }
    }
}
