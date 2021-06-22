using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public class ReferenceSourceIndex : IReferenceSourceIndex
    {
        public ReferenceSourceIndex(
            ImmutableArray<QueryBaseNode> inQueryScope,
            ImmutableArray<QueryBaseNode> inQueryField,
            ImmutableArray<QueryFilteredBaseNode> inQueryChildId,
            ImmutableArray<SourceNode> inLinkTargetId,
            ImmutableArray<SourceNode> inValueTypeId,
            ImmutableArray<SourceNode> inPublicationId)
        {
            InQueryScope = inQueryScope;
            InQueryField = inQueryField;
            InQueryChildId = inQueryChildId;
            InLinkTargetId = inLinkTargetId;
            InValueTypeId = inValueTypeId;
            InPublicationId = inPublicationId;
        }

        public static ReferenceSourceIndex Empty { get; }
            = new ReferenceSourceIndex(
                inQueryScope: ImmutableArray<QueryBaseNode>.Empty,
                inQueryField: ImmutableArray<QueryBaseNode>.Empty,
                inQueryChildId: ImmutableArray<QueryFilteredBaseNode>.Empty,
                inLinkTargetId: ImmutableArray<SourceNode>.Empty,
                inValueTypeId: ImmutableArray<SourceNode>.Empty,
                inPublicationId: ImmutableArray<SourceNode>.Empty);

        public ImmutableArray<QueryBaseNode> InQueryScope { get; }
        public ImmutableArray<QueryBaseNode> InQueryField { get; }
        public ImmutableArray<QueryFilteredBaseNode> InQueryChildId { get; }
        public ImmutableArray<SourceNode> InLinkTargetId { get; }
        public ImmutableArray<SourceNode> InValueTypeId { get; }
        public ImmutableArray<SourceNode> InPublicationId { get; }
    }
}
