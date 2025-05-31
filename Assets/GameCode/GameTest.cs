using System;
using UnityEngine;
using UnityEngine.UI;

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
            SampleSDK.Core.SampleSDK.Initialize();
            GameButtonClick += OnGameButtonClick;
        }

        private void OnGameButtonClick()
        {
            SampleSDK.Analytics.Analytics.TrackEvent("my button click");
        }
    }
}