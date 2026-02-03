# Setup Guide for Sync Playlist App

## Prerequisites

### Development Environment

1. **Operating System**: Windows 10 version 1809 (build 17763) or later
2. **Visual Studio 2022** (version 17.8 or later) with the following workloads:
   - .NET Multi-platform App UI development
   - Universal Windows Platform development
   - .NET desktop development

### Installing Visual Studio Workloads

If you already have Visual Studio installed, you can add the required workloads:

1. Open Visual Studio Installer
2. Click "Modify" on your Visual Studio 2022 installation
3. Select the following workloads:
   - ✅ .NET Multi-platform App UI development
   - ✅ Universal Windows Platform development
4. Click "Modify" to install

Alternatively, install via command line:

```powershell
# Run as Administrator
& "C:\Program Files (x86)\Microsoft Visual Studio\Installer\vs_installer.exe" modify --installPath "C:\Program Files\Microsoft Visual Studio\2022\Community" --add Microsoft.VisualStudio.Workload.NetCrossPlat --add Microsoft.VisualStudio.Workload.Universal
```

### .NET SDK

The project requires .NET 10.0 SDK or later. Check your version:

```bash
dotnet --version
```

If you need to install or update, download from: https://dotnet.microsoft.com/download

### Xcode (macOS/iOS builds only)

For building macOS and iOS targets:

- **Xcode 26.2 or later** is required
- Ensure Xcode developer tools are properly configured:

  ```bash
  sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer
  sudo xcodebuild -license accept
  sudo xcodebuild -runFirstLaunch
  ```

### .NET MAUI Workloads

Install the required .NET MAUI workloads:

```bash
# Install all MAUI workloads
sudo dotnet workload install maui

# Or install specific platform workloads
sudo dotnet workload install maui-android maui-ios maui-maccatalyst
```

## API Credentials Setup

### 1. Spotify API Setup

