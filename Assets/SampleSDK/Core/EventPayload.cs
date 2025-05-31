using System;

namespace SampleSDK.Core
{
    //Make this internal to prevent direct access from outside the SDK
    [Serializable]
    public class EventPayload
    {
        public string @event;
        public double sessionTime;
    }
}