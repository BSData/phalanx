using CommunityToolkit.Maui.Storage;
using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.ArmouryModel.Source;
using WarHub.ArmouryModel.ProjectModel;
using System;

namespace Phalanx.GodMode.NativeApp.Pages;

public partial class Index
{
    string? selectedFolder;
    private XmlWorkspace? xmlws;
    private WhamCompilation? compilation;
    private string? loadingStatus;

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
            IProgress<string?> progress = new Progress<string?>(x =>
            {
                InvokeAsync(() =>
                {
                    loadingStatus = x;
                    StateHasChanged();
                });
            });
            progress.Report("listing files");
            await Task.Run(async () =>
            {
                xmlws = XmlWorkspace.CreateFromDirectory(selectedFolder);
                progress.Report("loading files");
                var roots = await LoadFiles(xmlws.GetDocuments(SourceKind.Catalogue, SourceKind.Gamesystem)).ToListAsync();
                progress.Report("compiling data");
                compilation = WhamCompilation.Create(roots.ToImmutableArray());
                progress.Report("generating data diagnostics");
                var diags = compilation.GetDiagnostics();
                progress.Report($"Found {diags.Length} diagnostics (issues).");
            });
            StateHasChanged();
        }
    }

    static async IAsyncEnumerable<SourceTree> LoadFiles(IEnumerable<XmlDocument> files)
    {
        foreach (var file in files)
        {
            var tree = await SourceTree.CreateForDatafileAsync(file.DatafileInfo);
            yield return tree;
        }
    }
}
