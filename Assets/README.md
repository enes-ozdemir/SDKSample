# Sample SDK

A lightweight Unity SDK for tracking user sessions and events with robust offline support and data persistence.

## Features
- Session time tracking
- Event tracking with custom event names
- Automatic data synchronization with API
- Offline support with data persistence
- Error handling and retry mechanisms

## Installation
1. Import the `SampleSDK.unitypackage` into your Unity project
2. The SDK will be automatically initialized when your game starts

## Usage
```csharp
// Initialize the SDK (typically in Awake or Start)
SampleSDK.Initialize();

// Track custom events
SampleSDK.TrackEvent("button_click");
SampleSDK.TrackEvent("level_complete");
SampleSDK.TrackEvent("purchase_made");
```

## API Integration
The SDK automatically sends data to: `https://exampleapi.rollic.gs/event`

Data is sent in the following JSON format:
```json
{
    "event": "your_event_name",
    "session_time": 1231432524
}
```

## Offline Support
- The SDK automatically handles offline scenarios
- Events are stored locally when there's no internet connection
- Data is synchronized when connection is restored
- No data loss in case of API failures

## Requirements
- Unity 2020.3 or later
- .NET Standard 2.0

## Implementation Example
```csharp
public class GameManager : MonoBehaviour
{
    void Awake()
    {
        // Initialize the SDK
        SampleSDK.Initialize();
    }

    public void OnButtonClick()
    {
        // Track button click event
        SampleSDK.TrackEvent("button_click");
    }
}
```

## Documentation
For detailed documentation, please refer to the [Documentation](./Documentation) folder.

## Support
If you encounter any issues or have questions, please:
1. Check the [Documentation](./Documentation) first
2. Open an issue in the repository
3. Contact support at [support@example.com](mailto:support@example.com)

## License
This SDK is licensed under the MIT License. See the [LICENSE](./LICENSE) file for details.

## Contributing
We welcome contributions! Please see our [Contributing Guidelines](./CONTRIBUTING.md) for more information. 