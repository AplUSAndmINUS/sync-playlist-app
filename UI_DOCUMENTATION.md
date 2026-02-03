# User Interface Documentation

## Main Page Layout

The Sync Playlist App features a modern, clean interface inspired by Tailwind CSS design principles.

### Color Scheme

- **Primary Blue**: #3B82F6 - Used for action buttons and interactive elements
- **Success Green**: #10B981 - Used for sync button and success indicators
- **Slate Grays**: #1E293B to #F8FAFC - Used for text and backgrounds
- **Service-Specific Colors**:
  - Spotify Green: #1DB954
  - Apple Music Red: #FA243C
  - YouTube Red: #FF0000

### UI Sections

#### 1. Header
- Large, bold title: "Sync Playlist App"
- Subtitle explaining the app's purpose
- Clean, centered layout

#### 2. Authentication Section
- Three sign-in buttons, one for each service:
  - "Sign In to Spotify" (green background)
  - "Sign In to Apple Music" (red background)
  - "Sign In to YouTube Music" (red background)
- Checkmark (✓) appears next to each successfully authenticated service
- Light gray background (#F8FAFC) with rounded corners

#### 3. Source Selection Section
- Three buttons to load playlists from each service
- ScrollView containing playlist list:
  - Each playlist card shows:
    - Playlist name (bold)
    - Description
    - Track count and source service
  - Cards have white background with light gray borders
  - Tappable for selection

#### 4. Destination Selection Section
- Checkboxes for each destination service:
  - Spotify checkbox (green)
  - Apple Music checkbox (red)
  - YouTube Music checkbox (red)
- Allows multiple selections
- Light gray background

#### 5. Sync Control
- Large green "Sync Playlist" button
- Full-width, prominent placement
- Disabled when no playlist or destination selected

#### 6. Status Display
- Status message box with light indigo background
- Shows:
  - Authentication results
  - Playlist loading status
  - Sync progress and results
  - Number of tracks synced/skipped
- Only visible when there's status to show

#### 7. Loading Indicator
- Circular activity indicator
- Blue color (#3B82F6)
- Appears during sync operations

## User Flow

### 1. Initial State
```
┌─────────────────────────────────┐
│   Sync Playlist App             │
│   Sign in to services to start  │
├─────────────────────────────────┤
│ [Sign In to Spotify]            │
│ [Sign In to Apple Music]        │
│ [Sign In to YouTube Music]      │
└─────────────────────────────────┘
```

### 2. After Authentication
```
┌─────────────────────────────────┐
│ [Sign In to Spotify]        ✓   │
│ [Sign In to Apple Music]    ✓   │
│ [Sign In to YouTube Music]      │
├─────────────────────────────────┤
│ Status: Spotify: Signed in      │
└─────────────────────────────────┘
```

### 3. Selecting Source Playlists
```
┌─────────────────────────────────┐
│ Select Source Service           │
│ [Spotify] [Apple] [YouTube]     │
├─────────────────────────────────┤
│ Available Playlists:            │
│ ┌─────────────────────────────┐ │
│ │ My Favorite Songs           │ │
│ │ A collection of favorites   │ │
│ │ 50 tracks • from Spotify    │ │
│ └─────────────────────────────┘ │
│ ┌─────────────────────────────┐ │
│ │ Workout Mix                 │ │
│ │ High energy tracks          │ │
│ │ 30 tracks • from Spotify    │ │
│ └─────────────────────────────┘ │
└─────────────────────────────────┘
```

### 4. Selecting Destinations and Syncing
```
┌─────────────────────────────────┐
│ Select Destination Services     │
│ ☑ Spotify                       │
│ ☑ Apple Music                   │
│ ☐ YouTube Music                 │
├─────────────────────────────────┤
│    [Sync Playlist]              │
├─────────────────────────────────┤
│ Status: Spotify: 48/50 tracks   │
│ synced, 2 skipped; Apple Music: │
│ 47/50 tracks synced, 3 skipped  │
└─────────────────────────────────┘
```

## Accessibility Features

- All interactive elements have minimum 44x44 touch targets
- High contrast text (dark on light backgrounds)
- Clear visual feedback for all states (normal, hover, pressed, disabled)
- Screen reader friendly labels
- Keyboard navigation support

## Responsive Design

The UI adapts to different window sizes:
- **Desktop (1200px+)**: Full layout with adequate spacing
- **Tablet (800px+)**: Slightly compressed but all elements visible
- **Mobile**: Vertical scrolling, stacked layout

## Tailwind CSS Inspiration

The design uses Tailwind-inspired principles:
- **Spacing**: Consistent 20px spacing between sections, 15px within sections
- **Typography**: 
  - Headers: 32px bold for title, 20px bold for section headers
  - Body: 14px regular, 12px for secondary text
- **Colors**: From Tailwind's Slate, Blue, Green, Red, and Indigo palettes
- **Borders**: Rounded corners (6-8px radius) throughout
- **Shadows**: Minimal use of shadows, relying on borders instead

## Platform-Specific Notes

### Windows (WinUI 3)
- Window size: 1200x800 by default
- Native Windows controls and animations
- Acrylic background effects where supported
- Windows 11 rounded corners on window chrome

### Mobile Platforms
- Full-screen layout
- Platform-native scroll physics
- Haptic feedback on button presses
- System keyboard handling
