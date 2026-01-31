using SyncPlaylistApp.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Services;

/// <summary>
/// Service for interacting with Apple Music playlists
/// </summary>
public class AppleMusicPlaylistService : IPlaylistService
{
    private readonly AppleMusicAuthService _authService;

    public MusicService ServiceType => MusicService.AppleMusic;

    public AppleMusicPlaylistService(AppleMusicAuthService authService)
    {
        _authService = authService;
    }

    public async Task<List<Playlist>> GetPlaylistsAsync()
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Apple Music");

        // In real implementation, would make API call to Apple Music
        // GET https://api.music.apple.com/v1/me/library/playlists
        Debug.WriteLine("Fetching Apple Music playlists...");

        return new List<Playlist>
        {
            new Playlist
            {
                Id = "applemusic_playlist_1",
                Name = "Chill Vibes",
                Description = "Relaxing music",
                CoverImageUrl = "https://example.com/apple_cover1.jpg",
                TrackCount = 25,
                Source = MusicService.AppleMusic
            }
        };
    }

    public async Task<Playlist> GetPlaylistDetailsAsync(string playlistId)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Apple Music");

        Debug.WriteLine($"Fetching Apple Music playlist details for {playlistId}...");

        return new Playlist
        {
            Id = playlistId,
            Name = "Chill Vibes",
            Description = "Relaxing music",
            CoverImageUrl = "https://example.com/apple_cover1.jpg",
            TrackCount = 2,
            Source = MusicService.AppleMusic,
            Tracks = new List<Track>
            {
                new Track
                {
                    Id = "apple_track_1",
                    Name = "Weightless",
                    Artist = "Marconi Union",
                    Album = "Weightless",
                    DurationMs = 480000,
                    IsrcCode = "GBAHT0900123"
                },
                new Track
                {
                    Id = "apple_track_2",
                    Name = "Clair de Lune",
                    Artist = "Claude Debussy",
                    Album = "Suite Bergamasque",
                    DurationMs = 300000,
                    IsrcCode = "USHR10958652"
                }
            }
        };
    }

    public async Task<string> CreatePlaylistAsync(string name, string description)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Apple Music");

        Debug.WriteLine($"Creating Apple Music playlist: {name}");

        return $"apple_new_playlist_{Guid.NewGuid().ToString("N")[..8]}";
    }

    public async Task<bool> AddTracksToPlaylistAsync(string playlistId, List<Track> tracks)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Apple Music");

        Debug.WriteLine($"Adding {tracks.Count} tracks to Apple Music playlist {playlistId}");

        return true;
    }

    public async Task<Track?> SearchTrackAsync(Track track)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Apple Music");

        Debug.WriteLine($"Searching for track in Apple Music: {track.Name} by {track.Artist}");

        // Simulate search
        return new Track
        {
            Id = $"apple_found_{Guid.NewGuid().ToString("N")[..8]}",
            Name = track.Name,
            Artist = track.Artist,
            Album = track.Album,
            DurationMs = track.DurationMs,
            IsrcCode = track.IsrcCode
        };
    }
}
