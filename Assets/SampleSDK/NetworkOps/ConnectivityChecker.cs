using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace SampleSDK.NetworkOps
{
    public static class ConnectivityChecker
    {
        internal static event Action OnConnectivityRestored;

        private static CancellationTokenSource _cancellationTokenSource;
        private static bool _connectivityStarted = false;
        private static bool _isOnline = false;

        public static bool IsOnline => _isOnline;

        static ConnectivityChecker()
        {
            Debug.Log("ConnectivityChecker static constructor called.");
            Core.SampleSDK.OnInitialized += OnSDKInitialized;
        }

        public static void Initialize()
        {
            if (!_connectivityStarted)
            {
                Debug.Log("Initializing ConnectivityChecker...");
                StartCheckingConnectivity();
            }
        }

        private static void OnSDKInitialized()
        {
            Initialize();
        }

        internal static void StartCheckingConnectivity(int intervalMilliseconds = 5000)
        {
            StopCheckingConnectivity(); // Ensure no duplicate tasks are running
            _cancellationTokenSource = new CancellationTokenSource();
            _ = CheckConnectivityAsync(intervalMilliseconds, _cancellationTokenSource.Token);
        }

        public static void StopCheckingConnectivity()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _connectivityStarted = false;
        }

        private static async Task CheckConnectivityAsync(int intervalMilliseconds, CancellationToken cancellationToken)
        {
            Debug.Log("Starting connectivity check task...");
            var previousOnlineStatus = Application.internetReachability != NetworkReachability.NotReachable;
            _isOnline = previousOnlineStatus;

            while (!cancellationToken.IsCancellationRequested)
            {
                bool isCurrentlyOnline = (Application.internetReachability != NetworkReachability.NotReachable);
                _isOnline = isCurrentlyOnline;

                if (!previousOnlineStatus && isCurrentlyOnline)
                {
                    Debug.Log("Internet connectivity restored.");
                    OnConnectivityRestored?.Invoke();
                }
                else
                {
                    Debug.Log($"Internet connectivity status: {(isCurrentlyOnline ? "Online" : "Offline")}");
                }

                previousOnlineStatus = isCurrentlyOnline;

                try
                {
                    await Task.Delay(intervalMilliseconds, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    Debug.Log("Connectivity check task was cancelled.");
                    break;
                }
            }
        }
    }
}
