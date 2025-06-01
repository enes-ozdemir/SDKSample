using UnityEngine;
using SampleSDK.NetworkOps;
using SampleSDK.Core;

namespace SampleSDK.Mediator
{
    public static class ModuleInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            SDKCore.OnInitialized += () =>
            {
                Debug.Log("Sample SDK initialized successfully. Now initializing modules...");
                Analytics.Analytics.Initialize();
                NetworkHelper.Initialize();
            };
        }
    }
}