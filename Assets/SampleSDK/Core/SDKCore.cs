using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace SampleSDK.Core
{
   public class SampleSDK
   {
      public static bool IsInitialized { get; private set; }
      
      public static event Action OnInitialized;
      
      /// <summary>
      /// Initializes the SampleSDK.
      /// </summary>
      public static void Initialize()
      {
         if (IsInitialized)
         {
            Debug.LogWarning("SampleSDK is already initialized.");
            return;
         }
         
         IsInitialized = true;
         OnInitialized?.Invoke();
         Debug.Log("SampleSDK Initialized");
      }
   
     
   }
}
