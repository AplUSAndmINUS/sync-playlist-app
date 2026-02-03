namespace SyncPlaylistApp.Models;

public class Playlist
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CoverImageUrl { get; set; } = string.Empty;
    public int TrackCount { get; set; }
    public List<Track> Tracks { get; set; } = new();
    public MusicService Source { get; set; }
}

public class Track
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    public string Album { get; set; } = string.Empty;
    public int DurationMs { get; set; }
    public string IsrcCode { get; set; } = string.Empty; // International Standard Recording Code for cross-platform matching
}

public enum MusicService
{
    None,
    Spotify,
    AppleMusic,
    YouTubeMusic
}

public class ServiceAccount
{
    public MusicService Service { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenExpiry { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
}

public class SyncResult
{
    public int TotalTracks { get; set; }
    public int SuccessfulTracks { get; set; }
    public int SkippedTracks { get; set; }
    public List<string> SkippedTrackNames { get; set; } = new();
    public string DestinationPlaylistId { get; set; } = string.Empty;
    public MusicService DestinationService { get; set; }
    public bool IsSuccess { get; set; } = true;
    public string? ErrorMessage { get; set; }
}
