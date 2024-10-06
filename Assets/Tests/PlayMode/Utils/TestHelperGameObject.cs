using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tests.PlayMode.Utils
{
    public class TestHelperGameObject : MonoBehaviour
    {
        public bool SupressApplicationPausePrevention;

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void LateUpdate()
        {
            CheckToResumeGame();
        }

        public new void StartCoroutine(IEnumerator routine)
        {
            base.StartCoroutine(routine);
        }

        void CheckToResumeGame()
        {
            if (SupressApplicationPausePrevention) return;
            var gameState = Object.FindFirstObjectByType<GameState>(FindObjectsInactive.Exclude);
            if (gameState == null) return;
            // The game uses the AudioListener pause state has one of the conditions to check if its paused.
            if (!AudioListener.pause) return;
            Debug.Log("Forcing game Resume");
            gameState.Resume();
        }

    }
}