using Microsoft.Extensions.Logging;
using SyncPlaylistApp.Services;
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

        // Register authentication services
        builder.Services.AddSingleton<SpotifyAuthService>();
        builder.Services.AddSingleton<AppleMusicAuthService>();
        builder.Services.AddSingleton<YouTubeMusicAuthService>();

        // Register playlist services
        builder.Services.AddSingleton<SpotifyPlaylistService>();
        builder.Services.AddSingleton<AppleMusicPlaylistService>();
        builder.Services.AddSingleton<YouTubeMusicPlaylistService>();

        // Register sync service
        builder.Services.AddSingleton<PlaylistSyncService>();

        // Register ViewModels
        builder.Services.AddSingleton<MainViewModel>();

        // Register Views
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
