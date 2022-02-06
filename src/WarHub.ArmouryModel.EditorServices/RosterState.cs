using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.EditorServices;

/// <summary>
/// Represents a point-in-time roster together with dataset used to create the roster and roster diagnostics.
/// Diagnostics will contain warnings and errors to show to the user.
/// </summary>
public record RosterState(Compilation Compilation)
{
    public GamesystemNode Gamesystem => Compilation.GlobalNamespace.RootCatalogue.GetGamesystemDeclaration()
        ?? throw new InvalidOperationException();

    public ImmutableArray<CatalogueNode> Catalogues =>
        Compilation.GlobalNamespace.Catalogues.Where(x => !x.IsGamesystem)
            .Select(x => x.GetCatalogueDeclaration() ?? throw new InvalidOperationException())
            .ToImmutableArray();

    public RosterNode? Roster => Compilation.GlobalNamespace.Rosters.SingleOrDefault()?.GetDeclaration();

    public RosterNode RosterRequired => Roster ?? throw new InvalidOperationException();

    public static RosterState CreateFromNodes(params SourceNode[] rootNodes)
        => new(WhamCompilation.Create(rootNodes.Select(SourceTree.CreateForRoot).ToImmutableArray()));

    public static RosterState CreateFromNodes(IEnumerable<SourceNode> rootNodes)
        => new(WhamCompilation.Create(rootNodes.Select(SourceTree.CreateForRoot).ToImmutableArray()));

    public RosterState ReplaceRoster(RosterNode node)
    {
        var oldTree = RosterRequired.GetSourceTree(Compilation);
        var newTree = oldTree.WithRoot(node);
        return new(Compilation.ReplaceSourceTree(oldTree, newTree));
    }
}