1. Go to [Spotify Developer Dashboard](https://developer.spotify.com/dashboard)
2. Log in with your Spotify account
3. Click "Create an App"
4. Fill in:
   - App name: "Sync Playlist App"
   - App description: "Cross-platform playlist synchronization"
   - Redirect URI: `syncplaylistapp://callback`
5. After creation, note your:
   - **Client ID**
   - **Client Secret**
6. Update `SyncPlaylistApp/Configuration/AppSettings.cs`:

   ```csharp
   public static string SpotifyClientId { get; set; } = "your_actual_client_id";
   public static string SpotifyClientSecret { get; set; } = "your_actual_client_secret";
   ```

### 2. Apple Music API Setup

1. Enroll in [Apple Developer Program](https://developer.apple.com/programs/) (requires $99/year)
2. Go to [Apple Developer Portal](https://developer.apple.com/account/)
3. Navigate to Certificates, Identifiers & Profiles
4. Create a MusicKit Identifier:
   - Click "+" to create a new identifier
   - Select "Services IDs"
   - Register with bundle ID (e.g., `com.yourcompany.syncplaylistapp`)
5. Create a MusicKit Private Key:
   - Go to "Keys" section
   - Click "+" to create a key
   - Enable "MusicKit"
   - Download the `.p8` key file (you can only download it once!)
6. Note your:
   - **Team ID** (found in membership details)
   - **Key ID** (from the key you created)
   - **Private Key** (the `.p8` file content)
7. Generate a developer token (you'll need to create a JWT):
   - Use the private key, team ID, and key ID
   - Token expires after 6 months
8. Update `SyncPlaylistApp/Configuration/AppSettings.cs`:

   ```csharp
   public static string AppleMusicDeveloperToken { get; set; } = "your_jwt_token";
   public static string AppleMusicTeamId { get; set; } = "your_team_id";
   public static string AppleMusicKeyId { get; set; } = "your_key_id";
   ```

**Note**: For Apple Music, you'll need to implement JWT token generation. See Apple's [MusicKit documentation](https://developer.apple.com/documentation/applemusicapi/generating_developer_tokens).

### 3. YouTube Music API Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select an existing one
3. Enable APIs:
   - Click "Enable APIs and Services"
   - Search for "YouTube Data API v3"
   - Click "Enable"
4. Create OAuth 2.0 credentials:
   - Go to "Credentials" in the left menu
   - Click "Create Credentials" → "OAuth 2.0 Client ID"
   - If prompted, configure OAuth consent screen:
     - User Type: External
     - App name: "Sync Playlist App"
     - Add scopes: YouTube Data API v3 read/write
   - Application type: Desktop app
   - Name: "Sync Playlist App"
   - Add Authorized redirect URI: `syncplaylistapp://callback`
5. Note your:
   - **Client ID**
   - **Client Secret**
6. Update `SyncPlaylistApp/Configuration/AppSettings.cs`:
   ```csharp
   public static string YouTubeClientId { get; set; } = "your_actual_client_id";
   public static string YouTubeClientSecret { get; set; } = "your_actual_client_secret";
   ```

## Building the Application

### Using Visual Studio

1. Open `SyncPlaylistApp.sln` in Visual Studio 2022
2. Select the target platform:
   - For Windows: Select "Windows Machine" and "net10.0-windows10.0.19041.0"
3. Build the solution:
   - Menu: Build → Build Solution (Ctrl+Shift+B)
4. Run the application:
   - Press F5 to run with debugging
   - Or Ctrl+F5 to run without debugging

### Using Command Line

```powershell
# Navigate to the solution directory
cd path\to\sync-playlist-app

# Restore NuGet packages
dotnet restore SyncPlaylistApp.sln

# Build for Windows
dotnet build SyncPlaylistApp.sln -f net10.0-windows10.0.19041.0 -c Release

# Run the application
dotnet run --project SyncPlaylistApp\SyncPlaylistApp.csproj -f net10.0-windows10.0.19041.0
```

### Building for Other Platforms

**Android:**

```bash
dotnet build SyncPlaylistApp.sln -f net10.0-android -c Release
```

**iOS:** (requires Mac with Xcode 26.2+)

```bash
dotnet build SyncPlaylistApp.sln -f net10.0-ios -c Release
```

**macOS Catalyst:** (requires Mac with Xcode 26.2+)

```bash
dotnet build SyncPlaylistApp.sln -f net10.0-maccatalyst -c Release
```

## .NET 10 Platform-Specific Files

**.NET 10 MAUI requires explicit platform entry point files** (previously auto-generated in .NET 9).

The following platform files are included in the project:

**iOS & macOS Catalyst:**

- `Platforms/iOS/Program.cs` - iOS entry point
- `Platforms/iOS/AppDelegate.cs` - iOS app lifecycle
- `Platforms/MacCatalyst/Program.cs` - macOS entry point  
- `Platforms/MacCatalyst/AppDelegate.cs` - macOS app lifecycle

**Android:**

- `Platforms/Android/MainActivity.cs` - Android main activity
- `Platforms/Android/MainApplication.cs` - Android app lifecycle

**Windows:**

- `Platforms/Windows/App.xaml.cs` - Windows entry point

**Shared:**

- `App.xaml.cs` uses `CreateWindow()` pattern (replaces deprecated `MainPage` setter)

## Troubleshooting

### Build Errors

**"Workload 'maui' not found"**

```bash
sudo dotnet workload install maui
```

**"Windows SDK not found"**

- Install Windows 11 SDK via Visual Studio Installer or
- Download from: https://developer.microsoft.com/windows/downloads/windows-sdk/

**"Xcode version mismatch" (macOS/iOS builds)**

- Ensure Xcode 26.2+ is installed
- Update .NET workloads: `sudo dotnet workload update`
- Switch to full Xcode: `sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer`

**"Missing Main entry point" or "Program not found"**

- Verify platform-specific files exist in `Platforms/` folders
- Clean and rebuild: `dotnet clean && dotnet build`

**"SupportedOSPlatformVersion" error**

- .NET 10 requires:
  - iOS/macOS: 15.0+
  - Android: 21.0+
  - Windows: 10.0.17763.0+

### Runtime Errors

**Authentication Fails**

- Verify API credentials in `AppSettings.cs`
- Check redirect URIs match exactly in all API consoles
- Ensure URLs use the correct scheme (`syncplaylistapp://`)

**"OAuth redirect URI mismatch"**

- Double-check the redirect URI in each API console matches `syncplaylistapp://callback`

## Development Tips

### Hot Reload

MAUI supports hot reload for XAML and C# changes:

- Press Alt+F10 or click the Hot Reload button
- Changes appear without restarting the app

### Debugging

Set breakpoints in:

- Authentication services to debug OAuth flow
- Playlist services to debug API calls
- Sync service to debug the synchronization logic

### Testing Without API Credentials

The current implementation uses simulated authentication. You can test the UI flow without real credentials by:

1. Running the app
2. Clicking sign-in buttons (they'll use simulated tokens)
3. Testing the playlist selection and sync UI

To implement real authentication, replace the simulated auth in each `*AuthService.cs` file with actual OAuth calls.

## Next Steps

1. Test the basic UI and navigation
2. Implement real OAuth flows using `WebAuthenticator.AuthenticateAsync()`
3. Implement real API calls to fetch and sync playlists
4. Add token refresh logic
5. Implement secure credential storage
6. Add error handling and retry logic
7. Add unit tests

## Resources

- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)
- [Spotify Web API](https://developer.spotify.com/documentation/web-api/)
- [Apple Music API](https://developer.apple.com/documentation/applemusicapi)
- [YouTube Data API](https://developers.google.com/youtube/v3)
- [WinUI 3 Documentation](https://learn.microsoft.com/windows/apps/winui/)
