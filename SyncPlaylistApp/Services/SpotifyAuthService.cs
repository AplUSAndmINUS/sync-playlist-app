using SyncPlaylistApp.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Services;

/// <summary>
/// Authentication service for Spotify using OAuth 2.0
/// </summary>
public class SpotifyAuthService : IMusicServiceAuth
{
    private ServiceAccount? _currentAccount;
    private const string ClientId = "YOUR_SPOTIFY_CLIENT_ID"; // Configure in app settings
    private const string RedirectUri = "syncplaylistapp://callback";
    private const string AuthorizationEndpoint = "https://accounts.spotify.com/authorize";
    private const string TokenEndpoint = "https://accounts.spotify.com/api/token";

    public MusicService ServiceType => MusicService.Spotify;

    public async Task<ServiceAccount> AuthenticateAsync()
    {
        try
        {
            // Create authorization URL
            var scope = "playlist-read-private playlist-read-collaborative playlist-modify-public playlist-modify-private user-library-read";
            var state = Guid.NewGuid().ToString("N");
            
            var authUrl = $"{AuthorizationEndpoint}?" +
                $"client_id={ClientId}&" +
                $"response_type=code&" +
                $"redirect_uri={Uri.EscapeDataString(RedirectUri)}&" +
                $"scope={Uri.EscapeDataString(scope)}&" +
                $"state={state}";

            // Open browser for authentication
            // In a real implementation, this would use WebAuthenticator.AuthenticateAsync
            // For now, we'll simulate a successful authentication
            Debug.WriteLine($"Opening Spotify auth URL: {authUrl}");

            // Simulate authentication (in real app, would handle OAuth callback)
            _currentAccount = new ServiceAccount
            {
                Service = MusicService.Spotify,
                AccessToken = "simulated_spotify_access_token",
                RefreshToken = "simulated_spotify_refresh_token",
                TokenExpiry = DateTime.UtcNow.AddHours(1),
                UserId = "spotify_user_123",
                UserName = "Spotify User",
                IsAuthenticated = true
            };

            return _currentAccount;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Spotify authentication error: {ex.Message}");
            throw new InvalidOperationException("Failed to authenticate with Spotify", ex);
        }
    }

    public Task<bool> IsAuthenticatedAsync()
    {
        return Task.FromResult(_currentAccount?.IsAuthenticated == true && 
                              _currentAccount.TokenExpiry > DateTime.UtcNow);
    }

    public Task SignOutAsync()
    {
        _currentAccount = null;
        return Task.CompletedTask;
    }

    public ServiceAccount? GetCurrentAccount()
    {
        return _currentAccount;
    }
}
