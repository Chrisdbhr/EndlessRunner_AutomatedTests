using System.Collections;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Tests.PlayMode
{
    public class GameplayTests : PlayModeTest
    {
        #region Properties and Fields

        protected LoadoutState _loadoutState;
        protected TrackManager _trackManager;

        #endregion Properties and Fields

        #region Setup and Teardown

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            Debug.Log($"Setup {nameof(CharacterTests)}");
            if (!File.Exists(TestStrings.MainScene))
            {
                Assert.Inconclusive($"The path to '{nameof(TestStrings.MainScene)}' is incorrect.");
            }
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
            Object.Destroy(_loadoutState);
            Object.Destroy(_trackManager);
        }

        #endregion Setup and Teardown

        #region General

        protected void OverrideObstacleListWithOnlyLowerObstacles()
        {
            int safeSegmentOverride = 0;
            Debug.Log($"Overriding safe segment value to {safeSegmentOverride}");
            _trackManager.ReflectionSetFieldValue("m_SafeSegementLeft", safeSegmentOverride);

            var overridedPossibleObstacleList = new List<AssetReference>();
            var assetRef = GetFirstLowerObstacleFromTheme(_trackManager.currentTheme);
            overridedPossibleObstacleList.Add(assetRef);
            Assert.IsNotEmpty(overridedPossibleObstacleList);
            _trackManager.currentSegment.possibleObstacles = overridedPossibleObstacleList.ToArray();
        }

        protected AssetReference GetFirstLowerObstacleFromTheme(ThemeData themeData)
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

        protected IEnumerator SpawnInPlayerPosition(string assetName)
        {
            Assert.NotNull(assetName);
            var charTransform = _trackManager.characterController.transform;
            var op = Addressables.InstantiateAsync(assetName, charTransform.position, charTransform.rotation, charTransform);
            yield return op;
            Assert.NotNull(op);
            op.Result.transform.position = charTransform.position;
        }

        protected IEnumerator MainGameSceneSetup()
        {
            Debug.Log($"Loading scene: {TestStrings.MainScene}");
            SceneManager.LoadScene(TestStrings.MainScene);
            yield return null;
            Debug.Log($"Setting Tutorial as done");
            PlayerData.instance.tutorialDone = true;
        }

        protected IEnumerator WaitUntilGameStarts()
        {
            yield return null;
            _loadoutState = Object.FindFirstObjectByType<LoadoutState>();
            Assert.NotNull(_loadoutState, "Cant find LoadoutState in Main scene.");
            _loadoutState.StartGame();
            yield return new WaitForSeconds(0.01f);
            _trackManager = Object.FindFirstObjectByType<TrackManager>(FindObjectsInactive.Include);
            Assert.NotNull(_trackManager);
            var desiredSegmentCount = (int)typeof(TrackManager).ReflectionGetConstFieldValue("k_DesiredSegmentCount");
            Assert.NotZero(desiredSegmentCount);
            var listOfSegments = _trackManager.ReflectionGetFieldValue("m_Segments") as IList;
            Debug.Log("Waiting for segments to spawn");
            yield return new WaitUntil(() => listOfSegments != null && listOfSegments.Count >= desiredSegmentCount);
            Debug.Log("Skipping start animation");
            _trackManager.ReflectionSetFieldValue("m_TimeToStart", -1f);
        }

        protected IEnumerator SpawnLotOfFishes(TrackSegment segment)
        {
            Assert.NotNull(segment);
            _trackManager.ReflectionSetFieldValue("m_TimeSincePowerup", -9999f);
            _trackManager.ReflectionSetFieldValue("m_TimeSinceLastPremium", -9999f);
            int maxLoopTimes = 30;
            Debug.Log($"Spawning a lot of fishes on segment: {segment.name}", segment);
            Coin[] coins;
            do {
                if (maxLoopTimes <= 0) break;
                yield return _trackManager.SpawnCoinAndPowerup(segment);
                maxLoopTimes--;
                coins = segment.GetComponentsInChildren<Coin>();
            } while (coins == null || coins.Length < 40);
            if(maxLoopTimes <= 0) Debug.Log($"Spawned fishes used the {nameof(maxLoopTimes)} limit.", segment);
        }

        #endregion General
    }
}