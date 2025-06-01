using SampleSDK.NetworkOps;
using UnityEngine;

namespace SampleSDK.Analytics
{
    public static class Analytics
    {
        /// <summary>
        /// Tracks an event by sending its payload to the network.
        /// </summary>
        /// <param name="eventName">The name of the event to track.</param>
        public static void TrackEvent(string eventName)
        {
            var payload = new EventPayload
            {
                @event = eventName,
                sessionTime = Time.realtimeSinceStartupAsDouble
            };

            Debug.Log($"Payload: Event = {payload.@event}, Session Time = {payload.sessionTime}");
            NetworkHelper.SendEventPayload(payload);
        }

        public static void Initialize()
        {
            TrackEvent("SessionStart");
        }
    }
}