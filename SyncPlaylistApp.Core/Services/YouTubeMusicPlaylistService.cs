using SyncPlaylistApp.Core.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Core.Services;

/// <summary>
/// Service for interacting with YouTube Music playlists
/// </summary>
public class YouTubeMusicPlaylistService : IPlaylistService
{
  private readonly YouTubeMusicAuthService _authService;

  public MusicService ServiceType => MusicService.YouTubeMusic;

  public YouTubeMusicPlaylistService(YouTubeMusicAuthService authService)
  {
    _authService = authService;
  }

  public Task<List<Playlist>> GetPlaylistsAsync()
  {
    var account = _authService.GetCurrentAccount();
    if (account == null || !account.IsAuthenticated)
      throw new InvalidOperationException("User is not authenticated with YouTube Music");

    // In real implementation, would make API call to YouTube Data API
    Debug.WriteLine("Fetching YouTube Music playlists...");

    return Task.FromResult(new List<Playlist>
        {
            new Playlist
            {
                Id = "youtube_playlist_1",
                Name = "Discover Weekly Alternative",
                Description = "Weekly music discoveries",
                CoverImageUrl = "https://example.com/youtube_cover1.jpg",
                TrackCount = 40,
                Source = MusicService.YouTubeMusic
            }
        });
  }

  public Task<Playlist> GetPlaylistDetailsAsync(string playlistId)
  {
    var account = _authService.GetCurrentAccount();
    if (account == null || !account.IsAuthenticated)
      throw new InvalidOperationException("User is not authenticated with YouTube Music");

    Debug.WriteLine($"Fetching YouTube Music playlist details for {playlistId}...");

    return Task.FromResult(new Playlist
    {
      Id = playlistId,
      Name = "Discover Weekly Alternative",
      Description = "Weekly music discoveries",
      CoverImageUrl = "https://example.com/youtube_cover1.jpg",
      TrackCount = 2,
      Source = MusicService.YouTubeMusic,
      Tracks = new List<Track>
            {
                new Track
                {
                    Id = "youtube_track_1",
                    Name = "Champagne Supernova",
                    Artist = "Oasis",
                    Album = "(What's the Story) Morning Glory?",
                    DurationMs = 447000,
                    IsrcCode = "GBAYE9500070"
                },
                new Track
                {
                    Id = "youtube_track_2",
                    Name = "Everlong",
                    Artist = "Foo Fighters",
                    Album = "The Colour and the Shape",
                    DurationMs = 250000,
                    IsrcCode = "USRC11700001"
                }
            }
    });
  }

  public Task<string> CreatePlaylistAsync(string name, string description)
  {
    var account = _authService.GetCurrentAccount();
    if (account == null || !account.IsAuthenticated)
      throw new InvalidOperationException("User is not authenticated with YouTube Music");

    Debug.WriteLine($"Creating YouTube Music playlist: {name}");

    return Task.FromResult($"youtube_new_playlist_{Guid.NewGuid().ToString("N")[..8]}");
  }

  public Task<bool> AddTracksToPlaylistAsync(string playlistId, List<Track> tracks)
  {
    var account = _authService.GetCurrentAccount();
    if (account == null || !account.IsAuthenticated)
      throw new InvalidOperationException("User is not authenticated with YouTube Music");

    Debug.WriteLine($"Adding {tracks.Count} tracks to YouTube Music playlist {playlistId}");

    return Task.FromResult(true);
  }

  public Task<Track?> SearchTrackAsync(Track track)
  {
    var account = _authService.GetCurrentAccount();
    if (account == null || !account.IsAuthenticated)
      throw new InvalidOperationException("User is not authenticated with YouTube Music");

    Debug.WriteLine($"Searching for track in YouTube Music: {track.Name} by {track.Artist}");

    // Simulate search - sometimes tracks won't be found
    // For demonstration, we'll return null for some tracks to show skipping behavior
    if (track.Name.Contains("Bohemian", StringComparison.OrdinalIgnoreCase))
    {
      Debug.WriteLine($"Track '{track.Name}' not found in YouTube Music");
      return Task.FromResult<Track?>(null); // Simulate track not found
    }

    return Task.FromResult<Track?>(new Track
    {
      Id = $"youtube_found_{Guid.NewGuid().ToString("N")[..8]}",
      Name = track.Name,
      Artist = track.Artist,
      Album = track.Album,
      DurationMs = track.DurationMs,
      IsrcCode = track.IsrcCode
    });
  }
}
