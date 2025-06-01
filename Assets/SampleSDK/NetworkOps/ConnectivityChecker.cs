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

        internal static void Initialize()
        {
            if (!_connectivityStarted)
            {
                Debug.Log("Initializing ConnectivityChecker...");
                StartCheckingConnectivity();
                Application.quitting += StopCheckingConnectivity;
            }
        }

        private static void StartCheckingConnectivity(int intervalMilliseconds = 5000)
        {
            StopCheckingConnectivity();
            _cancellationTokenSource = new CancellationTokenSource();
            _ = CheckConnectivityAsync(intervalMilliseconds, _cancellationTokenSource.Token);
        }

        private static void StopCheckingConnectivity()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource = null;
            _connectivityStarted = false;
        }

        public static bool IsConnectionOnline()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        private static async Task CheckConnectivityAsync(int intervalMilliseconds, CancellationToken cancellationToken)
        {
            Debug.Log("Starting connectivity check task...");
            var previousOnlineStatus = Application.internetReachability != NetworkReachability.NotReachable;

            while (!cancellationToken.IsCancellationRequested)
            {
                var isCurrentlyOnline = IsConnectionOnline();

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