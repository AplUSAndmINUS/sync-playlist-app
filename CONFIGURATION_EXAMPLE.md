# Example App Settings Configuration

This file shows example values for configuring the Sync Playlist App.
Copy these values to `SyncPlaylistApp/Configuration/AppSettings.cs` after obtaining your API credentials.

## Spotify Configuration

```csharp
public static string SpotifyClientId { get; set; } = "1234567890abcdef1234567890abcdef";
public static string SpotifyClientSecret { get; set; } = "abcdef1234567890abcdef1234567890";
public static string SpotifyRedirectUri { get; set; } = "syncplaylistapp://callback";
```

**Where to get these:**
- Go to https://developer.spotify.com/dashboard
- Create an app
- Copy Client ID and Client Secret
- Add `syncplaylistapp://callback` to Redirect URIs

## Apple Music Configuration

```csharp
public static string AppleMusicDeveloperToken { get; set; } = "eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IkFCQ0RFRjEyMzQifQ...";
public static string AppleMusicTeamId { get; set; } = "ABCD123456";
public static string AppleMusicKeyId { get; set; } = "ABCDEFGHIJ";
```

**Where to get these:**
- Enroll in Apple Developer Program ($99/year)
- Go to https://developer.apple.com/account/
- Create a MusicKit identifier
- Create a MusicKit key (.p8 file)
- Generate JWT token using team ID, key ID, and private key
- Token format: https://developer.apple.com/documentation/applemusicapi/generating_developer_tokens

## YouTube Music Configuration

```csharp
public static string YouTubeClientId { get; set; } = "123456789012-abcdefghijklmnopqrstuvwxyz123456.apps.googleusercontent.com";
public static string YouTubeClientSecret { get; set; } = "GOCSPX-abcdefghijklmnopqrstuvwx";
public static string YouTubeRedirectUri { get; set; } = "syncplaylistapp://callback";
```

**Where to get these:**
- Go to https://console.cloud.google.com/
- Create a project
- Enable YouTube Data API v3
- Create OAuth 2.0 credentials (Desktop app)
- Add `syncplaylistapp://callback` to Authorized redirect URIs

## Security Notes

⚠️ **IMPORTANT**: Never commit API credentials to source control!

- Add `AppSettings.cs` with real credentials to `.gitignore`
- Consider using environment variables or Azure Key Vault for production
- Rotate credentials if they are accidentally exposed
- Use .NET User Secrets for development: https://learn.microsoft.com/aspnet/core/security/app-secrets

## Example .gitignore Entry

```gitignore
# API Configuration (if storing in a separate file)
**/Configuration/AppSettings.Local.cs
**/appsettings.Local.json
```

## Alternative: Using User Secrets (Recommended for Development)

Instead of hardcoding in AppSettings.cs, use .NET User Secrets:

```bash
# Initialize user secrets
cd SyncPlaylistApp
dotnet user-secrets init

# Set secrets
dotnet user-secrets set "Spotify:ClientId" "your-client-id"
dotnet user-secrets set "Spotify:ClientSecret" "your-client-secret"
dotnet user-secrets set "AppleMusic:DeveloperToken" "your-token"
dotnet user-secrets set "YouTube:ClientId" "your-client-id"
dotnet user-secrets set "YouTube:ClientSecret" "your-client-secret"
```

Then access in code:
```csharp
// In MauiProgram.cs or App.xaml.cs
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<App>()
    .Build();

var spotifyClientId = configuration["Spotify:ClientId"];
```
