namespace WarHub.ArmouryModel.EditorServices;

public interface IDataStorage
{
    IAsyncEnumerable<IGameSystemDataSet> GetDataSets();
}
