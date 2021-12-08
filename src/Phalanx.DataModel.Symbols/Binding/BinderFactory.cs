using Phalanx.DataModel.Symbols.Implementation;
using WarHub.ArmouryModel.Source;

namespace Phalanx.DataModel.Symbols.Binding;

public class BinderFactory
{
    public BinderFactory(DatasetCompilation compilation, SourceTree sourceTree)
    {
        Compilation = compilation;
        SourceTree = sourceTree;
    }

    public DatasetCompilation Compilation { get; }

    public SourceTree SourceTree { get; }

    public Binder GetBinder(SourceNode node, ISymbol? containingSymbol = null)
    {
        // TODO if containing symbol is provided, it can make finding appropriate symbols for binders much faster
        var visitor = new BinderFactoryVisitor(this);
        var binder = visitor.Visit(node);
        return binder;
    }

    private sealed class BinderFactoryVisitor : SourceVisitor<Binder>
    {
        private readonly BinderFactory factory;

        public BinderFactoryVisitor(BinderFactory factory)
        {
            this.factory = factory;
        }

        private DatasetCompilation Compilation => factory.Compilation;

        public override Binder DefaultVisit(SourceNode node)
        {
            return VisitCore(node.Parent);
        }

        public override Binder Visit(SourceNode? node)
        {
            return VisitCore(node);
        }

        private Binder VisitCore(SourceNode? node)
        {
            // binding a null node is unexpected, can throw
            return node!.Accept(this);
        }

        public override Binder VisitCatalogue(CatalogueNode node)
        {
            var next = Compilation.GlobalNamespaceBinder;
            return new CatalogueBaseBinder(next, GetCatalogueSymbol(node));
        }

        public override Binder VisitGamesystem(GamesystemNode node)
        {
            var next = Compilation.GlobalNamespaceBinder;
            return new CatalogueBaseBinder(next, GetCatalogueSymbol(node));
        }

        private CatalogueBaseSymbol GetCatalogueSymbol(CatalogueBaseNode node)
        {
            return Compilation.GlobalNamespace.Catalogues.First(x => x.Declaration == node);
        }
    }
}
