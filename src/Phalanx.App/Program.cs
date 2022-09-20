using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Phalanx.App;
using Phalanx.App.Pages.Printing;
using Phalanx.App.Util;
using Phalanx.SampleDataset;
using WarHub.ArmouryModel.EditorServices;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<RosterFormatsProvider>();
builder.Services.AddSingleton<RosterEditorService>();

var host = builder.Build();

//get roster from file
var ws = SampleDataResources.CreateXmlWorkspace();
var rosterState = RosterState.CreateFromNodes(ws.Documents.Select(x => x.GetRootAsync().Result!));

var rosterService = host.Services.GetRequiredService<RosterEditorService>();

rosterService.LoadRoster(rosterState);

await host.RunAsync();
