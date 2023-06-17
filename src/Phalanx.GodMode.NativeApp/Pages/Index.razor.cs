using CommunityToolkit.Maui.Storage;
using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.ProjectModel;

namespace Phalanx.GodMode.NativeApp.Pages;

public partial class Index
{
    string? selectedFolder;
    private XmlWorkspace? xmlws;
    private WhamCompilation? compilation;

    public async Task OpenFolder()
    {
        var result =
            string.IsNullOrEmpty(selectedFolder)
            ? await FolderPicker.Default.PickAsync(default)
            : await FolderPicker.Default.PickAsync(selectedFolder, default);
        if (result.IsSuccessful && result.Folder.Path != selectedFolder)
        {
            selectedFolder = result.Folder.Path;
            StateHasChanged();
            await Task.Run(async () =>
            {
                xmlws = XmlWorkspace.CreateFromDirectory(selectedFolder);
                var roots = await LoadFiles(xmlws.GetDocuments(SourceKind.Catalogue, SourceKind.Gamesystem)).ToListAsync();
                var sourceTrees = roots.Select(x => x.tree).ToImmutableArray();
                compilation = WhamCompilation.Create(sourceTrees);
            });
            StateHasChanged();
        }
    }

    static async IAsyncEnumerable<(SourceTree tree, IDatafileInfo fileinfo)> LoadFiles(IEnumerable<XmlDocument> files)
    {
        foreach (var file in files)
        {
            var node = await file.GetRootAsync();
            var tree = SourceTree.CreateForRoot(node!);
            yield return (tree, file.DatafileInfo);
        }
    }
}
