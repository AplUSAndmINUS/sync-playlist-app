namespace SyncPlaylistApp.Configuration;

/// <summary>
/// Configuration class for API credentials and settings.
/// In production, these should be stored securely (e.g., Azure Key Vault, app settings with user secrets)
/// </summary>
public static class AppSettings
{
    // Spotify Configuration
    // Get these from: https://developer.spotify.com/dashboard
    public static string SpotifyClientId { get; set; } = "YOUR_SPOTIFY_CLIENT_ID";
    public static string SpotifyClientSecret { get; set; } = "YOUR_SPOTIFY_CLIENT_SECRET";
    public static string SpotifyRedirectUri { get; set; } = "syncplaylistapp://callback";
    
    // Apple Music Configuration
    // Get these from: https://developer.apple.com/
    public static string AppleMusicDeveloperToken { get; set; } = "YOUR_APPLE_MUSIC_DEVELOPER_TOKEN";
    public static string AppleMusicTeamId { get; set; } = "YOUR_APPLE_MUSIC_TEAM_ID";
    public static string AppleMusicKeyId { get; set; } = "YOUR_APPLE_MUSIC_KEY_ID";
    
    // YouTube Music Configuration
    // Get these from: https://console.cloud.google.com/
    public static string YouTubeClientId { get; set; } = "YOUR_YOUTUBE_CLIENT_ID";
    public static string YouTubeClientSecret { get; set; } = "YOUR_YOUTUBE_CLIENT_SECRET";
    public static string YouTubeRedirectUri { get; set; } = "syncplaylistapp://callback";
    
    // API Endpoints
    public static class Endpoints
    {
        // Spotify
        public const string SpotifyAuthUrl = "https://accounts.spotify.com/authorize";
        public const string SpotifyTokenUrl = "https://accounts.spotify.com/api/token";
        public const string SpotifyApiBase = "https://api.spotify.com/v1";
        
        // Apple Music
        public const string AppleMusicApiBase = "https://api.music.apple.com/v1";
        
        // YouTube
        public const string YouTubeAuthUrl = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string YouTubeTokenUrl = "https://oauth2.googleapis.com/token";
        public const string YouTubeApiBase = "https://www.googleapis.com/youtube/v3";
    }
    
    // OAuth Scopes
    public static class Scopes
    {
        public const string SpotifyScopes = "playlist-read-private playlist-read-collaborative playlist-modify-public playlist-modify-private user-library-read";
        public const string YouTubeScopes = "https://www.googleapis.com/auth/youtube https://www.googleapis.com/auth/youtube.readonly";
    }
}
