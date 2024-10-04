using System;
using System.Collections;
using System.IO;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class GameplayTests
    {
        const string MainScene = "Assets/Scenes/Main.unity";

        LoadoutState _loadoutState;
        TrackManager _trackManager;

        [SetUp]
        public void Setup()
        {
            Debug.Log($"Setup {nameof(GameplayTests)}");
            if (!File.Exists(MainScene))
            {
                Assert.Inconclusive($"The path to '{nameof(MainScene)}' is incorrect.");
            }
        }

        [UnityTest, Order(0), Timeout(10 * 1000)]
        public IEnumerator LeftClicksDontGoOffScreen()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(LeftClicksDontGoOffScreen)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var positionXLimit = _trackManager.laneOffset * -1f;
            var characterController = _trackManager.characterController;
            int presses = 5;
            do {
                Debug.Log($"Simulating press {presses}");
                characterController.ChangeLane(-1);
                yield return null;
                yield return new WaitForSeconds(characterController.laneChangeSpeed * 0.01f);
                presses--;
            } while (presses > 0 );
            Assert.GreaterOrEqual(characterController.characterCollider.transform.position.x, positionXLimit, "Character moved farter from the Left lane.");
        }

        [UnityTest, Order(1)]
        public IEnumerator FishCollectionGivingPoints()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(FishCollectionGivingPoints)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var initialScore = _trackManager.score;
            Debug.Log("Spawning Fishes");
            int spawns = 5;
            do {
                yield return _trackManager.SpawnCoinAndPowerup(_trackManager.currentSegment);
                spawns--;
            } while (spawns > 0);
            Debug.Log("Waiting for Player to collect a few fishes");
            yield return new WaitForSeconds(2f);
            Assert.Greater(_trackManager.score, initialScore);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_loadoutState);
            Object.Destroy(_trackManager);
        }

        IEnumerator MainGameSceneSetup()
        {
            Debug.Log($"Loading scene: {MainScene}");
            SceneManager.LoadScene(MainScene);
            yield return null;
            Debug.Log($"Setting Tutorial as done");
            PlayerData.instance.tutorialDone = true;
        }

        IEnumerator WaitUntilGameStarts()
        {
            yield return null;
            _loadoutState = Object.FindFirstObjectByType<LoadoutState>();
            Assert.NotNull(_loadoutState, "Cant find LoadoutState in Main scene.");
            _loadoutState.StartGame();
            yield return null;
            _trackManager = Object.FindFirstObjectByType<TrackManager>();
            Assert.NotNull(_trackManager);
            var desiredSegmentCount = (int)_trackManager.ReflectionGetConstFieldValue("k_DesiredSegmentCount");
            Assert.NotZero(desiredSegmentCount);
            var listOfSegments = _trackManager.ReflectionGetFieldValue("m_Segments") as IList;
            Debug.Log("Waiting for segments to spawn");
            yield return new WaitUntil(() => listOfSegments != null && listOfSegments.Count >= desiredSegmentCount);
            Debug.Log("Skipping start animation");
            _trackManager.ReflectionSetFieldValue("m_TimeToStart", -1f);
        }

    }
}