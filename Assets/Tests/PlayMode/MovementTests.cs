using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Tests.Utils;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class MovementTests
    {
        const string MainScene = "Assets/Scenes/Main.unity";

        [UnitySetUp]
        public IEnumerator Setup()
        {
            Debug.Log($"Setup {nameof(MovementTests)}");
            if (!File.Exists(MainScene))
            {
                Assert.Inconclusive($"The path to '{nameof(MainScene)}' is incorrect.");
            }
            SceneManager.LoadScene(MainScene);
            yield return null;
            Debug.Log($"Setting Tutorial as done");
            PlayerData.instance.tutorialDone = true;
        }

        [UnityTest, Timeout(10 * 1000)]
        public IEnumerator LeftClicksDontGoOffScreen()
        {
            Debug.Log($"Starting Test {nameof(LeftClicksDontGoOffScreen)}");
            yield return null;
            var loadoutState = Object.FindFirstObjectByType<LoadoutState>();

            Assert.NotNull(loadoutState, "Cant find LoadoutState on Main scene.");

            loadoutState.StartGame();

            yield return null;

            var trackManager = Object.FindFirstObjectByType<TrackManager>();
            var desiredSegmentCount = (int)trackManager.ReflectionGetConstFieldValue("k_DesiredSegmentCount");
            var listOfSegments = trackManager.ReflectionGetFieldValue("m_Segments") as IList;

            Debug.Log("Waiting for segments to spawn");

            yield return new WaitUntil(() => listOfSegments != null && listOfSegments.Count >= desiredSegmentCount);

            Debug.Log("Skipping start animation");

            trackManager.ReflectionSetFieldValue("m_TimeToStart", -1f);

            var characterController = trackManager.characterController;

            var positionXLimit = trackManager.laneOffset * -1f;

            int presses = 5;

            do {
                Debug.Log($"Simulating press {presses}");

                characterController.ChangeLane(-1);

                yield return new WaitForSeconds(characterController.laneChangeSpeed * 0.01f);
                presses--;
            } while (presses > 0 );

            Assert.GreaterOrEqual(characterController.characterCollider.transform.position.x, positionXLimit, "Character moved farter from the Left lane.");
        }

    }
}