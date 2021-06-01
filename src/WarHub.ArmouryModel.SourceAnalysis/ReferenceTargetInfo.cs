using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public static class ReferenceTargetInfo
    {
        public static IReferenceTargetInfo<T> Create<T>(T targetNode) where T : SourceNode
            => new ReferenceTargetInfo<T>(targetNode);

        public static IReferenceTargetInfo<T> Error<T>(ReferenceErrorInfo errorInfo) where T : SourceNode
            => new ReferenceTargetInfo<T>(errorInfo);
    }
}
