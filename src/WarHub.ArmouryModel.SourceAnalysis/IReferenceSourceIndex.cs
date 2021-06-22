using System.Collections.Immutable;
using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IReferenceSourceIndex
    {
        ImmutableArray<QueryBaseNode> InQueryScope { get; }
        ImmutableArray<QueryBaseNode> InQueryField { get; }
        ImmutableArray<QueryFilteredBaseNode> InQueryChildId { get; }
        ImmutableArray<SourceNode> InLinkTargetId { get; }
        ImmutableArray<SourceNode> InValueTypeId { get; }
        ImmutableArray<SourceNode> InPublicationId { get; }
    }
}
