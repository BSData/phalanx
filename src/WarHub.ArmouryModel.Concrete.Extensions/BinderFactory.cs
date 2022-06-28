using System.Diagnostics.CodeAnalysis;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.Concrete;

internal class BinderFactory
{
    public BinderFactory(WhamCompilation compilation, SourceTree sourceTree)
    {
        Compilation = compilation;
        SourceTree = sourceTree;
        BuckStopsHereBinder = new(compilation);
    }

    public WhamCompilation Compilation { get; }

    public SourceTree SourceTree { get; }

    public BuckStopsHereBinder BuckStopsHereBinder { get; }

    public Binder GetBinder(SourceNode node, ISymbol? containingSymbol = null)
    {
        var visitor = new BinderFactoryVisitor(this, containingSymbol);
        var binder = visitor.Visit(node);
        return binder;
    }

    private sealed class BinderFactoryVisitor : SourceVisitor<Binder>
    {
        private readonly BinderFactory factory;
        private readonly ISymbol? containingSymbol;

        public BinderFactoryVisitor(BinderFactory factory, ISymbol? containingSymbol)
        {
            this.factory = factory;
            this.containingSymbol = containingSymbol;
        }

        private WhamCompilation Compilation => factory.Compilation;

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

        public override Binder VisitRoster(RosterNode node)
        {
            var next = Compilation.GlobalNamespaceBinder;
            return new RosterBinder(next, GetRosterSymbol(node));
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

        public override Binder VisitCharacteristic(CharacteristicNode node)
        {
            var next = DefaultVisit(node);
            var profileSymbol = GetProfileSymbol(node.FirstAncestorOrSelf<ProfileNode>()!, next)
                ?? throw new InvalidOperationException("Profile symbol required.");
            return new CharacteristicBinder(next, profileSymbol);
        }

        private ProfileSymbol? GetProfileSymbol(ProfileNode node, Binder outerBinder)
        {
            if (containingSymbol is ProfileSymbol profileSymbol && profileSymbol.Declaration == node)
            {
                return profileSymbol;
            }
            var container = GetContainerEntry(outerBinder, node);
            return container?.Resources.OfType<ProfileSymbol>().FirstOrDefault(x => x.Declaration == node);
        }

        // TODO group binder
        // public override Binder VisitSelectionEntryGroup(SelectionEntryGroupNode node)
        // {
        //     var next = DefaultVisit(node);
        //     return new SelectionGroupBinder(next, node);
        // }

        private static IContainerEntrySymbol? GetContainerEntry(Binder binder, SourceNode node)
        {
            if (binder.ContainingContainerSymbol is { } container)
            {
                return container;
            }
            _ = node.FirstAncestorOrSelf<ContainerEntryBaseNode>();
            // TODO not sure what to do here
            return null;
        }

        private CatalogueBaseSymbol GetCatalogueSymbol(CatalogueBaseNode node)
        {
            return Compilation.SourceGlobalNamespace.Catalogues
                .First(x => x.Declaration == node);
        }

        private RosterSymbol GetRosterSymbol(RosterNode node)
        {
            return Compilation.SourceGlobalNamespace.Rosters
                .First(x => x.Declaration == node);
        }
    }
}
