using SyncPlaylistApp.Core.Models;

namespace SyncPlaylistApp.Core.Services;

public interface IPlaylistService
{
  MusicService ServiceType { get; }
  Task<List<Playlist>> GetPlaylistsAsync();
  Task<Playlist> GetPlaylistDetailsAsync(string playlistId);
  Task<string> CreatePlaylistAsync(string name, string description);
  Task<bool> AddTracksToPlaylistAsync(string playlistId, List<Track> tracks);
  Task<Track?> SearchTrackAsync(Track track);
}
