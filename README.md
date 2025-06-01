# Sample SDK

This SDK for tracking user sessions and events with offline support and data persistence.

## Installation
1. Import the `SampleSDK.unitypackage` into your Unity project
2. Call `SDKCore.Initialize();` to initialize the SDK.

## Features
- Tracks user sessions and custom events.
- Supports offline mode with data persistence.

## Usage
```csharp
// Initialize the SDK (typically in Awake or Start)
SDKCore.Initialize();

// Track custom events
Analytics.TrackEvent("button_click");
Analytics.TrackEvent("level_complete");
Analytics.TrackEvent("purchase_made");
```

## Implementation Example
```csharp
public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        // Initialize the SDK
        SDKCore.Initialize();
    }

    public void OnButtonClick()
    {
        // Track button click event
        Analytics.TrackEvent("OnGameButtonClick")
    }
}
```
