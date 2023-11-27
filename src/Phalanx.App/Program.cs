using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.FluentUI.AspNetCore.Components;
using Phalanx.App;
using Phalanx.App.Pages.Printing;
using Phalanx.App.Util;
using WarHub.ArmouryModel.DataProviders;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<RosterFormatsProvider>();
builder.Services.AddSingleton<RosterEditorService>();
builder.Services.AddOptions<GalleryBrowserOptions>().BindConfiguration("GalleryBrowser");
builder.Services.AddSingleton<GalleryBrowserState>();

builder.Services.AddScoped<GalleryHttpClient>();
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
