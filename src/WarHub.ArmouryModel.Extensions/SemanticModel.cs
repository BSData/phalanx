namespace WarHub.ArmouryModel;

public abstract class SemanticModel
{
    public abstract Compilation Compilation { get; }

    public abstract SourceTree SourceTree { get; }
}
