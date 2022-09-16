namespace Phalanx.App.Util;
using WarHub.ArmouryModel.EditorServices;

public class RosterEditorService {

    public RosterEditor? Editor;
    public RosterEditorService(){
        Editor = null;
    }

    public void LoadRoster(RosterState rosterState){
        Editor = new RosterEditor(rosterState);

    }
}