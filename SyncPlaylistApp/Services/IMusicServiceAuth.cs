using SyncPlaylistApp.Models;

namespace SyncPlaylistApp.Services;

public interface IMusicServiceAuth
{
    MusicService ServiceType { get; }
    Task<ServiceAccount> AuthenticateAsync();
    Task<bool> IsAuthenticatedAsync();
    Task SignOutAsync();
    ServiceAccount? GetCurrentAccount();
}
