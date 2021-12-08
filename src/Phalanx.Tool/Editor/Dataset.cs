using Phalanx.DataModel;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.SourceAnalysis;

namespace Phalanx.Tool.Editor;

/// <summary>
///  Bundle of game system and catalogue objects.
///  In future, I expect this to contain a semantic model as well
///  (symbol layer, with a resolved object graph - links, references, etc.).
/// </summary>
public record Dataset(GamesystemContext Context)
{
    public GamesystemNode Gamesystem => Context.Gamesystem!;
    public ImmutableArray<CatalogueNode> Catalogues => Context.Catalogues;
    public IEnumerable<CatalogueBaseNode> Nodes => Context.Roots;

    public DatasetCompilation Compile()
    {
        var sourceTrees = Context.Roots.Select(SourceTree.CreateForRoot).ToImmutableArray();
        return DatasetCompilation.Create(sourceTrees);
    }
}
