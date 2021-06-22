using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public static class ReferenceResolverCore
    {
        public static IReferenceTargetInfo<TNode> ResolveReference<TNode>(
            ImmutableArray<CatalogueBaseNode> closureRoots,
            string? targetId,
            Func<CatalogueBaseNode, ListNode<TNode>> getList) where TNode : SourceNode, IIdentifiableNode
        {
            if (string.IsNullOrEmpty(targetId))
            {
                return CreateError(new SimpleReferenceErrorInfo("Empty targetId is non-resolvable."));
            }
            foreach (var rootNode in closureRoots)
            {
                if (TryGetTarget(rootNode, out var target))
                {
                    return ReferenceTargetInfo.Create(target);
                }
            }
            return CreateError(new SimpleReferenceErrorInfo($"Failed to resolve targetId '{targetId}'."));

            bool TryGetTarget(CatalogueBaseNode root, [NotNullWhen(true)] out TNode? target)
            {
                var list = getList(root);
                var targets = list.NodeList.Where(x => x.Id == targetId);
                target = targets.FirstOrDefault();
                return target != null;
            }

            static IReferenceTargetInfo<TNode> CreateError(ReferenceErrorInfo error)
            {
                return new ReferenceTargetInfo<TNode>(error);
            }
        }

        public static IReferenceTargetInfo<TNode> ResolveReferenceFlat<TNode>(
            string? targetId,
            IEnumerable<TNode> list) where TNode : SourceNode, IIdentifiableNode
        {
            if (string.IsNullOrEmpty(targetId))
            {
                return CreateError(new SimpleReferenceErrorInfo("Empty targetId is non-resolvable."));
            }
            if (TryGetTarget(out var target))
            {
                return ReferenceTargetInfo.Create(target);
            }
            return CreateError(new SimpleReferenceErrorInfo($"Failed to resolve targetId '{targetId}'."));

            bool TryGetTarget([NotNullWhen(true)] out TNode? target)
            {
                var targets = list.Where(x => x.Id == targetId);
                target = targets.FirstOrDefault();
                return target != null;
            }

            static IReferenceTargetInfo<TNode> CreateError(ReferenceErrorInfo error)
            {
                return new ReferenceTargetInfo<TNode>(error);
            }
        }
    }
}
