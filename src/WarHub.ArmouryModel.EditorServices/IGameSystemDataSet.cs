namespace WarHub.ArmouryModel.EditorServices;

public interface IGameSystemDataSet
{
    IAsyncEnumerable<IDataFileEntry> GetEntries();
    string Name { get; }
    string ID { get; }
}
