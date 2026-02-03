using SyncPlaylistApp.Core.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Core.Services;

/// <summary>
/// Service for interacting with Spotify playlists
/// </summary>
public class SpotifyPlaylistService : IPlaylistService
{
    private readonly SpotifyAuthService _authService;

    public MusicService ServiceType => MusicService.Spotify;

    public SpotifyPlaylistService(SpotifyAuthService authService)
    {
        _authService = authService;
    }

    public Task<List<Playlist>> GetPlaylistsAsync()
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Spotify");

        // In real implementation, would make API call to Spotify
        // GET https://api.spotify.com/v1/me/playlists
        Debug.WriteLine("Fetching Spotify playlists...");

        // Simulated data
        return Task.FromResult(new List<Playlist>
        {
            new Playlist
            {
                Id = "spotify_playlist_1",
                Name = "My Favorite Songs",
                Description = "A collection of my favorite tracks",
                CoverImageUrl = "https://example.com/cover1.jpg",
                TrackCount = 50,
                Source = MusicService.Spotify
            },
            new Playlist
            {
                Id = "spotify_playlist_2",
                Name = "Workout Mix",
                Description = "High energy tracks for workout",
                CoverImageUrl = "https://example.com/cover2.jpg",
                TrackCount = 30,
                Source = MusicService.Spotify
            }
        });
    }

    public Task<Playlist> GetPlaylistDetailsAsync(string playlistId)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Spotify");

        // In real implementation, would make API call to Spotify
        // GET https://api.spotify.com/v1/playlists/{playlist_id}
        Debug.WriteLine($"Fetching Spotify playlist details for {playlistId}...");

        // Simulated data
        return Task.FromResult(new Playlist
        {
            Id = playlistId,
            Name = "My Favorite Songs",
            Description = "A collection of my favorite tracks",
            CoverImageUrl = "https://example.com/cover1.jpg",
            TrackCount = 3,
            Source = MusicService.Spotify,
            Tracks = new List<Track>
            {
                new Track
                {
                    Id = "spotify_track_1",
                    Name = "Bohemian Rhapsody",
                    Artist = "Queen",
                    Album = "A Night at the Opera",
                    DurationMs = 354000,
                    IsrcCode = "GBUM71029604"
                },
                new Track
                {
                    Id = "spotify_track_2",
                    Name = "Stairway to Heaven",
                    Artist = "Led Zeppelin",
                    Album = "Led Zeppelin IV",
                    DurationMs = 482000,
                    IsrcCode = "USAT29900011"
                },
                new Track
                {
                    Id = "spotify_track_3",
                    Name = "Hotel California",
                    Artist = "Eagles",
                    Album = "Hotel California",
                    DurationMs = 391000,
                    IsrcCode = "USEE10001993"
                }
            }
        });
    }

    public Task<string> CreatePlaylistAsync(string name, string description)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Spotify");

        // In real implementation, would make API call to Spotify
        // POST https://api.spotify.com/v1/users/{user_id}/playlists
        Debug.WriteLine($"Creating Spotify playlist: {name}");

        return Task.FromResult($"spotify_new_playlist_{Guid.NewGuid().ToString("N")[..8]}");
    }

    public Task<bool> AddTracksToPlaylistAsync(string playlistId, List<Track> tracks)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Spotify");

        // In real implementation, would make API call to Spotify
        // POST https://api.spotify.com/v1/playlists/{playlist_id}/tracks
        Debug.WriteLine($"Adding {tracks.Count} tracks to Spotify playlist {playlistId}");

        return Task.FromResult(true);
    }

    public Task<Track?> SearchTrackAsync(Track track)
    {
        var account = _authService.GetCurrentAccount();
        if (account == null || !account.IsAuthenticated)
            throw new InvalidOperationException("User is not authenticated with Spotify");

        // In real implementation, would make API call to Spotify
        // GET https://api.spotify.com/v1/search
        Debug.WriteLine($"Searching for track: {track.Name} by {track.Artist}");

        // Simulate search - in real app would search by ISRC first, then by name/artist
        // For demonstration, we'll assume tracks are found
        return Task.FromResult<Track?>(new Track
        {
            Id = $"spotify_found_{Guid.NewGuid().ToString("N")[..8]}",
            Name = track.Name,
            Artist = track.Artist,
            Album = track.Album,
            DurationMs = track.DurationMs,
            IsrcCode = track.IsrcCode
        });
    }
}
