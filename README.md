# Sync Playlist App

A .NET 10 MAUI cross-platform application for syncing playlists across Spotify, Apple Music, and YouTube Music.

## Platform Support

- ✅ **Windows** - WinUI 3
- ✅ **macOS** - Mac Catalyst  
- ✅ **iOS** - Native iOS
- ✅ **Android** - Native Android

## Overview

This application allows users to:

- Sign in to Spotify, Apple Music, and YouTube Music via their official OAuth/authentication APIs
- Select a playlist from any supported service as the source
- Sync that playlist to one or more destination services
- Automatically skip songs that aren't found in the destination service

## Getting Started

See [SyncPlaylistApp/README.md](SyncPlaylistApp/README.md) for detailed setup, configuration, and build instructions.

## Features

- **Multi-platform authentication** - OAuth support for Spotify, Apple Music, and YouTube Music
- **Cross-service playlist synchronization** - Sync playlists between any supported services
- **Smart track matching** - Uses ISRC codes for accurate cross-platform matching
- **Native UI** - WinUI 3 on Windows, native controls on other platforms
- **Tailwind-inspired design** - Modern, clean interface
- **.NET 10** - Latest .NET framework with improved performance

## Technical Details

### Built With

- **.NET 10** - Latest framework version
- **.NET MAUI** - Cross-platform UI framework
- **WinUI 3** - Modern Windows UI (when running on Windows)
- **MVVM Pattern** - Clean separation of concerns
- **Dependency Injection** - Service-based architecture

### Minimum Requirements

- **iOS/macOS**: iOS 15.0+ / macOS 15.0+
- **Android**: API 21 (Android 5.0+)
- **Windows**: Windows 10 build 17763+
- **Development**: Xcode 26.2+ (for iOS/macOS builds)
