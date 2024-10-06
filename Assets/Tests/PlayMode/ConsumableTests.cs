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
            yield return SpawnInPlayerPosition(coinMagnet.name);
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
            yield return SpawnInPlayerPosition(extraLife.name);
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
            yield return SpawnInPlayerPosition(invincibility.name);
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

    }
}