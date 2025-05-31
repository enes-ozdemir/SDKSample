using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SampleSDK.NetworkOps
{
    public static class NetworkHelper
    {
        private const string API_URL = "https://webhook.site/b7c66668-f6fe-40d3-bbba-c8d8e4d5e42f";
        private static readonly Queue<EventPayload> EventQueue = new Queue<EventPayload>();
        
        static NetworkHelper()
        {
            ConnectivityChecker.OnConnectivityRestored += RetryFailedPayloads;
        }
        
        public static void SendEventPayload(EventPayload payload)
        {
            var jsonPayload = JsonUtility.ToJson(payload);
            var request = new UnityWebRequest(API_URL, "POST")
            {
                uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonPayload)),
                downloadHandler = new DownloadHandlerBuffer()
            };
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            operation.completed += _ =>
            {
                if (request.result == UnityWebRequest.Result.Success)
                {
                    Debug.Log($"API Response: {request.downloadHandler.text}");
                }
                else
                {
                    Debug.LogError($"API Error: {request.error}");
                    SaveFailedPayload(payload);
                }
                request.Dispose();
            };
        }

        private static void SaveFailedPayload(EventPayload payload)
        {
            Debug.LogWarning("Saving failed payload for retry.");
            EventQueue.Enqueue(payload);
        }

        private static void RetryFailedPayloads()
        {
            while (EventQueue.Count > 0)
            {
                var payload = EventQueue.Dequeue();
                Debug.Log($"Retrying payload: {payload.@event}");
                SendEventPayload(payload);
            }
        }
    }
}