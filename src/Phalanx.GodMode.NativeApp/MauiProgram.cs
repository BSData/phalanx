using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Fast.Components.FluentUI;
using Microsoft.Fast.Components.FluentUI.Infrastructure;

namespace Phalanx.GodMode.NativeApp;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.AddFluentUIComponents(options =>
        {
            options.HostingModel = BlazorHostingModel.Hybrid;
        });
        builder.Services.AddScoped<IStaticAssetService, FileBasedStaticAssetService>();
        builder.Services.AddSingleton<StaticAssetCache>();

        return builder.Build();
    }
}
