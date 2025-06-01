using UnityEngine;
using SampleSDK.NetworkOps;

namespace SampleSDK.Mediator
{
    public static class ModuleInitializer
    {
        public static void Initialize()
        {
            Debug.Log("SampleSDK Mediator initialized");
            Analytics.Analytics.Initialize();
            NetworkHelper.Initialize();
        }
    }
}