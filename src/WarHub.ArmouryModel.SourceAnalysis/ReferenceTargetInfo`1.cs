using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public class ReferenceTargetInfo<TNode> : IReferenceTargetInfo<TNode> where TNode : SourceNode
    {
        private ReferenceTargetInfo(bool isResolved, TNode? targetNode, ReferenceErrorInfo? error)
        {
            IsResolved = isResolved;
            TargetNode = targetNode;
            ErrorInfo = error;
        }

        public ReferenceTargetInfo(TNode targetNode) : this(true, targetNode, null) { }

        public ReferenceTargetInfo(ReferenceErrorInfo error) : this(false, null, error) { }

        public bool IsResolved { get; }

        public TNode? TargetNode { get; }

        public ReferenceErrorInfo? ErrorInfo { get; }
    }
}
