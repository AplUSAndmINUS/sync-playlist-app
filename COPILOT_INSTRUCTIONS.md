# Copilot Instructions for Sync Playlist App

## Project Overview

This application allows users to sync music playlists across different streaming platforms (Spotify, Apple Music, YouTube Music). It features:

- **Multi-platform support**: Three separate projects targeting different platforms
- **Shared business logic**: Core library with maximum code reuse (~80%+)
- **Authentication**: OAuth-based authentication for each music service
- **Playlist sync**: One-way and multi-destination playlist synchronization
- **Modern UI**: MAUI for mobile/desktop, Blazor WebAssembly for web

## Architecture

### Solution Structure

```
SyncPlaylistApp.sln
├── SyncPlaylistApp/             # .NET MAUI app (iOS, Android, macOS, Windows)
├── SyncPlaylistApp.Core/        # Shared business logic library
└── SyncPlaylistApp.Web/         # Blazor WebAssembly web app
```

### Core Library (`SyncPlaylistApp.Core`)

**Target Framework**: `net10.0` (platform-agnostic)

**Purpose**: Contains all platform-independent business logic, models, and services

**Key Components**:

- `Models/MusicModels.cs`: Data models (Playlist, Track, SyncResult, etc.)
- `Services/`: Authentication and playlist operations for each platform
  - `IMusicServiceAuth.cs`, `IPlaylistService.cs`: Service interfaces
  - `SpotifyAuthService.cs`, `SpotifyPlaylistService.cs`
  - `AppleMusicAuthService.cs`, `AppleMusicPlaylistService.cs`
  - `YouTubeMusicAuthService.cs`, `YouTubeMusicPlaylistService.cs`
  - `PlaylistSyncService.cs`: Orchestrates multi-platform sync operations
- `ViewModels/MainViewModel.cs`: Core platform-agnostic ViewModel with business logic
- `Configuration/AppSettings.cs`: API credentials and app settings
- `ServiceCollectionExtensions.cs`: DI registration helper method

**Dependencies**:
- `Microsoft.Extensions.DependencyInjection.Abstractions` (v9.0.0)

**Usage Pattern**:
```csharp
// In Program.cs / MauiProgram.cs
builder.Services.AddSyncPlaylistServices();
```

### MAUI App (`SyncPlaylistApp`)

**Target Frameworks**: `net10.0-android`, `net10.0-ios`, `net10.0-maccatalyst`, `net10.0-windows`

**Purpose**: Native mobile and desktop UI using XAML

**Key Components**:

- `ViewModels/MainViewModel.cs`: MAUI-specific wrapper that adds `ICommand` bindings for XAML
- `Views/MainPage.xaml`: Main UI page with data bindings
- `MauiProgram.cs`: App configuration and DI setup
- `Platforms/`: Platform-specific entry points (required in .NET 10)
  - `iOS/Program.cs`, `iOS/AppDelegate.cs`
  - `MacCatalyst/Program.cs`, `MacCatalyst/AppDelegate.cs`
  - `Android/MainActivity.cs`, `Android/MainApplication.cs`

**Important Patterns**:

1. **ViewModel Extension**: MAUI MainViewModel inherits from Core MainViewModel and adds Command bindings:
   ```csharp
   public class MainViewModel : Core.ViewModels.MainViewModel
   {
       public ICommand SyncCommand { get; }
       // Commands wrap async methods from base class
   }
   ```

2. **Namespace References**: Uses Core types via `SyncPlaylistApp.Core.Models`, `SyncPlaylistApp.Core.Services`

3. **XAML**: Binds to Core models via assembly-qualified namespace:
   ```xml
   xmlns:models="clr-namespace:SyncPlaylistApp.Core.Models;assembly=SyncPlaylistApp.Core"
   ```

### Blazor WebAssembly App (`SyncPlaylistApp.Web`)

**Target Framework**: `net10.0`

**Purpose**: Cross-platform web UI that runs entirely in browser via WebAssembly

**Key Components**:

- `Pages/Home.razor`: Authentication landing page
- `Pages/Sync.razor`: Playlist sync UI
- `Program.cs`: Blazor app configuration and DI setup

**Important Patterns**:

1. **Direct Method Calls**: Blazor uses direct method calls instead of Commands:
   ```csharp
   <button @onclick="() => ViewModel.SignInSpotifyAsync()">Sign In</button>
   ```

