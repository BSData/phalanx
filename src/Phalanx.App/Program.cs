using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Phalanx.App;
using Phalanx.App.Pages.Printing;
using Microsoft.Fast.Components.FluentUI;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<RosterFormatsProvider>();
builder.Services.AddFluentUIComponents();

await builder.Build().RunAsync();
