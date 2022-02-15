using WarHub.ArmouryModel.ProjectModel;

namespace WarHub.ArmouryModel.EditorServices;

public interface IDataFileEntry
{
    string Name { get; }
    string ID { get; }
    string FileName { get; }
    int Revision { get; }
    IDatafileInfo FileInfo { get; }
}
