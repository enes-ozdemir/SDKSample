using System;
using UnityEngine;

namespace SampleSDK.Core
{
    public static class SDKCore
    {
        public static bool IsInitialized { get; private set; }

        /// <summary>
        /// Event that is triggered when the SDK is initialized.
        /// </summary>
        public static event Action OnInitialized;

        /// <summary>
        /// Initializes the SDK.
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