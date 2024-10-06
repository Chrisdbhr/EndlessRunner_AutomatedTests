using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tests.PlayMode.Mocks;
using Tests.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tests.PlayMode
{
    public class CharacterTests : GameplayTests
    {

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
            yield return SpawnLotOfFishes(_trackManager.currentSegment);
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

        [UnityTest,Order(3)]
        public IEnumerator ResetProcessWorkingAsExpected()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(ResetProcessWorkingAsExpected)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            Assert.NotNull(_trackManager.characterController);
            Assert.NotNull(_trackManager.characterController.characterCollider);
            Assert.NotNull(_trackManager.characterController.characterCollider.controller);
            _trackManager.characterController.characterCollider.controller.currentLife = 0;
            var mockObstacle = new GameObject("Mock obstacle").AddComponent<MockObstacle>();
            yield return mockObstacle.Spawn(_trackManager.currentSegment,1f);

            yield return new WaitForSeconds(3f);

            // I am using 'FindObjectsOfType' more than one time because active and Instantiate objects keep changing every UI update.

            Debug.Log("Looking for the GameOver button to click");
            var gameOverButton = Object.FindObjectsOfType<Button>().FirstOrDefault(b => b.name == "GameOver");
            Assert.NotNull(gameOverButton);
            TestUtils.SimulateButtonClick(gameOverButton);

            yield return new WaitForSeconds(.5f);

            var closeButton = Object.FindObjectsOfType<Button>().FirstOrDefault(b => b.name == "CloseButton");
            // If no mission completed, maybe the popup didnt show up so no assert if null.
            if (closeButton != null) {
                TestUtils.SimulateButtonClick(closeButton);
                yield return null;
            }

            yield return new WaitForSeconds(1f);

            var runButton = Object.FindObjectsOfType<Button>().FirstOrDefault(b => b.name == "RunButton");
            Assert.NotNull(runButton);
            TestUtils.SimulateButtonClick(runButton);
            yield return null;
            Assert.Greater(_trackManager.characterController.characterCollider.controller.currentLife, 0);
        }

        #endregion Tests

    }
}