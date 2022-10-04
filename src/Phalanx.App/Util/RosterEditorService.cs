using Phalanx.SampleDataset;
using WarHub.ArmouryModel.EditorServices;

namespace Phalanx.App.Util;

public class RosterEditorService
{
    public RosterEditor? ActiveEditor { get; private set; }

    public void LoadSampleRoster()
    {
        var ws = SampleDataResources.CreateXmlWorkspace();
        var rosterState = RosterState.CreateFromNodes(ws.Documents.Select(x => x.GetRootAsync().Result!));
        ActiveEditor = new RosterEditor(rosterState);
    }
}
