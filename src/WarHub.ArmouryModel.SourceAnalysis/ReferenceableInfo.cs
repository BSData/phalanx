using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    internal class ReferenceableInfo : IReferenceableInfo
    {
        public ReferenceableInfo(SourceNode targetNode, IReferenceSourceIndex referenceIndex)
        {
            TargetNode = targetNode;
            ReferenceIndex = referenceIndex;
        }

        public SourceNode TargetNode { get; }

        public IReferenceSourceIndex ReferenceIndex { get; }
    }
}
