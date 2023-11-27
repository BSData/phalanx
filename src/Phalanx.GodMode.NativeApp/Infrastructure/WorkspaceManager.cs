using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel.Workspaces.BattleScribe;

namespace Phalanx.GodMode.NativeApp.Infrastructure;
internal class WorkspaceManager
{
    public XmlWorkspace? CurrentXmlWorkspace { get; set; }

    public WhamCompilation? CurrentCompilation { get; set; }
}
