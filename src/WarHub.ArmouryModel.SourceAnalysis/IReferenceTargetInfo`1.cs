using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    /// <summary>
    /// Contains information on reference resolution:
    /// if it was found, if there were other candidates, etc.
    /// </summary>
    public interface IReferenceTargetInfo<out TNode> : IReferenceTargetInfo where TNode : SourceNode
    {
        SourceNode? IReferenceTargetInfo.TargetNode => TargetNode;

        new TNode? TargetNode { get; }
    }
}
