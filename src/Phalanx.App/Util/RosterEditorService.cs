namespace Phalanx.App.Util;
using WarHub.ArmouryModel.EditorServices;
using Phalanx.SampleDataset;

public class RosterEditorService
{
    public static RosterEditor? Editor;
    public RosterEditorService()
    {
        Editor = null;
    }

    // TODO, parameterize this with a file path or etc once roster selection/creation is a thing
    public void LoadRoster()
    {
        //get roster from file

        var ws = SampleDataResources.CreateXmlWorkspace();
        var rosterState = RosterState.CreateFromNodes(ws.Documents.Select(x => x.GetRootAsync().Result!));

        Editor = new RosterEditor(rosterState);

    }
}
