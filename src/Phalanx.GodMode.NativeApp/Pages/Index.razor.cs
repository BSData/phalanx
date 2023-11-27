using CommunityToolkit.Maui.Storage;
using WarHub.ArmouryModel.Concrete;
using WarHub.ArmouryModel;
using WarHub.ArmouryModel.Workspaces.BattleScribe;
using WarHub.ArmouryModel.Source;
using Microsoft.AspNetCore.Components;
using Phalanx.GodMode.NativeApp.Infrastructure;

namespace Phalanx.GodMode.NativeApp.Pages;

public partial class Index
{
    [Inject]
    NavigationManager Nav { get; set; } = null!;
    [Inject]
    WorkspaceManager Manager { get; set; } = null!;

    string? selectedFolder;
    private string? loadingStatus;

    void Edit(ICatalogueSymbol root)
    {
        Nav.NavigateTo(Nav.GetUriWithQueryParameters("/editor", new Dictionary<string, object?> { ["id"] = root.Id }));
    }

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
                var xmlws = XmlWorkspace.CreateFromDirectory(selectedFolder);
                Manager.CurrentXmlWorkspace = xmlws;
                progress.Report("loading files");
                var roots = await LoadFiles(xmlws.GetDocuments(SourceKind.Catalogue, SourceKind.Gamesystem)).ToListAsync();
                progress.Report("compiling data");
                var compilation = WhamCompilation.Create(roots.ToImmutableArray());
                Manager.CurrentCompilation = compilation;
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
