using System;

namespace SampleSDK.NetworkOps
{
    [Serializable]
    internal class EventPayload
    {
        public string @event;
        public double sessionTime;
    }
}