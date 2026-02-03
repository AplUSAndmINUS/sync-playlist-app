# Sync Playlist App

A .NET MAUI application for syncing playlists across Spotify, Apple Music, and YouTube Music.

## Features

- **Multi-Platform Authentication**: Sign in to Spotify, Apple Music, and YouTube Music using their official OAuth/authentication systems
- **Playlist Synchronization**: Select a playlist from one service and sync it to one or more destination services
- **Smart Track Matching**: Uses ISRC codes and metadata to find matching tracks across platforms
- **Graceful Error Handling**: Automatically skips tracks that aren't found in the destination service
- **WinUI 3 Support**: Built with WinUI 3 for modern Windows applications
- **Tailwind-Inspired UI**: Clean, modern interface using Tailwind CSS color scheme

## Requirements

- .NET 9.0 SDK or later
- Windows 10 version 1809 (build 17763) or later for Windows development
- Visual Studio 2022 17.8+ with:
  - .NET MAUI workload
  - Windows App SDK
  - Universal Windows Platform development tools

## Configuration

Before running the app, you need to configure API credentials:

### Spotify
1. Create an app at [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Add redirect URI: `syncplaylistapp://callback`
3. Update `SpotifyAuthService.cs` with your Client ID

### Apple Music
1. Register at [Apple Developer Program](https://developer.apple.com/)
2. Create a MusicKit identifier
3. Generate a developer token
4. Update `AppleMusicAuthService.cs` with your developer token

### YouTube Music
1. Create a project in [Google Cloud Console](https://console.cloud.google.com/)
2. Enable YouTube Data API v3
3. Create OAuth 2.0 credentials
4. Add redirect URI: `syncplaylistapp://callback`
5. Update `YouTubeMusicAuthService.cs` with your Client ID

## Building and Running

### Windows (WinUI 3)
```bash
dotnet build SyncPlaylistApp.sln -f net9.0-windows10.0.19041.0
dotnet run --project SyncPlaylistApp/SyncPlaylistApp.csproj -f net9.0-windows10.0.19041.0
```

### Android
```bash
dotnet build SyncPlaylistApp.sln -f net9.0-android
```

### iOS
```bash
dotnet build SyncPlaylistApp.sln -f net9.0-ios
```

## Architecture

The application follows MVVM pattern:

- **Models**: Data structures for playlists, tracks, and service accounts
- **Services**: Authentication and playlist management for each music service
- **ViewModels**: Business logic and data binding
- **Views**: XAML-based UI with Tailwind-inspired styling

### Key Components

- `IMusicServiceAuth`: Interface for authentication services
- `IPlaylistService`: Interface for playlist operations
- `PlaylistSyncService`: Core sync logic that handles cross-platform synchronization
- `MainViewModel`: Main view model coordinating the UI and services

## How It Works

1. **Authentication**: Users sign in to one or more music services
2. **Source Selection**: Choose a source service and select a playlist
3. **Destination Selection**: Select one or more destination services
4. **Synchronization**: 
   - Fetches complete playlist details from source
   - For each track, searches the destination service by ISRC code or metadata
   - If found, adds to destination playlist
   - If not found, skips and logs the track
5. **Results**: Displays sync results including successful and skipped tracks

## API Integration Notes

This implementation uses simulated authentication and API calls for demonstration purposes. In production:

- Replace simulated auth with actual OAuth flows using `WebAuthenticator`
- Implement real API calls to:
  - Spotify Web API
  - Apple Music API (MusicKit)
  - YouTube Data API v3
- Add proper token refresh logic
- Implement secure credential storage
- Add rate limiting and error retry logic

## License

See LICENSE file for details.
