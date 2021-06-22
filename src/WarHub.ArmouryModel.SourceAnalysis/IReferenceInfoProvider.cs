using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IReferenceInfoProvider
    {
        IReferenceSourceIndex GetReferences(SourceNode node);
    }
}
