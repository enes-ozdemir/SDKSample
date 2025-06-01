using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace SampleSDK.NetworkOps
{
    public static class NetworkHelper
    {
        private const string API_URL = "https://exampleapi.rollic.gs/event";
        private static readonly Queue<EventPayload> EventQueue = new Queue<EventPayload>();
        private static CancellationTokenSource _cts = new CancellationTokenSource();
        private const int MaxQueueSize = 200;

        private static readonly string FailedPayloadsFilePath =
            Path.Combine(Application.persistentDataPath, "FailedPayloads.json");

        public static void Initialize()
        {
            ConnectivityChecker.Initialize();
            LoadFailedPayloads();
            ConnectivityChecker.OnConnectivityRestored += RetryFailedPayloads;
            Application.quitting += CleanupAndSaveState;
        }

        private static void CleanupAndSaveState()
        {
            ConnectivityChecker.OnConnectivityRestored -= RetryFailedPayloads;
            SaveFailedPayloadsToDisk();
            _cts?.Cancel();
        }

        public static void SendEventPayload(EventPayload payload)
        {
            if (!ConnectivityChecker.IsConnectionOnline())
            {
                Debug.LogWarning("No internet connection. Queueing payload.");
                SaveFailedPayload(payload);
                return;
            }

            _ = SendWithRetryAsync(payload);
        }

        private static async Task SendWithRetryAsync(EventPayload payload, int maxRetries = 3,
            int initialDelayMs = 1000)
        {
            var attempt = 0;
            var delay = initialDelayMs;

            while (attempt < maxRetries)
            {
                if (_cts.IsCancellationRequested)
                {
                    Debug.Log("SendWithRetryAsync cancelled.");
                    return;
                }

                attempt++;

                if (TrySerializePayload(payload, out var jsonPayload))
                {
                    if (await TrySendRequestAsync(jsonPayload))
                    {
                        Debug.Log($"API Response: {jsonPayload}");
                        return;
                    }
                }

                Debug.LogWarning($"API Error (Attempt {attempt}). Retrying in {delay} ms...");
                await Task.Delay(delay);
                delay *= 2;
            }

            Debug.LogError("All retry attempts failed. Saving payload for later retry.");
            SaveFailedPayload(payload);
        }

        private static bool TrySerializePayload(EventPayload payload, out string jsonPayload)
        {
            try
            {
                jsonPayload = JsonUtility.ToJson(payload);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to serialize payload: {ex}");
                jsonPayload = null;
                return false;
            }
        }

        private static async Task<bool> TrySendRequestAsync(string jsonPayload)
        {
            using var request = new UnityWebRequest(API_URL, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();
            while (!operation.isDone)
            {
                if (_cts.IsCancellationRequested)
                {
                    Debug.Log("TrySendRequestAsync cancelled.");
                    return false;
                }

                await Task.Yield();
            }

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"API Response: {request.downloadHandler.text}");
                return true;
            }

            Debug.LogWarning($"API Error: {request.error}");
            return false;
        }

        private static void SaveFailedPayload(EventPayload payload)
        {
            if (EventQueue.Count >= MaxQueueSize)
            {
                Debug.LogError($"Event queue size exceeded limit ({MaxQueueSize}). Dropping oldest payload.");
                EventQueue.Dequeue();
            }

            EventQueue.Enqueue(payload);
            SaveFailedPayloadsToDisk();
        }

        private static void RetryFailedPayloads()
        {
            if (!ConnectivityChecker.IsConnectionOnline())
            {
                Debug.LogWarning("Retry aborted: No internet connection.");
                return;
            }

            while (EventQueue.Count > 0)
            {
                var payload = EventQueue.Dequeue();
                Debug.Log($"Retrying payload: {payload.@event}");
                SendEventPayload(payload);
            }
        }

        private static void SaveFailedPayloadsToDisk()
        {
            try
            {
                var payloads = new List<EventPayload>(EventQueue);
                var json = JsonUtility.ToJson(new EventPayloadList { payloads = payloads });
                File.WriteAllText(FailedPayloadsFilePath, json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save payloads to disk: {ex}");
            }
        }

        private static void LoadFailedPayloads()
        {
            if (!File.Exists(FailedPayloadsFilePath)) return;

            var json = File.ReadAllText(FailedPayloadsFilePath);
            var payloadList = JsonUtility.FromJson<EventPayloadList>(json);
            if (payloadList == null) return;
            foreach (var payload in payloadList.payloads)
            {
                EventQueue.Enqueue(payload);
            }

            Debug.Log("Loaded failed payloads from file storage.");
            RetryFailedPayloads();
        }
    }
}