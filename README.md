# Sample SDK

This SDK for tracking user sessions and events with offline support and data persistence.

## Installation
1. Import the `SampleSDK.unitypackage` into your Unity project
2. Call `SDKCore.Initialize();` to initialize the SDK.

## Usage
```csharp
// Initialize the SDK (typically in Awake or Start)
SampleSDK.Initialize();

// Track custom events
Analytics.TrackEvent("button_click");
Analytics.TrackEvent("level_complete");
Analytics.TrackEvent("purchase_made");
```

## Implementation Example
```csharp
public class GameManager : MonoBehaviour
{
    void Awake()
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