# Architecture Documentation

## System Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                         Sync Playlist App                       │
│                        (.NET MAUI / WinUI 3)                    │
└─────────────────────────────────────────────────────────────────┘
                                  │
                ┌─────────────────┼─────────────────┐
                │                 │                 │
                ▼                 ▼                 ▼
    ┌──────────────────┐ ┌──────────────┐ ┌──────────────────┐
    │   Views (XAML)   │ │  ViewModels  │ │   Services       │
    │                  │ │              │ │                  │
    │ - MainPage.xaml  │ │ - MainVM     │ │ - Auth Services  │
    │                  │ │   • Commands │ │ - Playlist Svc   │
    │ Tailwind-styled  │ │   • Props    │ │ - Sync Service   │
    └──────────────────┘ └──────────────┘ └──────────────────┘
                                  │                 │
                                  └────────┬────────┘
                                           │
                          ┌────────────────┼────────────────┐
                          │                │                │
                          ▼                ▼                ▼
                  ┌──────────────┐ ┌──────────────┐ ┌──────────────┐
                  │   Spotify    │ │ Apple Music  │ │   YouTube    │
                  │     API      │ │     API      │ │  Music API   │
                  └──────────────┘ └──────────────┘ └──────────────┘
```

## Layer Architecture

### Presentation Layer (Views)
**Files**: `Views/MainPage.xaml`, `Views/MainPage.xaml.cs`

- XAML-based UI using MAUI controls
- Tailwind CSS-inspired styling via XAML resources
- Data binding to ViewModels
- Event handlers for user interactions

**Responsibilities:**
- Display UI components
- Handle user input events
- Bind to ViewModel properties
- Navigate between pages (if multiple)

### ViewModel Layer
**Files**: `ViewModels/MainViewModel.cs`

- Implements INotifyPropertyChanged for data binding
- Exposes ICommand for user actions
- Manages UI state
- Orchestrates service calls

**Responsibilities:**
- Handle user commands (sign in, load playlists, sync)
- Maintain UI state (authentication status, selected items)
- Call service methods
- Update UI through property changes

### Service Layer

#### Authentication Services
**Files**: 
- `Services/IMusicServiceAuth.cs` (interface)
- `Services/SpotifyAuthService.cs`
- `Services/AppleMusicAuthService.cs`
- `Services/YouTubeMusicAuthService.cs`

**Responsibilities:**
- Implement OAuth 2.0 flow (or service-specific auth)
- Manage access tokens and refresh tokens
- Handle token expiration
- Store authentication state

#### Playlist Services
**Files**:
- `Services/IPlaylistService.cs` (interface)
- `Services/SpotifyPlaylistService.cs`
- `Services/AppleMusicPlaylistService.cs`
- `Services/YouTubeMusicPlaylistService.cs`

**Responsibilities:**
- Fetch playlists from each service
- Get playlist details with tracks
- Search for tracks
- Create playlists
- Add tracks to playlists

#### Sync Service
**Files**: `Services/PlaylistSyncService.cs`

**Responsibilities:**
- Orchestrate playlist synchronization
- Match tracks across services
- Handle missing tracks (skip them)
- Create destination playlists
- Report sync results

### Model Layer
**Files**: `Models/MusicModels.cs`

**Data Models:**
- `Playlist`: Represents a music playlist
- `Track`: Represents a song/track
- `ServiceAccount`: Holds authentication information
- `SyncResult`: Contains sync operation results
- `MusicService`: Enum for service types

### Configuration Layer
**Files**: `Configuration/AppSettings.cs`

**Responsibilities:**
- Store API credentials
- Define API endpoints
- OAuth scopes configuration

## Data Flow

### 1. Authentication Flow
```
User clicks "Sign In"
    │
    ▼
MainViewModel.SignInSpotify()
    │
    ▼
SpotifyAuthService.AuthenticateAsync()
    │
    ├─► Opens browser with OAuth URL
    ├─► User authorizes app
    ├─► Receives callback with auth code
    ├─► Exchanges code for access token
    └─► Stores ServiceAccount with token
    │
    ▼
ViewModel updates IsSpotifyAuthenticated = true
    │
    ▼
UI shows checkmark
```

### 2. Load Playlists Flow
```
User clicks "Spotify" button
    │
    ▼
MainViewModel.LoadPlaylists(Spotify)
    │
    ▼
