using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SyncPlaylistApp.Models;
using SyncPlaylistApp.Services;

namespace SyncPlaylistApp.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly SpotifyAuthService _spotifyAuth;
    private readonly AppleMusicAuthService _appleMusicAuth;
    private readonly YouTubeMusicAuthService _youtubeAuth;
    private readonly SpotifyPlaylistService _spotifyPlaylist;
    private readonly AppleMusicPlaylistService _appleMusicPlaylist;
    private readonly YouTubeMusicPlaylistService _youtubePlaylist;
    private readonly PlaylistSyncService _syncService;

    private bool _isSpotifyAuthenticated;
    private bool _isAppleMusicAuthenticated;
    private bool _isYouTubeMusicAuthenticated;
    private bool _isSyncing;
    private string _syncStatus = string.Empty;

    public MainViewModel(
        SpotifyAuthService spotifyAuth,
        AppleMusicAuthService appleMusicAuth,
        YouTubeMusicAuthService youtubeAuth,
        SpotifyPlaylistService spotifyPlaylist,
        AppleMusicPlaylistService appleMusicPlaylist,
        YouTubeMusicPlaylistService youtubePlaylist,
        PlaylistSyncService syncService)
    {
        _spotifyAuth = spotifyAuth;
        _appleMusicAuth = appleMusicAuth;
        _youtubeAuth = youtubeAuth;
        _spotifyPlaylist = spotifyPlaylist;
        _appleMusicPlaylist = appleMusicPlaylist;
        _youtubePlaylist = youtubePlaylist;
        _syncService = syncService;

        // Initialize commands
        SignInSpotifyCommand = new Command(async () => await SignInSpotify());
        SignInAppleMusicCommand = new Command(async () => await SignInAppleMusic());
        SignInYouTubeMusicCommand = new Command(async () => await SignInYouTubeMusic());
        LoadPlaylistsCommand = new Command<MusicService>(async (service) => await LoadPlaylists(service));
        SyncPlaylistCommand = new Command(async () => await SyncPlaylist(), () => CanSync());

        // Subscribe to collection changes to update CanSync
        DestinationServices.CollectionChanged += (s, e) => ((Command)SyncPlaylistCommand).ChangeCanExecute();
    }

    public bool IsSpotifyAuthenticated
    {
        get => _isSpotifyAuthenticated;
        set { _isSpotifyAuthenticated = value; OnPropertyChanged(); }
    }

    public bool IsAppleMusicAuthenticated
    {
        get => _isAppleMusicAuthenticated;
        set { _isAppleMusicAuthenticated = value; OnPropertyChanged(); }
    }

    public bool IsYouTubeMusicAuthenticated
    {
        get => _isYouTubeMusicAuthenticated;
        set { _isYouTubeMusicAuthenticated = value; OnPropertyChanged(); }
    }

    public bool IsSyncing
    {
        get => _isSyncing;
        set
        {
            _isSyncing = value;
            OnPropertyChanged();
            ((Command)SyncPlaylistCommand).ChangeCanExecute();
        }
    }

    public string SyncStatus
    {
        get => _syncStatus;
        set { _syncStatus = value; OnPropertyChanged(); }
    }

    public ObservableCollection<Playlist> SourcePlaylists { get; } = new();

    private MusicService _selectedSourceService;
    public MusicService SelectedSourceService
    {
        get => _selectedSourceService;
        set
        {
            _selectedSourceService = value;
            OnPropertyChanged();
        }
    }

    private Playlist? _selectedPlaylist;
    public Playlist? SelectedPlaylist
    {
        get => _selectedPlaylist;
        set
        {
            _selectedPlaylist = value;
            OnPropertyChanged();
            ((Command)SyncPlaylistCommand).ChangeCanExecute();
        }
    }

    public ObservableCollection<MusicService> DestinationServices { get; } = new();

    public ICommand SignInSpotifyCommand { get; }
    public ICommand SignInAppleMusicCommand { get; }
    public ICommand SignInYouTubeMusicCommand { get; }
    public ICommand LoadPlaylistsCommand { get; }
    public ICommand SyncPlaylistCommand { get; }

    private async Task SignInSpotify()
    {
        try
        {
            await _spotifyAuth.AuthenticateAsync();
            IsSpotifyAuthenticated = await _spotifyAuth.IsAuthenticatedAsync();
            SyncStatus = "Spotify: Signed in successfully";
        }
        catch (Exception ex)
        {
            SyncStatus = $"Spotify sign-in failed: {ex.Message}";
        }
    }

    private async Task SignInAppleMusic()
    {
        try
        {
            await _appleMusicAuth.AuthenticateAsync();
            IsAppleMusicAuthenticated = await _appleMusicAuth.IsAuthenticatedAsync();
            SyncStatus = "Apple Music: Signed in successfully";
        }
        catch (Exception ex)
        {
            SyncStatus = $"Apple Music sign-in failed: {ex.Message}";
        }
    }

    private async Task SignInYouTubeMusic()
    {
        try
        {
            await _youtubeAuth.AuthenticateAsync();
            IsYouTubeMusicAuthenticated = await _youtubeAuth.IsAuthenticatedAsync();
            SyncStatus = "YouTube Music: Signed in successfully";
        }
        catch (Exception ex)
        {
            SyncStatus = $"YouTube Music sign-in failed: {ex.Message}";
        }
    }

    private async Task LoadPlaylists(MusicService service)
    {
        try
        {
            SourcePlaylists.Clear();
            SelectedPlaylist = null;
            SelectedSourceService = service;

            IPlaylistService? playlistService = service switch
            {
                MusicService.Spotify => _spotifyPlaylist,
                MusicService.AppleMusic => _appleMusicPlaylist,
                MusicService.YouTubeMusic => _youtubePlaylist,
                _ => null
            };

            if (playlistService != null)
            {
                var playlists = await playlistService.GetPlaylistsAsync();
                foreach (var playlist in playlists)
                {
                    SourcePlaylists.Add(playlist);
                }
                SyncStatus = $"Loaded {playlists.Count} playlists from {service}";
            }
        }
        catch (Exception ex)
        {
            SyncStatus = $"Failed to load playlists: {ex.Message}";
        }
    }

    private async Task SyncPlaylist()
    {
        if (SelectedPlaylist == null || DestinationServices.Count == 0)
            return;

        try
        {
            IsSyncing = true;
            SyncStatus = "Syncing playlist...";

            // Get full playlist details with tracks
            IPlaylistService? sourceService = SelectedSourceService switch
            {
                MusicService.Spotify => _spotifyPlaylist,
                MusicService.AppleMusic => _appleMusicPlaylist,
                MusicService.YouTubeMusic => _youtubePlaylist,
                _ => null
            };

            if (sourceService == null)
                throw new InvalidOperationException("No source service selected");

            var fullPlaylist = await sourceService.GetPlaylistDetailsAsync(SelectedPlaylist.Id);

            // Sync to all selected destination services
            var results = await _syncService.SyncToMultipleServicesAsync(
                fullPlaylist,
                DestinationServices.ToList());

            // Build status message
            if (results.Count == 0)
            {
                SyncStatus = "No destinations to sync. Source service cannot be a destination.";
            }
            else
            {
                var statusMessages = new List<string>();
                foreach (var result in results)
                {
                    if (!result.IsSuccess)
                    {
                        statusMessages.Add($"{result.DestinationService}: Failed - {result.ErrorMessage}");
                    }
                    else
                    {
                        var msg = $"{result.DestinationService}: {result.SuccessfulTracks}/{result.TotalTracks} tracks synced";
                        if (result.SkippedTracks > 0)
                        {
                            msg += $", {result.SkippedTracks} skipped";
                        }
                        statusMessages.Add(msg);
                    }
                }

                SyncStatus = string.Join("; ", statusMessages);
            }
        }
        catch (Exception ex)
        {
            SyncStatus = $"Sync failed: {ex.Message}";
        }
        finally
        {
            IsSyncing = false;
        }
    }

    private bool CanSync()
    {
        return SelectedPlaylist != null && DestinationServices.Count > 0 && !IsSyncing;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
