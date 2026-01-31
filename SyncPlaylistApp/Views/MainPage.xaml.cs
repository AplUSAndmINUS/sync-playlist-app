using SyncPlaylistApp.Models;
using SyncPlaylistApp.ViewModels;

namespace SyncPlaylistApp.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void OnLoadSpotifyPlaylists(object sender, EventArgs e)
    {
        await _viewModel.LoadPlaylistsCommand.Execute(MusicService.Spotify);
    }

    private async void OnLoadAppleMusicPlaylists(object sender, EventArgs e)
    {
        await _viewModel.LoadPlaylistsCommand.Execute(MusicService.AppleMusic);
    }

    private async void OnLoadYouTubeMusicPlaylists(object sender, EventArgs e)
    {
        await _viewModel.LoadPlaylistsCommand.Execute(MusicService.YouTubeMusic);
    }

    private void OnDestinationChanged(object sender, CheckedChangedEventArgs e)
    {
        _viewModel.DestinationServices.Clear();

        if (SpotifyDestCheck.IsChecked)
            _viewModel.DestinationServices.Add(MusicService.Spotify);
        
        if (AppleMusicDestCheck.IsChecked)
            _viewModel.DestinationServices.Add(MusicService.AppleMusic);
        
        if (YouTubeMusicDestCheck.IsChecked)
            _viewModel.DestinationServices.Add(MusicService.YouTubeMusic);
    }
}