SpotifyPlaylistService.GetPlaylistsAsync()
    │
    ├─► Checks authentication
    ├─► Makes API call to Spotify
    └─► Returns List<Playlist>
    │
    ▼
ViewModel adds to SourcePlaylists collection
    │
    ▼
UI displays playlist cards
```

### 3. Sync Playlist Flow
```
User selects playlist and destinations, clicks "Sync"
    │
    ▼
MainViewModel.SyncPlaylist()
    │
    ├─► Gets full playlist with tracks
    │   (calls SourceService.GetPlaylistDetailsAsync())
    │
    ▼
PlaylistSyncService.SyncToMultipleServicesAsync()
    │
    ├─► For each destination service:
    │   │
    │   ├─► Create new playlist
    │   │   (DestService.CreatePlaylistAsync())
    │   │
    │   ├─► For each track in source:
    │   │   │
    │   │   ├─► Search track in destination
    │   │   │   (DestService.SearchTrackAsync())
    │   │   │
    │   │   ├─► If found: Add to list
    │   │   └─► If not found: Skip & log
    │   │
    │   └─► Add all found tracks to playlist
    │       (DestService.AddTracksToPlaylistAsync())
    │
    ▼
Returns List<SyncResult> with counts
    │
    ▼
ViewModel displays results in status
```

## Track Matching Strategy

```
┌─────────────────────────────┐
│  Source Track               │
│  - Name: "Bohemian Rhapsody"│
│  - Artist: "Queen"          │
│  - ISRC: "GBUM71029604"     │
└─────────────────────────────┘
              │
              ▼
┌─────────────────────────────┐
│  Search Strategy            │
├─────────────────────────────┤
│  1. Search by ISRC code     │
│     (most accurate)         │
│                             │
│  2. If not found, search by:│
│     Track Name + Artist     │
│                             │
│  3. If still not found:     │
│     Return null (skip)      │
└─────────────────────────────┘
              │
              ▼
┌─────────────────────────────┐
│  Result                     │
├─────────────────────────────┤
│  • Found: Add to playlist   │
│  • Not found: Skip track    │
│    and increment skip count │
└─────────────────────────────┘
```

## Extension Points

The architecture is designed for extensibility:

### Adding New Services
1. Implement `IMusicServiceAuth` interface
2. Implement `IPlaylistService` interface
3. Add service to `PlaylistSyncService` constructor
4. Add UI elements in MainPage.xaml
5. Update ViewModel with new commands

### Adding Features
- **Playlist Scheduling**: Add background task service
- **Conflict Resolution**: Extend SyncService with merge strategies
- **Offline Mode**: Add local database layer
- **Analytics**: Add tracking service
- **Notifications**: Implement notification service

## Platform-Specific Components

### Windows (WinUI 3)
**Files**: `Platforms/Windows/App.xaml`, `Platforms/Windows/App.xaml.cs`

- Window initialization
- Window size configuration
- Platform-specific services

### Android
**Directory**: `Platforms/Android/`
- AndroidManifest.xml configuration
- Platform-specific renderers

### iOS
**Directory**: `Platforms/iOS/`
- Info.plist configuration
- Platform-specific renderers

## Security Considerations

1. **Token Storage**: Currently in-memory (temporary)
   - **Production**: Use platform-specific secure storage
     - Windows: Credential Manager
     - Android: Keystore
     - iOS: Keychain

2. **API Credentials**: Currently in AppSettings.cs
   - **Production**: Use Azure Key Vault or environment variables

3. **OAuth State**: Not implemented
   - **Production**: Generate and validate state parameter

4. **HTTPS**: All API calls use HTTPS
   - Enforced by service providers

## Performance Considerations

1. **Async/Await**: All I/O operations are asynchronous
2. **Pagination**: Services should support pagination for large playlists
3. **Rate Limiting**: Implement retry logic with exponential backoff
4. **Caching**: Consider caching playlist data
5. **Batch Operations**: Add tracks in batches where supported by APIs

## Testing Strategy

### Unit Tests (To Be Implemented)
- Test ViewModels in isolation
- Mock service interfaces
- Test sync logic edge cases

### Integration Tests (To Be Implemented)
- Test actual API calls (with test accounts)
- Validate OAuth flows
- Test track matching accuracy

### UI Tests (To Be Implemented)
- Test user workflows
- Validate data binding
- Test error scenarios
