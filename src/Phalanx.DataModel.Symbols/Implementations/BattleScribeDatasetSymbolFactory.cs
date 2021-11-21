using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

internal static class BattleScribeDatasetSymbolFactory
{
    internal static ICatalogueSymbol CreateSymbol(CatalogueBaseNode node, Compilation compilation)
    {
        if (node is CatalogueNode catalogueNode)
        {
            return new CatalogueSymbol(compilation, catalogueNode);
        }
        else if (node is GamesystemNode gamesystemNode)
        {
            return new GamesystemSymbol(compilation, gamesystemNode);
        }
        else
        {
            throw new ArgumentException("Unrecognized root node type.", nameof(node));
        }
    }
}
