using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode
{
    public class StoreTests : MainSceneTests
    {
        [UnityTest]
        public IEnumerator ShopPurchases()
        {
            TestUtils.LogStartTestInformation(nameof(ShopPurchases));
            yield return MainGameSceneSetup();
            Assert.NotNull(PlayerData.instance);
            PlayerData.instance.coins += 1000;
            var storeButton = Object.FindObjectsByType<Button>(FindObjectsSortMode.None).FirstOrDefault(b => b.name == "StoreButton");
            Assert.NotNull(storeButton);
            TestUtils.SimulateButtonClick(storeButton);
            yield return new WaitUntil(() => CharacterDatabase.loaded);
            yield return new WaitUntil(() => ThemeDatabase.loaded);

            var shopUI = Object.FindFirstObjectByType<ShopUI>();
            Assert.NotNull(shopUI);
            Assert.NotNull(shopUI.itemList);

            foreach (var consumableType in TestUtils.GetValues<Consumable.ConsumableType>()) {
                if(consumableType is Consumable.ConsumableType.NONE or Consumable.ConsumableType.MAX_COUNT) continue;
                PlayerData.instance.consumables.TryGetValue(consumableType, out var oldAmount);
                var consumable = ConsumableDatabase.GetConsumbale(consumableType);
                Assert.NotNull(consumable);
                Debug.Log($"Testing purchse of: {consumable.GetConsumableName()}");
                yield return null;
                shopUI.itemList.Buy(consumable);
                Assert.Greater(PlayerData.instance.consumables[consumableType], oldAmount);
            }

        }

    }
}