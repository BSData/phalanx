using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IReferenceableInfo
    {
        SourceNode TargetNode { get; }

        IReferenceSourceIndex ReferenceIndex { get; }
    }
}
