using SyncPlaylistApp.Core.Models;
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

    private void OnLoadSpotifyPlaylists(object sender, EventArgs e)
    {
        if (_viewModel.LoadPlaylistsCommand.CanExecute(MusicService.Spotify))
            _viewModel.LoadPlaylistsCommand.Execute(MusicService.Spotify);
    }

    private void OnLoadAppleMusicPlaylists(object sender, EventArgs e)
    {
        if (_viewModel.LoadPlaylistsCommand.CanExecute(MusicService.AppleMusic))
            _viewModel.LoadPlaylistsCommand.Execute(MusicService.AppleMusic);
    }

    private void OnLoadYouTubeMusicPlaylists(object sender, EventArgs e)
    {
        if (_viewModel.LoadPlaylistsCommand.CanExecute(MusicService.YouTubeMusic))
            _viewModel.LoadPlaylistsCommand.Execute(MusicService.YouTubeMusic);
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
