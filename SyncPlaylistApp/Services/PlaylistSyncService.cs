using SyncPlaylistApp.Models;
using System.Diagnostics;

namespace SyncPlaylistApp.Services;

/// <summary>
/// Service for syncing playlists between music services
/// </summary>
public class PlaylistSyncService
{
    private readonly Dictionary<MusicService, IPlaylistService> _playlistServices;

    public PlaylistSyncService(
        SpotifyPlaylistService spotifyService,
        AppleMusicPlaylistService appleMusicService,
        YouTubeMusicPlaylistService youtubeService)
    {
        _playlistServices = new Dictionary<MusicService, IPlaylistService>
        {
            { MusicService.Spotify, spotifyService },
            { MusicService.AppleMusic, appleMusicService },
            { MusicService.YouTubeMusic, youtubeService }
        };
    }

    public async Task<SyncResult> SyncPlaylistAsync(
        Playlist sourcePlaylist,
        MusicService destinationService,
        string? destinationPlaylistId = null)
    {
        var result = new SyncResult
        {
            DestinationService = destinationService,
            TotalTracks = sourcePlaylist.Tracks.Count
        };

        try
        {
            if (!_playlistServices.TryGetValue(destinationService, out var destService))
            {
                throw new InvalidOperationException($"No service available for {destinationService}");
            }

            // Create new playlist if no destination ID provided
            if (string.IsNullOrEmpty(destinationPlaylistId))
            {
                var playlistName = $"{sourcePlaylist.Name} (from {sourcePlaylist.Source})";
                var playlistDesc = $"Synced from {sourcePlaylist.Source}: {sourcePlaylist.Description}";
                destinationPlaylistId = await destService.CreatePlaylistAsync(playlistName, playlistDesc);
                Debug.WriteLine($"Created new playlist: {destinationPlaylistId}");
            }

            result.DestinationPlaylistId = destinationPlaylistId;

            // Process each track
            var tracksToAdd = new List<Track>();
            
            foreach (var track in sourcePlaylist.Tracks)
            {
                try
                {
                    // Search for the track in the destination service
                    var foundTrack = await destService.SearchTrackAsync(track);
                    
                    if (foundTrack != null)
                    {
                        tracksToAdd.Add(foundTrack);
                        result.SuccessfulTracks++;
                        Debug.WriteLine($"Found track: {track.Name} by {track.Artist}");
                    }
                    else
                    {
                        // Track not found - skip it as per requirements
                        result.SkippedTracks++;
                        result.SkippedTrackNames.Add($"{track.Name} by {track.Artist}");
                        Debug.WriteLine($"Skipping track (not found): {track.Name} by {track.Artist}");
                    }
                }
                catch (Exception ex)
                {
                    // If search fails, skip the track
                    result.SkippedTracks++;
                    result.SkippedTrackNames.Add($"{track.Name} by {track.Artist}");
                    Debug.WriteLine($"Error searching for track '{track.Name}': {ex.Message}");
                }
            }

            // Add all found tracks to the destination playlist
            if (tracksToAdd.Count > 0)
            {
                await destService.AddTracksToPlaylistAsync(destinationPlaylistId, tracksToAdd);
                Debug.WriteLine($"Added {tracksToAdd.Count} tracks to playlist {destinationPlaylistId}");
            }

            return result;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error syncing playlist: {ex.Message}");
            throw;
        }
    }

    public async Task<List<SyncResult>> SyncToMultipleServicesAsync(
        Playlist sourcePlaylist,
        List<MusicService> destinationServices)
    {
        var results = new List<SyncResult>();

        foreach (var destService in destinationServices)
        {
            if (destService == sourcePlaylist.Source)
            {
                Debug.WriteLine($"Skipping destination service {destService} as it's the source");
                continue;
            }

            try
            {
                var result = await SyncPlaylistAsync(sourcePlaylist, destService);
                results.Add(result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to sync to {destService}: {ex.Message}");
                // Continue with other services
            }
        }

        return results;
    }
}
