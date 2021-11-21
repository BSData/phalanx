using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Implementation;

public class CatalogueSymbol : CatalogueBaseSymbol
{
    public CatalogueSymbol(Compilation declaringCompilation, CatalogueNode declaration)
        : base(declaringCompilation, declaration)
    {
        Declaration = declaration;
        Imports = CreateLinks().ToImmutableArray();

        IEnumerable<ICatalogueReferenceSymbol> CreateLinks()
        {
            foreach (var item in declaration.CatalogueLinks)
            {
                yield return new CatalogueReferenceSymbol(this, item);
            }
        }
    }

    public override bool IsLibrary => Declaration.IsLibrary;

    public override bool IsGamesystem => false;

    public override ICatalogueSymbol Gamesystem { get; } = null!; // TODO bind

    public override ImmutableArray<ICatalogueReferenceSymbol> Imports { get; }

    internal CatalogueNode Declaration { get; }
}
