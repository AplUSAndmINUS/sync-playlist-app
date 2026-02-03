using System.Windows.Input;
using SyncPlaylistApp.Core.Models;
using SyncPlaylistApp.Core.Services;
using SyncPlaylistApp.Core.ViewModels;

namespace SyncPlaylistApp.ViewModels;

/// <summary>
/// MAUI-specific MainViewModel that extends Core with Command bindings for XAML
/// </summary>
public class MainViewModel : Core.ViewModels.MainViewModel
{
    public MainViewModel(
        SpotifyAuthService spotifyAuth,
        AppleMusicAuthService appleMusicAuth,
        YouTubeMusicAuthService youtubeAuth,
        SpotifyPlaylistService spotifyPlaylist,
        AppleMusicPlaylistService appleMusicPlaylist,
        YouTubeMusicPlaylistService youtubePlaylist,
        PlaylistSyncService syncService)
        : base(spotifyAuth, appleMusicAuth, youtubeAuth, spotifyPlaylist, appleMusicPlaylist, youtubePlaylist, syncService)
    {
        // Initialize MAUI-specific Commands for XAML bindings
        SignInSpotifyCommand = new Command(async () => await SignInSpotifyAsync());
        SignInAppleMusicCommand = new Command(async () => await SignInAppleMusicAsync());
        SignInYouTubeMusicCommand = new Command(async () => await SignInYouTubeMusicAsync());

        // LoadPlaylistsCommand now maps to different service buttons
        LoadPlaylistsCommand = new Command<MusicService>(async (service) => await LoadPlaylistsAsync(service));

        SyncPlaylistCommand = new Command(async () => await SyncPlaylistAsync(), () => CanSync());

        // Subscribe to property changes to update Command.CanExecute
        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(IsSyncing) || e.PropertyName == nameof(SelectedPlaylist))
            {
                ((Command)SyncPlaylistCommand).ChangeCanExecute();
            }
        };

        // Subscribe to collection changes to update CanSync
        DestinationServices.CollectionChanged += (s, e) => ((Command)SyncPlaylistCommand).ChangeCanExecute();
    }

    // MAUI Command bindings for XAML
    public ICommand SignInSpotifyCommand { get; }
    public ICommand SignInAppleMusicCommand { get; }
    public ICommand SignInYouTubeMusicCommand { get; }
    public ICommand LoadPlaylistsCommand { get; }
    public ICommand SyncPlaylistCommand { get; }
}
