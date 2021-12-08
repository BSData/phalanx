using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class SourceGlobalNamespaceSymbol : Symbol, IGamesystemNamespaceSymbol
{
    public SourceGlobalNamespaceSymbol(
        ImmutableArray<CatalogueBaseNode> catalogues,
        Compilation declaringCompilation)
    {
        DeclaringCompilation = declaringCompilation;
        Catalogues = catalogues.Select(CreateSymbol).ToImmutableArray();
        var rootCandidates = Catalogues.Where(x => x.IsGamesystem).ToImmutableArray();
        RootCatalogue = rootCandidates.FirstOrDefault()
            ?? DeclaringCompilation.CreateMissingGamesystemSymbol();
        if (rootCandidates.Length != 1)
        {
            // TODO add diagnostic
        }

        CatalogueBaseSymbol CreateSymbol(CatalogueBaseNode node)
        {
            if (node is CatalogueNode catalogueNode)
            {
                return new CatalogueSymbol(this, catalogueNode);
            }
            else if (node is GamesystemNode gamesystemNode)
            {
                return new GamesystemSymbol(this, gamesystemNode);
            }
            else
            {
                // TODO consider adding diagnostic and returning an ErrorSymbol?
                throw new ArgumentException("Unrecognized root node type.", nameof(node));
            }
        }
    }

    public override SymbolKind Kind => SymbolKind.Namespace;

    public override string? Id => RootCatalogue.Id;

    public override string Name => RootCatalogue.Name;

    public override string? Comment => null;

    public override ISymbol? ContainingSymbol => null;

    public ICatalogueSymbol RootCatalogue { get; }

    public ImmutableArray<CatalogueBaseSymbol> Catalogues { get; }

    ImmutableArray<ICatalogueSymbol> IGamesystemNamespaceSymbol.Catalogues =>
        Catalogues.CastArray<ICatalogueSymbol>();

    public override IGamesystemNamespaceSymbol? ContainingNamespace => null;

    public override ICatalogueSymbol? ContainingCatalogue => null;

    internal override Compilation DeclaringCompilation { get; }
}
