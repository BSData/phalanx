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
            return GetParentBinder(node);
        }

        public override Binder Visit(SourceNode? node)
        {
            return VisitCore(node);
        }

        private Binder VisitCore(SourceNode? node)
        {
            // binding a null node is unexpected, can throw
            return node!.Accept(this)!;
        }

        private Binder GetParentBinder(SourceNode node)
        {
            // no support for detached nodes (e.g. withoug root like roster or catalogue node)
            // can we add such support?
            return node.Parent is { } parent ? VisitCore(parent) : Compilation.GlobalNamespaceBinder;
        }

        public override Binder VisitRoster(RosterNode node)
        {
            var next = GetParentBinder(node);
            return new RosterBinder(next, GetRosterSymbol(node));
        }

        public override Binder VisitCatalogue(CatalogueNode node)
        {
            var next = GetParentBinder(node);
            return new CatalogueBaseBinder(next, GetCatalogueSymbol(node));
        }

        public override Binder VisitGamesystem(GamesystemNode node)
        {
            var next = GetParentBinder(node);
            return new CatalogueBaseBinder(next, GetCatalogueSymbol(node));
        }

        public override Binder VisitCategoryEntry(CategoryEntryNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitEntryLink(EntryLinkNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitForceEntry(ForceEntryNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitInfoGroup(InfoGroupNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitInfoLink(InfoLinkNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitProfile(ProfileNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitRule(RuleNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitSelectionEntry(SelectionEntryNode node)
        {
            return VisitEntryCore(node);
        }

        public override Binder VisitSelectionEntryGroup(SelectionEntryGroupNode node)
        {
            return VisitEntryCore(node);
        }

        private Binder VisitEntryCore(EntryBaseNode node)
        {
            var next = GetParentBinder(node);
            var containingEntry = GetContainingEntry(next, node);
            if (containingEntry is not null)
            {
                next = new EntryBinder(containingEntry, next);
            }
            return next;
        }

        public override Binder VisitCharacteristic(CharacteristicNode node)
        {
            var next = GetParentBinder(node);
            if (GetAncestorSymbol<ProfileSymbol>(node) is { } profile)
            {
                return new CharacteristicBinder(next, profile, profile.Type);
            }
            if (GetAncestorSymbol<RosterProfileSymbol>(node) is { } rosterProfile)
            {
                return new CharacteristicBinder(next, rosterProfile, rosterProfile.Type);
            }
            return next; // throw?
        }

        public override Binder VisitForce(ForceNode node)
        {
            var next = GetParentBinder(node);
            var symbol = GetAncestorSymbol<ForceSymbol>(node);
            return symbol is null ? next : new ForceBinder(next, symbol);
        }

        public override Binder VisitSelection(SelectionNode node)
        {
            var next = GetParentBinder(node);
            var symbol = GetAncestorSymbol<SelectionSymbol>(node);
            return symbol is null ? next : new SelectionBinder(next, symbol);
        }

        private CatalogueBaseSymbol GetCatalogueSymbol(CatalogueBaseNode node) =>
            GetAncestorModule<CatalogueBaseSymbol>(node);

        private RosterSymbol GetRosterSymbol(RosterNode node) =>
            GetAncestorModule<RosterSymbol>(node);

        private static EntrySymbol? GetContainingEntry(Binder outerBinder, SourceNode node)
        {
            foreach (var item in outerBinder.ContainingSymbol!.GetMembers())
            {
                if (item is EntrySymbol entry && ReferenceEquals(entry.Declaration, node))
                {
                    return entry;
                }
            }
            return null;
        }

        private T? GetAncestorSymbol<T>(SourceNode node) where T : Symbol
        {
            return _(containingSymbol);

            static T? _(ISymbol? symbol) => (symbol as T ?? symbol?.ContainingSymbol as T) is { } ancestor
                ? ancestor
                : symbol?.ContainingSymbol is { } parent
                ? _(parent)
                : null;
        }

        private T GetAncestorModule<T>(SourceNode node) where T : Symbol
        {
            if ((containingSymbol as T ?? containingSymbol?.ContainingModule as T) is { } module)
            {
                return module;
            }
            return Compilation.SourceGlobalNamespace.AllRootSymbols
                .OfType<T>()
                .First(x => (x as SourceDeclaredSymbol)?.Declaration == node);
        }
    }
}
