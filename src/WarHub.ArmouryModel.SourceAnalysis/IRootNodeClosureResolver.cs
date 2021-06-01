using WarHub.ArmouryModel.Source;

namespace WarHub.ArmouryModel.SourceAnalysis
{
    public interface IRootNodeClosureResolver
    {
        IRootNodeClosure? GetRootClosure(SourceNode node);
    }
}
