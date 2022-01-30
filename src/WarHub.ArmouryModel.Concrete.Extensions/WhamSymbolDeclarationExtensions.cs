using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel;

public static class WhamSymbolDeclarationExtensions
{
    public static RosterNode? GetDeclaration(this IRosterSymbol symbol) =>
        GetDeclarationCore<RosterSymbol, RosterNode>(symbol);

    public static GamesystemNode? GetGamesystemDeclaration(this ICatalogueSymbol symbol) =>
        GetDeclarationCore<GamesystemSymbol, GamesystemNode>(symbol);

    public static CatalogueNode? GetCatalogueDeclaration(this ICatalogueSymbol symbol) =>
        GetDeclarationCore<CatalogueSymbol, CatalogueNode>(symbol);

    private static TNode? GetDeclarationCore<TSymbol, TNode>(ISymbol symbol)
        where TSymbol : ISymbol, INodeDeclaredSymbol<TNode>
        where TNode : SourceNode
    {
        return symbol is TSymbol { Declaration: var decl } ? decl : null;
    }

}