2. **Reactive UI**: Uses `ViewModel.PropertyChanged` event to trigger re-renders:
   ```csharp
   protected override void OnInitialized()
   {
       ViewModel.PropertyChanged += (s, e) => InvokeAsync(StateHasChanged);
   }
   ```

3. **Bootstrap 5**: Uses Bootstrap CSS from CDN for responsive design

## Code Patterns and Conventions

### Async Method Patterns

- If a method doesn't use `await`, don't mark it `async` - use `Task.FromResult()`:
  ```csharp
  public Task<List<Playlist>> GetPlaylistsAsync()
  {
      return Task.FromResult(new List<Playlist> { /* stub data */ });
  }
  ```

### Error Handling

- All sync operations return `SyncResult` with `IsSuccess` and `ErrorMessage`:
  ```csharp
  catch (Exception ex)
  {
      return new SyncResult
      {
          IsSuccess = false,
          ErrorMessage = ex.Message,
          // ... other properties
      };
  }
  ```

### ViewModel Property Changes

- In MAUI ViewModels, trigger `ChangeCanExecute()` when properties affecting command state change:
  ```csharp
  set
  {
      _isSyncing = value;
      OnPropertyChanged();
      ((Command)SyncCommand).ChangeCanExecute();  // Important!
  }
  ```

### Dependency Injection

- All services are singletons/scoped, ViewModels are transient **by default in Core**
- Use centralized `AddSyncPlaylistServices()` extension method
- **Platform-specific override**: Both MAUI and Web apps override MainViewModel with Singleton to maintain state:
  ```csharp
  // In MauiProgram.cs
  builder.Services.AddSyncPlaylistServices();  // Registers Core MainViewModel as Transient
  builder.Services.AddSingleton<MainViewModel>();  // Overrides with MAUI-specific Singleton
  
  // In Web/Program.cs
  builder.Services.AddSyncPlaylistServices();  // Registers Core MainViewModel as Transient
  builder.Services.AddSingleton<Core.ViewModels.MainViewModel>();  // Overrides with Singleton
  ```
- **Why Singleton for Web?** Ensures authentication state persists across page navigation (Home → Sync)
- **Why Singleton for MAUI?** Single instance for the app's lifetime, consistent with typical MVVM pattern

## Building and Running

### Prerequisites

- .NET 10 SDK (10.0.102 or later)
- Xcode 26.2+ (for macOS/iOS builds)
- Android SDK (for Android builds)

### Build Commands

**Build MAUI App (macOS)**:
```bash
dotnet build SyncPlaylistApp/SyncPlaylistApp.csproj -f net10.0-maccatalyst -c Release
```

**Build Blazor Web App**:
```bash
dotnet build SyncPlaylistApp.Web/SyncPlaylistApp.Web.csproj -c Release
```

**Run Blazor Web App**:
```bash
dotnet run --project SyncPlaylistApp.Web/SyncPlaylistApp.Web.csproj
```

### Common Issues

1. **Missing Platform Entry Points**: .NET 10 MAUI requires explicit `Program.cs` files in `Platforms/` subdirectories
2. **Namespace Mismatches**: Ensure XAML uses `assembly=SyncPlaylistApp.Core` qualifier for Core types
3. **Command CanExecute**: Always call `ChangeCanExecute()` when properties change in MAUI ViewModels

## API Configuration

API credentials are stored in `SyncPlaylistApp.Core/Configuration/AppSettings.cs`. To use real services:

1. Register applications at:
   - Spotify: https://developer.spotify.com/dashboard
   - Apple Music: https://developer.apple.com/account
   - YouTube Music: https://console.cloud.google.com

2. Update `AppSettings.cs` with your credentials:
   ```csharp
   public static string SpotifyClientId { get; set; } = "YOUR_CLIENT_ID";
   ```

## Testing

Currently using stub data for rapid prototyping. Real API integration requires:

1. OAuth redirect URI handling (mobile/web specific)
2. Token storage (secure storage on mobile, session storage on web)
3. API client libraries (SpotifyAPI-NET, Apple MusicKit, YouTube Data API v3)

## Future Enhancements

- Real API integration with OAuth flows
- Persistent authentication token storage
- Bi-directional sync support
- Conflict resolution strategies
- Playlist creation/modification history
- Unit tests for Core library services
- Integration tests for sync operations

## Getting Help

- For .NET MAUI issues: https://github.com/dotnet/maui/issues
- For Blazor issues: https://github.com/dotnet/aspnetcore/issues
- For project-specific issues: See GitHub Issues in this repository
