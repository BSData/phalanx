using System;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public static class RootNodeClosureExtensions
    {
        public static IReferenceTargetInfo<TNode> ResolveReference<TNode>(
            this IRootNodeClosure closure,
            string? targetId,
            Func<CatalogueBaseNode, ListNode<TNode>> getList)
            where TNode : SourceNode, IIdentifiableNode
        {
            return ReferenceResolverCore.ResolveReference(closure.ReferencesAndRoot, targetId, getList);
        }

        public static IReferenceTargetInfo<SourceNode> ResolveLink(
            this IRootNodeClosureResolver closureResolver,
            SourceNode node)
            => closureResolver.GetRootClosure(node)?.ResolveLink(node)
            ?? CreateNoClosureError<SourceNode>();

        public static IReferenceTargetInfo<SourceNode> ResolveLink(
            this IRootNodeClosure closure,
            SourceNode node)
        {
            return node switch
            {
                CatalogueLinkNode { Type: CatalogueLinkKind.Catalogue } link =>
                    closure.ResolveCatalogue(link),

                CategoryLinkNode link =>
                    closure.ResolveReference(link.TargetId, x => x.CategoryEntries),

                EntryLinkNode { Type: EntryLinkKind.SelectionEntry } link =>
                    closure.ResolveReference(link.TargetId, x => x.SharedSelectionEntries),

                EntryLinkNode { Type: EntryLinkKind.SelectionEntryGroup } link =>
                    closure.ResolveReference(link.TargetId, x => x.SharedSelectionEntryGroups),

                InfoLinkNode { Type: InfoLinkKind.InfoGroup } link =>
                    closure.ResolveReference(link.TargetId, x => x.SharedInfoGroups),

                InfoLinkNode { Type: InfoLinkKind.Profile } link =>
                    closure.ResolveReference(link.TargetId, x => x.SharedProfiles),

                InfoLinkNode { Type: InfoLinkKind.Rule } link =>
                    closure.ResolveReference(link.TargetId, x => x.SharedRules),

                null => CreateError("Can't resolve null link."),
                _ => CreateError($"Can't resolve unknown link type ({node.GetType()}).")
            };
        }

        public static IReferenceTargetInfo<SourceNode> ResolveValueType(
            this IRootNodeClosureResolver closureResolver,
            SourceNode node)
            => closureResolver.GetRootClosure(node)?.ResolveValueType(node)
            ?? CreateNoClosureError<SourceNode>();

        public static IReferenceTargetInfo<SourceNode> ResolveValueType(
            this IRootNodeClosure closure,
            SourceNode node)
        {
            return node switch
            {
                CostNode value => closure.ResolveCostType(value),
                CostLimitNode value => closure.ResolveCostType(value),
                CharacteristicNode value => closure.ResolveCharacteristicType(value),
                ProfileNode value => closure.ResolveProfileType(value),
                null => CreateError("Can't resolve null link."),
                _ => CreateError($"Can't resolve unknown link type ({node.GetType()}).")
            };
        }

        public static IReferenceTargetInfo<PublicationNode> ResolvePublication(
            this IRootNodeClosureResolver closureResolver,
            IPublicationReferencingNode reference)
            => closureResolver.GetRootClosure((SourceNode)reference)?.ResolvePublication(reference)
            ?? CreateNoClosureError<PublicationNode>();

        public static IReferenceTargetInfo<PublicationNode> ResolvePublication(
            this IRootNodeClosure closure,
            IPublicationReferencingNode reference)
        {
            if (reference is null)
            {
                return CreateError<PublicationNode>("Can't resolve null node.");
            }
            return closure.ResolveReference(reference.PublicationId, x => x.Publications);
        }

        public static IReferenceTargetInfo<CatalogueNode> ResolveCatalogue(
            this IRootNodeClosureResolver closureResolver,
            CatalogueLinkNode link)
            => closureResolver.GetRootClosure(link)?.ResolveCatalogue(link)
            ?? CreateNoClosureError<CatalogueNode>();

        public static IReferenceTargetInfo<CatalogueNode> ResolveCatalogue(
            this IRootNodeClosure closure,
            CatalogueLinkNode link)
        {
            if (link is null)
            {
                return CreateError<CatalogueNode>("Can't resolve null link.");
            }
            return ReferenceResolverCore.ResolveReferenceFlat(
                link.TargetId,
                closure.References.OfType<CatalogueNode>());
        }

        public static IReferenceTargetInfo<CostTypeNode> ResolveCostType(
            this IRootNodeClosureResolver closureResolver,
            CostBaseNode cost)
            => closureResolver.GetRootClosure(cost)?.ResolveCostType(cost)
            ?? CreateNoClosureError<CostTypeNode>();

        public static IReferenceTargetInfo<CostTypeNode> ResolveCostType(
            this IRootNodeClosure closure,
            CostBaseNode cost)
        {
            if (cost is null)
            {
                return CreateError<CostTypeNode>("Can't resolve null node.");
            }
            return closure.ResolveReference(cost.TypeId, x => x.CostTypes);
        }

        public static IReferenceTargetInfo<CharacteristicTypeNode> ResolveCharacteristicType(
            this IRootNodeClosureResolver closureResolver,
            CharacteristicNode characteristic)
            => closureResolver.GetRootClosure(characteristic)?.ResolveCharacteristicType(characteristic)
            ?? CreateNoClosureError<CharacteristicTypeNode>();

        public static IReferenceTargetInfo<CharacteristicTypeNode> ResolveCharacteristicType(
            this IRootNodeClosure closure,
            CharacteristicNode characteristic)
        {
            if (characteristic is null)
            {
                return CreateError("Can't resolve null link.");
            }
            var parentProfile = characteristic.FirstAncestorOrSelf<ProfileNode>();
            if (parentProfile is null)
            {
                return CreateError("Can't resolve without a parent Profile.");
            }
            var profileTypeResult = closure.ResolveProfileType(parentProfile);
            if (!profileTypeResult.IsResolved || profileTypeResult.TargetNode is null)
            {
                return CreateError("Failed to resolve ProfileType for the parent Profile.");
            }
            return ReferenceResolverCore.ResolveReferenceFlat(
                characteristic.TypeId,
                profileTypeResult.TargetNode.CharacteristicTypes);

            static IReferenceTargetInfo<CharacteristicTypeNode> CreateError(string message)
                => CreateError<CharacteristicTypeNode>(message);
        }

        public static IReferenceTargetInfo<ProfileTypeNode> ResolveProfileType(
            this IRootNodeClosureResolver closureResolver,
            ProfileNode profile)
            => closureResolver.GetRootClosure(profile)?.ResolveProfileType(profile)
            ?? CreateNoClosureError<ProfileTypeNode>();

        public static IReferenceTargetInfo<ProfileTypeNode> ResolveProfileType(
            this IRootNodeClosure closure,
            ProfileNode profile)
        {
            if (profile is null)
            {
                return CreateError<ProfileTypeNode>("Can't resolve null link.");
            }
            return closure.ResolveReference(profile.TypeId, x => x.ProfileTypes);
        }

        private static IReferenceTargetInfo<SourceNode> CreateError(string message)
            => ReferenceTargetInfo.Error<SourceNode>(new SimpleReferenceErrorInfo(message));

        private static IReferenceTargetInfo<T> CreateError<T>(string message) where T : SourceNode
            => ReferenceTargetInfo.Error<T>(new SimpleReferenceErrorInfo(message));

        private static IReferenceTargetInfo<T> CreateNoClosureError<T>() where T : SourceNode
            => ReferenceTargetInfo.Error<T>(
                new SimpleReferenceErrorInfo(
                    "Given node doesn't belong to any closure of current context."));
    }
}
