using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IReferenceTargetInfo
    {
        bool IsResolved { get; }
        SourceNode? TargetNode { get; }
        ReferenceErrorInfo? ErrorInfo { get; }
    }
}
