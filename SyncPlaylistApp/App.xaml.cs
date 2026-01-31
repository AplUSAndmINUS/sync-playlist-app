using SyncPlaylistApp.Views;

namespace SyncPlaylistApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}
