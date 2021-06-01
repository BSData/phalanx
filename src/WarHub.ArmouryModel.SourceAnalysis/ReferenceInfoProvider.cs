using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    internal class ReferenceInfoProvider : IReferenceInfoProvider
    {
        private ImmutableDictionary<SourceNode, IReferenceSourceIndex>? indexes;

        public ReferenceInfoProvider(GamesystemContext context)
        {
            Context = context;
        }

        private ImmutableDictionary<SourceNode, IReferenceSourceIndex> Indexes
            => indexes ??= BuildIndexes();

        public GamesystemContext Context { get; }

        private ImmutableDictionary<SourceNode, IReferenceSourceIndex> BuildIndexes()
        {
            var visitor = new RefSourceVisitor(Context);
            foreach (var root in Context.Roots)
            {
                visitor.Visit(root);
            }
            return visitor.CreateReferenceableInfos()
                .ToImmutableDictionary(x => x.TargetNode, x => x.ReferenceIndex);
        }

        public IReferenceSourceIndex GetReferences(SourceNode node)
        {
            if (Indexes.TryGetValue(node, out var refInfo))
            {
                return refInfo;
            }
            return ReferenceSourceIndex.Empty;
        }

        private class RefCounter
        {
            public List<QueryBaseNode> InQueryScope { get; } = new List<QueryBaseNode>();
            public List<QueryBaseNode> InQueryField { get; } = new List<QueryBaseNode>();
            public List<QueryFilteredBaseNode> InQueryChildId { get; } = new List<QueryFilteredBaseNode>();
            public List<SourceNode> InLinkTargetId { get; } = new List<SourceNode>();
            public List<SourceNode> InValueTypeId { get; } = new List<SourceNode>();
            public List<SourceNode> InPublicationId { get; } = new List<SourceNode>();
        }

        private class RefSourceVisitor : SourceWalker
        {
            private readonly IRootNodeClosureResolver closureResolver;

            private static Func<RefCounter, List<QueryBaseNode>> InQueryScope { get; } = x => x.InQueryScope;

            private static Func<RefCounter, List<QueryBaseNode>> InQueryField { get; } = x => x.InQueryField;

            private static Func<RefCounter, List<QueryFilteredBaseNode>> InQueryChildId { get; } = x => x.InQueryChildId;

            private static Func<RefCounter, List<SourceNode>> InLinkTargetId { get; } = x => x.InLinkTargetId;

            private static Func<RefCounter, List<SourceNode>> InValueTypeId { get; } = x => x.InValueTypeId;

            private static Func<RefCounter, List<SourceNode>> InPublicationId { get; } = x => x.InPublicationId;

            private IRootNodeClosure? closure;

            [NotNull]
            private IRootNodeClosure? Closure
            {
                get => closure ?? throw new InvalidOperationException("Can't process without closure.");
                set => closure = value;
            }

            private Dictionary<SourceNode, RefCounter> RefLists { get; }
                = new Dictionary<SourceNode, RefCounter>();

            private Dictionary<SourceNode, List<IReferenceTargetInfo>> ErrorLists { get; }
                = new Dictionary<SourceNode, List<IReferenceTargetInfo>>();

            public RefSourceVisitor(IRootNodeClosureResolver closureResolver)
            {
                this.closureResolver = closureResolver;
            }

            public IEnumerable<IReferenceableInfo> CreateReferenceableInfos()
            {
                return RefLists.Select(pair => new ReferenceableInfo(pair.Key, CreateRefIndex(pair.Value)));

                static IReferenceSourceIndex CreateRefIndex(RefCounter value)
                {
                    return new ReferenceSourceIndex(
                        inQueryScope: value.InQueryScope.ToImmutableArray(),
                        inQueryField: value.InQueryField.ToImmutableArray(),
                        inQueryChildId: value.InQueryChildId.ToImmutableArray(),
                        inLinkTargetId: value.InLinkTargetId.ToImmutableArray(),
                        inValueTypeId: value.InValueTypeId.ToImmutableArray(),
                        inPublicationId: value.InPublicationId.ToImmutableArray());
                }
            }

            public override void VisitCatalogue(CatalogueNode node)
            {
                Closure = closureResolver.GetRootClosure(node);
                base.VisitCatalogue(node);
            }

            public override void VisitGamesystem(GamesystemNode node)
            {
                Closure = closureResolver.GetRootClosure(node);
                base.VisitGamesystem(node);
            }

            private void Process<TRefNode>(TRefNode refSource, IReferenceTargetInfo targetInfo, Func<RefCounter, List<TRefNode>> getList)
                where TRefNode : SourceNode
            {
                if (targetInfo.TargetNode is null)
                {
                    if (!ErrorLists.TryGetValue(refSource, out var errorList))
                    {
                        ErrorLists[refSource] = errorList = new List<IReferenceTargetInfo>();
                    }
                    errorList.Add(targetInfo);
                    return;
                }
                var refTarget = targetInfo.TargetNode;
                if (!RefLists.TryGetValue(refTarget, out var counter))
                {
                    RefLists[refTarget] = counter = new RefCounter();
                }
                var list = getList(counter);
                list.Add(refSource);
            }

            public override void DefaultVisit(SourceNode node)
            {
                base.DefaultVisit(node);
                if (node is IPublicationReferencingNode pubRef)
                {
                    Process(node, Closure.ResolvePublication(pubRef), InPublicationId);
                }
            }

            public override void VisitCondition(ConditionNode node)
            {
                base.VisitCondition(node);
                VisitQueryFiltered(node);
            }

            public override void VisitConstraint(ConstraintNode node)
            {
                base.VisitConstraint(node);
                VisitQuery(node);
            }

            public override void VisitRepeat(RepeatNode node)
            {
                base.VisitRepeat(node);
                VisitQueryFiltered(node);
            }

            private void VisitQueryFiltered(QueryFilteredBaseNode node)
            {
                // TODO

                // childId accepts:
                // - special: any
                // - SelectionTypes: unit, model, upgrade
                // - force entry
                // - category entry
                // - catalogue
                // - any item from selection entry tree (entries, links and groups)
                if (!IsWellKnownChildId(node.ChildId))
                {
                    var targetInfo = ProbableTargets(node.ChildId).FirstOrDefault(x => x.IsResolved)
                        ?? ReferenceTargetInfo.Error<SourceNode>(new SimpleReferenceErrorInfo("ChildId not resolved."));
                    Process(node, targetInfo, InQueryChildId);

                    // resolve, in order: category, force, catalogue, entryLink, entryBase
                    IEnumerable<IReferenceTargetInfo> ProbableTargets(string? id)
                    {
                        yield return Closure.ResolveReference(id, x => x.CategoryEntries);
                        yield return Closure.ResolveReference(id, x => x.ForceEntries); // TODO recursive
                        yield return ReferenceResolverCore.ResolveReferenceFlat(id, Closure.ReferencesAndRoot);
                        yield return Closure.ResolveReference(id, x => x.EntryLinks); // TODO whole tree
                    }
                }
                VisitQuery(node);

                static bool IsWellKnownChildId(string? id) => id switch
                {
                    "any" => true,
                    "unit" => true,
                    "model" => true,
                    "upgrade" => true,
                    _ => false
                };
            }

            private void VisitQuery(QueryBaseNode node)
            {
                // scope accepts:
                // - special: self, parent, ancestor, primary-category, force, roster, primary-catalogue
                // - force entry
                // - category entry
                // - current entry tree entries (ancestors)
                // field accepts:
                // - special: forces, selections
                // - cost type
                if (!IsWellKnownScope(node.Scope))
                {
                    // resolve in order: force, category, entry tree
                    var targetInfo = ProbableTargets(node.Scope).FirstOrDefault(x => x.IsResolved)
                        ?? ReferenceTargetInfo.Error<SourceNode>(new SimpleReferenceErrorInfo("Scope not resolved."));
                    Process(node, targetInfo, InQueryScope);
                    IEnumerable<IReferenceTargetInfo> ProbableTargets(string? id)
                    {
                        yield return Closure.ResolveReference(id, x => x.CategoryEntries);
                        yield return Closure.ResolveReference(id, x => x.ForceEntries); // TODO recursive
                        yield return Closure.ResolveReference(id, x => x.EntryLinks); // TODO whole tree
                    }
                }
                if (!IsWellKnownField(node.Field))
                {
                    // resolve, in order: cost type
                    Process(node, Closure.ResolveReference(node.Field, x => x.CostTypes), InQueryField);
                }

                static bool IsWellKnownScope(string? scope) => scope switch
                {
                    "self" => true,
                    "parent" => true,
                    "ancestor" => true,
                    "primary-category" => true,
                    "force" => true,
                    "roster" => true,
                    "primary-catalogue" => true,
                    _ => false
                };
                static bool IsWellKnownField(string? field) => field switch
                {
                    "forces" => true,
                    "selections" => true,
                    _ => false
                };
            }

            public override void VisitCatalogueLink(CatalogueLinkNode node)
            {
                base.VisitCatalogueLink(node);
                Process(node, Closure.ResolveLink(node), InLinkTargetId);
            }

            public override void VisitCategoryLink(CategoryLinkNode node)
            {
                base.VisitCategoryLink(node);
                Process(node, Closure.ResolveLink(node), InLinkTargetId);
            }

            public override void VisitCharacteristic(CharacteristicNode node)
            {
                base.VisitCharacteristic(node);
                Process(node, Closure.ResolveCharacteristicType(node), InValueTypeId);
            }

            public override void VisitCost(CostNode node)
            {
                base.VisitCost(node);
                Process(node, Closure.ResolveCostType(node), InValueTypeId);
            }

            public override void VisitCostLimit(CostLimitNode node)
            {
                base.VisitCostLimit(node);
                Process(node, Closure.ResolveCostType(node), InValueTypeId);
            }

            public override void VisitEntryLink(EntryLinkNode node)
            {
                base.VisitEntryLink(node);
                Process(node, Closure.ResolveLink(node), InLinkTargetId);
            }

            public override void VisitInfoLink(InfoLinkNode node)
            {
                base.VisitInfoLink(node);
                Process(node, Closure.ResolveLink(node), InLinkTargetId);
            }

            public override void VisitProfile(ProfileNode node)
            {
                base.VisitProfile(node);
                Process(node, Closure.ResolveProfileType(node), InValueTypeId);
            }
        }
    }
}
