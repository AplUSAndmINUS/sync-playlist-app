using Microsoft.Extensions.Logging;
using SyncPlaylistApp.Core;
using SyncPlaylistApp.ViewModels;
using SyncPlaylistApp.Views;

namespace SyncPlaylistApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Register Core services (auth, playlists, sync)
        builder.Services.AddSyncPlaylistServices();

        // Register MAUI-specific MainViewModel (with Command bindings)
        // This overrides the Core MainViewModel registration
        builder.Services.AddSingleton<MainViewModel>();

        // Register Views
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
