using SyncPlaylistApp.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Services;

/// <summary>
/// Authentication service for YouTube Music using OAuth 2.0
/// </summary>
public class YouTubeMusicAuthService : IMusicServiceAuth
{
    private ServiceAccount? _currentAccount;
    private const string ClientId = "YOUR_YOUTUBE_CLIENT_ID"; // Configure in app settings
    private const string RedirectUri = "syncplaylistapp://callback";
    private const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
    private const string TokenEndpoint = "https://oauth2.googleapis.com/token";

    public MusicService ServiceType => MusicService.YouTubeMusic;

    public async Task<ServiceAccount> AuthenticateAsync()
    {
        try
        {
            // Create authorization URL for YouTube Music (uses YouTube Data API)
            var scope = "https://www.googleapis.com/auth/youtube https://www.googleapis.com/auth/youtube.readonly";
            var state = Guid.NewGuid().ToString("N");
            
            var authUrl = $"{AuthorizationEndpoint}?" +
                $"client_id={ClientId}&" +
                $"response_type=code&" +
                $"redirect_uri={Uri.EscapeDataString(RedirectUri)}&" +
                $"scope={Uri.EscapeDataString(scope)}&" +
                $"state={state}&" +
                $"access_type=offline";

            Debug.WriteLine($"Opening YouTube Music auth URL: {authUrl}");

            // Simulate authentication (in real app, would handle OAuth callback)
            _currentAccount = new ServiceAccount
            {
                Service = MusicService.YouTubeMusic,
                AccessToken = "simulated_youtube_access_token",
                RefreshToken = "simulated_youtube_refresh_token",
                TokenExpiry = DateTime.UtcNow.AddHours(1),
                UserId = "youtube_user_123",
                UserName = "YouTube Music User",
                IsAuthenticated = true
            };

            return _currentAccount;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"YouTube Music authentication error: {ex.Message}");
            throw new InvalidOperationException("Failed to authenticate with YouTube Music", ex);
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
