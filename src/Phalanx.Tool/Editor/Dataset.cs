using WarHub.ArmouryModel.Source;

namespace Phalanx.Tool.Editor;

/// <summary>
///  Bundle of game system and catalogue objects.
///  In future, I expect this to contain a semantic model as well
///  (symbol layer, with a resolved object graph - links, references, etc.).
/// </summary>
public record Dataset(ImmutableArray<CatalogueBaseNode> Nodes)
{
    public GamesystemNode Gamesystem => Nodes.OfType<GamesystemNode>().Single();
    public ImmutableArray<CatalogueNode> Catalogues => Nodes.OfType<CatalogueNode>().ToImmutableArray();
}
