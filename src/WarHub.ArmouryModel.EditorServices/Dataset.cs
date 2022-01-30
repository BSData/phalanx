using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
///  Bundle of game system and catalogue objects.
///  In future, I expect this to contain a semantic model as well
///  (symbol layer, with a resolved object graph - links, references, etc.).
/// </summary>
public record Dataset(ImmutableArray<CatalogueBaseNode> Nodes)
{
    public GamesystemNode Gamesystem => Nodes.OfType<GamesystemNode>().Single();
    public ImmutableArray<CatalogueNode> Catalogues => Nodes.OfType<CatalogueNode>().ToImmutableArray();

    public WhamCompilation Compile()
    {
        var sourceTrees = Nodes.Select(SourceTree.CreateForRoot).ToImmutableArray();
        return WhamCompilation.Create(sourceTrees);
    }
}
