using SampleSDK.NetworkOps;
using UnityEngine;

namespace SampleSDK.Analytics
{
    public static class Analytics
    {
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
    }
}