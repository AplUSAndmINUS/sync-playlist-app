using SyncPlaylistApp.Services;
using SyncPlaylistApp.ViewModels;
using SyncPlaylistApp.Views;

namespace SyncPlaylistApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Register services and viewmodels (in a real app, would use DI container)
        var spotifyAuth = new SpotifyAuthService();
        var appleMusicAuth = new AppleMusicAuthService();
        var youtubeAuth = new YouTubeMusicAuthService();

        var spotifyPlaylist = new SpotifyPlaylistService(spotifyAuth);
        var appleMusicPlaylist = new AppleMusicPlaylistService(appleMusicAuth);
        var youtubePlaylist = new YouTubeMusicPlaylistService(youtubeAuth);

        var syncService = new PlaylistSyncService(spotifyPlaylist, appleMusicPlaylist, youtubePlaylist);

        var mainViewModel = new MainViewModel(
            spotifyAuth,
            appleMusicAuth,
            youtubeAuth,
            spotifyPlaylist,
            appleMusicPlaylist,
            youtubePlaylist,
            syncService);

        MainPage = new NavigationPage(new MainPage(mainViewModel));
    }
}
