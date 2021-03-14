using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace Phalanx.Tool.Editor
{
    public record Dataset(GamesystemNode Gamesystem, ImmutableArray<CatalogueNode> Catalogues);
}
