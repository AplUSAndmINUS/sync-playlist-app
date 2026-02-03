using Microsoft.Extensions.DependencyInjection;
using SyncPlaylistApp.Core.Services;
using SyncPlaylistApp.Core.ViewModels;

namespace SyncPlaylistApp.Core;

/// <summary>
/// Extension methods for registering Core services with DI container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all SyncPlaylistApp.Core services (auth, playlists, sync, viewmodels)
    /// </summary>
    public static IServiceCollection AddSyncPlaylistServices(this IServiceCollection services)
    {
        // Register authentication services
        services.AddSingleton<SpotifyAuthService>();
        services.AddSingleton<AppleMusicAuthService>();
        services.AddSingleton<YouTubeMusicAuthService>();

        // Register playlist services
        services.AddSingleton<SpotifyPlaylistService>();
        services.AddSingleton<AppleMusicPlaylistService>();
        services.AddSingleton<YouTubeMusicPlaylistService>();

        // Register sync service
        services.AddSingleton<PlaylistSyncService>();

        // Register ViewModels
        services.AddTransient<MainViewModel>();

        return services;
    }
}
