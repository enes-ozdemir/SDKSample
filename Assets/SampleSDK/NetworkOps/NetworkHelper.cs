using UnityEngine;
using UnityEngine.Networking;
using SampleSDK.Core;

namespace SampleSDK.NetworkOps
{
    public class NetworkHelper
    {
        private const string API_URL = "https://webhook.site/b7c66668-f6fe-40d3-bbba-c8d8e4d5e42f";

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
                }
                request.Dispose();
            };
        }
    }
}