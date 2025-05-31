using SampleSDK.NetworkOps;
using UnityEngine;

namespace SampleSDK.Core
{
   public class SampleSDK
   {
      public static bool IsInitialized { get; private set; }
      
      public static void Initialize()
      {
         if (IsInitialized)
         {
            Debug.LogWarning("SampleSDK is already initialized.");
            return;
         }
         
         IsInitialized = true;
         Debug.Log("SampleSDK Initialized");
      }
   
      public static void TrackEvent(string eventName)
      {
         if(IsInitialized == false)
         {
            Debug.LogError("SampleSDK is not initialized. Please call Initialize() before tracking events.");
            return;
         }
         
         Debug.Log($"Event Tracked: {eventName}");
         var payload = new EventPayload
         {
            @event = eventName,
            sessionTime = Time.realtimeSinceStartupAsDouble
         };
         
         //apı call
         Debug.Log($"Payload: Event = {payload.@event}, Session Time = {payload.sessionTime}");
         NetworkHelper.SendEventPayload(payload);
      }
   }
}
