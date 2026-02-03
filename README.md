# Sync Playlist App

A .NET 10 cross-platform application for syncing playlists across Spotify, Apple Music, and YouTube Music. Available as both a native MAUI app and a Blazor WebAssembly web application.

## Platform Support

### Native App (.NET MAUI)
- ✅ **Windows** - WinUI 3
- ✅ **macOS** - Mac Catalyst  
- ✅ **iOS** - Native iOS
- ✅ **Android** - Native Android

### Web App (Blazor WebAssembly)
- ✅ **Any modern browser** - Chrome, Edge, Firefox, Safari

## Overview

This application allows users to:

- Sign in to Spotify, Apple Music, and YouTube Music via their official OAuth/authentication APIs
- Select a playlist from any supported service as the source
- Sync that playlist to one or more destination services
- Automatically skip songs that aren't found in the destination service
- **NEW**: Access the full application from any browser without installation

## Getting Started

See [SETUP.md](SETUP.md) for detailed setup, configuration, and build instructions.

## Features

- **Multi-platform authentication** - OAuth support for Spotify, Apple Music, and YouTube Music
- **Cross-service playlist synchronization** - Sync playlists between any supported services
- **Smart track matching** - Uses ISRC codes for accurate cross-platform matching
- **Native UI** - WinUI 3 on Windows, native controls on other platforms
- **Blazor WebAssembly UI** - Run the full app in your browser via WebAssembly
- **Tailwind-inspired design** - Modern, clean interface with Bootstrap 5
- **.NET 10** - Latest .NET framework with improved performance
- **Shared Core Library** - 80%+ code reuse between native and web apps

## Technical Details

### Solution Structure

The solution consists of three projects:

1. **SyncPlaylistApp** - .NET MAUI native app for mobile and desktop
2. **SyncPlaylistApp.Core** - Shared business logic library (platform-agnostic)
3. **SyncPlaylistApp.Web** - Blazor WebAssembly web app

### Built With

- **.NET 10** - Latest framework version
- **.NET MAUI** - Cross-platform native UI framework
- **Blazor WebAssembly** - Browser-based UI with WebAssembly runtime
- **WinUI 3** - Modern Windows UI (when running on Windows)
- **Bootstrap 5** - Modern responsive web UI framework
- **MVVM Pattern** - Clean separation of concerns
- **Dependency Injection** - Centralized service registration
- **Dependency Injection** - Service-based architecture

### Minimum Requirements

- **iOS/macOS**: iOS 15.0+ / macOS 15.0+
- **Android**: API 21 (Android 5.0+)
- **Windows**: Windows 10 build 17763+
- **Development**: Xcode 26.2+ (for iOS/macOS builds)

### AI Considerations & Transparency

This app was created in conjunction with GitHub Copilot running Anthropic Claude Sonnet 4.5 runtime. 
Code was validated at time of compilation by @AplUSAndmINUS with both Core and Web versions of the app.
The app was created for knowledge purposes of understanding .NET Maui 10.0 for cross-compatibility and C# learning.

### License and PR Terms

This app remains open-source and can be reviewed at any point by those with suggested updates.
Any requested updates to the app must be done with a user story (GitHub Issue) and attached PR denoting the changes.
Any updates will be at the full discretion of @AplUSAndmINUS and @Copilot for these purposes through PR Review.
This app remains under a GNU 3.0 license.