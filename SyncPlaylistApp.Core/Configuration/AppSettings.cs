namespace SyncPlaylistApp.Core.Configuration;

/// <summary>
/// Application settings and API credentials
/// </summary>
public static class AppSettings
{
    // Spotify Configuration
    public static string SpotifyClientId { get; set; } = "YOUR_SPOTIFY_CLIENT_ID";
    public static string SpotifyClientSecret { get; set; } = "YOUR_SPOTIFY_CLIENT_SECRET";
    public static string SpotifyRedirectUri { get; set; } = "syncplaylistapp://callback";

    // Apple Music Configuration
    public static string AppleMusicDeveloperToken { get; set; } = "YOUR_APPLE_MUSIC_DEVELOPER_TOKEN";
    public static string AppleMusicTeamId { get; set; } = "YOUR_APPLE_TEAM_ID";
    public static string AppleMusicKeyId { get; set; } = "YOUR_APPLE_KEY_ID";
    public static string AppleMusicPrivateKey { get; set; } = string.Empty; // Content of .p8 file

    // YouTube Music Configuration  
    public static string YouTubeClientId { get; set; } = "YOUR_YOUTUBE_CLIENT_ID";
    public static string YouTubeClientSecret { get; set; } = "YOUR_YOUTUBE_CLIENT_SECRET";
    public static string YouTubeRedirectUri { get; set; } = "syncplaylistapp://callback";

    // General Configuration
    public static string AppVersion { get; set; } = "1.0.0";
    public static string AppName { get; set; } = "Sync Playlist App";
}
