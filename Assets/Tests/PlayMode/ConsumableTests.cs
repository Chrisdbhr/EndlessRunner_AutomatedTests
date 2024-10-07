using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.PlayMode.Mocks;
using Tests.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class ConsumableTests : GameplayTests
    {

        [UnityTest]
        public IEnumerator TestCoinMagnet()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(TestCoinMagnet)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            Assert.NotNull(_trackManager.consumableDatabase);
            Assert.IsNotEmpty(_trackManager.consumableDatabase.consumbales);
            var coinMagnet = _trackManager.consumableDatabase.consumbales.FirstOrDefault(c => c is CoinMagnet) as CoinMagnet;
            Assert.NotNull(coinMagnet);
            var overrideConsumableDb = ScriptableObject.CreateInstance<ConsumableDatabase>();
            overrideConsumableDb.consumbales = new Consumable[] { coinMagnet };
            overrideConsumableDb.Load();
            _trackManager.consumableDatabase = overrideConsumableDb;
            yield return SpawnInFrontOfPlayer(coinMagnet.name);
            yield return SpawnLotOfFishes(_trackManager.currentSegment);
            int oldScore = _trackManager.score;
            Debug.Log($"Old score is {oldScore}");
            yield return new WaitForSeconds(2f);
            Debug.Log($"New score is {_trackManager.score}");
            Assert.GreaterOrEqual(_trackManager.score, oldScore + 12);
        }

        [UnityTest]
        public IEnumerator TestExtraLife()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(TestExtraLife)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var extraLife = _trackManager.consumableDatabase.consumbales.FirstOrDefault(c => c is ExtraLife) as ExtraLife;
            Assert.NotNull(extraLife);
            _trackManager.characterController.currentLife = 2;
            yield return SpawnInFrontOfPlayer(extraLife.name);
            Assert.GreaterOrEqual(3, _trackManager.characterController.currentLife);
        }

        [UnityTest]
        public IEnumerator TestInvincibility()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(TestInvincibility)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var invincibility = _trackManager.consumableDatabase.consumbales.FirstOrDefault(c => c is Invincibility) as Invincibility;
            Assert.NotNull(invincibility);
            yield return SpawnInFrontOfPlayer(invincibility.name);
            int initialLife = _trackManager.characterController.currentLife;
            var mockObstacle = new GameObject("Mock obstacle").AddComponent<MockObstacle>();
            OverrideObstacleListWithOnlyLowerObstacles();

            // Different from SpawnCoinAndPowerup method, this is not an IEnumerator so it's call cant be yield,
            // game code change would need to be suggested to expose access to more precise testing.
            _trackManager.SpawnObstacle(_trackManager.currentSegment);
            yield return mockObstacle.Spawn(_trackManager.currentSegment,1f);

            yield return new WaitForSeconds(1.7f);
            Assert.AreEqual(initialLife,_trackManager.characterController.currentLife);
        }

        [UnityTest]
        public IEnumerator TestScore2Multiplier()
        {
            Debug.Log($"{TestStrings.TestStartLogPrefix}{nameof(TestScore2Multiplier)}");
            yield return MainGameSceneSetup();
            yield return WaitUntilGameStarts();
            var score2Multiplier = _trackManager.consumableDatabase.consumbales.FirstOrDefault(c => c is Score2Multiplier) as Score2Multiplier;
            Assert.NotNull(score2Multiplier);
            TestHelperGameObject.SupressApplicationPausePrevention = true;
            Assert.NotNull(GameManager.instance);
            var gameState = GameManager.instance.topState as GameState;
            Assert.NotNull(gameState);
            gameState.Pause();
            foreach (var coinInWorld in Object.FindObjectsOfType<Coin>()) {
                Object.Destroy(coinInWorld.gameObject);
            }
            yield return this.SpawnInFrontOfPlayer(score2Multiplier.name);
            var charController = _trackManager.characterController;
            Assert.NotNull(charController);
            var coinFromPool = Coin.coinPool.Get(charController.transform.position + Vector3.forward * 2f,Quaternion.identity);
            Assert.NotNull(coinFromPool);
            gameState.Resume();
            TestHelperGameObject.SupressApplicationPausePrevention = true;
            int oldScore = _trackManager.score;
            Debug.Log($"Old score is {oldScore}");
            yield return new WaitForSeconds(1f);
            Debug.Log($"New score is {_trackManager.score}");
            Assert.GreaterOrEqual(_trackManager.score, 18);
        }

    }
}