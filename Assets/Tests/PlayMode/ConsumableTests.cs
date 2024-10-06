using System.Collections;
using System.Linq;
using NUnit.Framework;
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
            var charTransform = _trackManager.characterController.transform;
            var op = Addressables.InstantiateAsync(coinMagnet.name, charTransform.position, charTransform.rotation, charTransform);
            yield return op;
            Assert.NotNull(op);
            op.Result.transform.position = charTransform.position;
            yield return SpawnLotOfFishes(_trackManager.currentSegment);
            int oldScore = _trackManager.score;
            Debug.Log($"Old score is {oldScore}");
            yield return new WaitForSeconds(2f);
            Debug.Log($"New score is {_trackManager.score}");
            Assert.GreaterOrEqual(_trackManager.score, oldScore + 12);
        }

    }
}