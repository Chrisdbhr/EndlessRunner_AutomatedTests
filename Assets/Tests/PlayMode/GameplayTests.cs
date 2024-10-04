using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class GameplayTests
    {
        #region Properties and Fields

        const string MainScene = "Assets/Scenes/Main.unity";

        LoadoutState _loadoutState;
        TrackManager _trackManager;

        #endregion Properties and Fields

        #region Setup and Teardown

        [SetUp]
        public void Setup()
        {
            Debug.Log($"Setup {nameof(GameplayTests)}");
            if (!File.Exists(MainScene))
            {
                Assert.Inconclusive($"The path to '{nameof(MainScene)}' is incorrect.");
            }
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(_loadoutState);
            Object.Destroy(_trackManager);
        }

        #endregion Setup and Teardown

        #region Tests

        [UnityTest, Order(0), Timeout(10 * 1000)]
        public IEnumerator LeftClicksDontGoOffScreen()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(LeftClicksDontGoOffScreen)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var positionXLimit = _trackManager.laneOffset * -1f;
            Assert.NotNull(_trackManager.characterController);
            var characterController = _trackManager.characterController;
            int presses = 5;
            do {
                Debug.Log($"Simulating press {presses}");

                // Directly accessing the ChangeLane() since the current old input system that is implemented in the game
                // does't support InputSimulation like the New Unity Input System does.
                // Current code is checked that "ChangeLane" is only called by Input methods.
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
            int safeSegmentOverride = 10;
            Debug.Log($"Overriding safe segment value to {safeSegmentOverride}");
            _trackManager.ReflectionSetFieldValue("m_SafeSegementLeft", safeSegmentOverride);
            var initialScore = _trackManager.score;
            Debug.Log("Spawning Fishes");
            int spawns = 5;
            do {
                yield return _trackManager.SpawnCoinAndPowerup(_trackManager.currentSegment);
                spawns--;
            } while (spawns > 0);
            Debug.Log("Waiting for Player to collect a few fishes");
            yield return new WaitForSeconds(1f);
            Assert.Greater(_trackManager.score, initialScore);
        }

        [UnityTest, Order(2)]
        public IEnumerator HitSomethingAndDie()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(HitSomethingAndDie)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            Assert.NotNull(_trackManager.characterController);
            Assert.NotNull(_trackManager.characterController.characterCollider);
            Assert.NotNull(_trackManager.characterController.characterCollider.controller);
            _trackManager.characterController.characterCollider.controller.currentLife = 1;
            int safeSegmentOverride = 0;
            Debug.Log($"Overriding safe segment value to {safeSegmentOverride}");
            _trackManager.ReflectionSetFieldValue("m_SafeSegementLeft", safeSegmentOverride);

            var overridedPossibleObstacleList = new List<AssetReference>();
            var assetRef = GetFirstLowerObstacleFromTheme(_trackManager.currentTheme);
            overridedPossibleObstacleList.Add(assetRef);
            Assert.IsNotEmpty(overridedPossibleObstacleList);
            _trackManager.currentSegment.possibleObstacles = overridedPossibleObstacleList.ToArray();

            // Different from SpawnCoinAndPowerup method, this is not an IEnumerator so it's call cant be yield,
            // game code change would need to be suggested to expose access to more precise testing.
            _trackManager.SpawnObstacle(_trackManager.currentSegment);

            yield return new WaitForSeconds(1f);
            Assert.LessOrEqual(_trackManager.characterController.characterCollider.controller.currentLife, 0);
        }

        #endregion Tests

        #region General

        AssetReference GetFirstLowerObstacleFromTheme(ThemeData themeData)
        {
            Assert.NotNull(themeData);
            foreach (var zone in themeData.zones) {
                foreach (var prefab in zone.prefabList) {
                    if (prefab.editorAsset is not GameObject go) continue;
                    if (!go.TryGetComponent(out TrackSegment trackSegment)) continue;
                    foreach (var obstacleAssetRef in trackSegment.possibleObstacles) {
                        var gameObject = obstacleAssetRef.editorAsset as GameObject;
                        Assert.NotNull(gameObject);
                        var lowerName = gameObject.name.ToLower();
                        if (!lowerName.Contains("low")) continue;
                        if (!gameObject.TryGetComponent(out AllLaneObstacle _)) continue;
                        return obstacleAssetRef;
                    }
                }
            }
            Assert.Fail("No Lower obstacle found on theme: " + themeData.name);
            return default;
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

        #endregion General

    }
}