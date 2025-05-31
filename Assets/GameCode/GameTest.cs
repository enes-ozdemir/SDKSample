using System;
using UnityEngine;
using UnityEngine.UI;
using SampleSDK.Analytics;
using SampleSDK.Core;

namespace GameCode
{
    public class GameTest : MonoBehaviour
    {
        [SerializeField] private Button gameButton;

        private event Action GameButtonClick;

        private void OnValidate()
        {
            if (gameButton != null)
                gameButton.onClick.AddListener(() => GameButtonClick?.Invoke());
            else
                Debug.LogWarning("GameButton is not assigned  or found in the GameObject.");
        }

        private void Awake()
        {
            SDKCore.Initialize();
            GameButtonClick += OnGameButtonClick;
        }

        private void OnGameButtonClick()
        {
            Analytics.TrackEvent("my button click");
        }
    }
}