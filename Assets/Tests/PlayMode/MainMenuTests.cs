using System.Collections;
using System.Linq;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode
{
    public class MainMenuTests : MainSceneTests
    {
        [UnityTest, Category(TestStrings.IntegrationCategoryName)]
        public IEnumerator PlayerSaveDataDeletionThroughUIMenus()
        {
            TestUtils.LogStartTestInformation(nameof(PlayerSaveDataDeletionThroughUIMenus));
            yield return MainGameSceneSetup();
            yield return null;
            Assert.NotNull(PlayerData.instance);
            PlayerData.instance.coins += 100;
            yield return null;
            var settingButton = Object.FindObjectsByType<Button>(FindObjectsSortMode.None).FirstOrDefault(b => b.name == "SettingButton");
            Assert.NotNull(settingButton);
            TestUtils.SimulateButtonClick(settingButton);
            yield return null;
            var deleteDataButton = Object.FindObjectsByType<Button>(FindObjectsSortMode.None).FirstOrDefault(b => b.name == "DeleteData");
            Assert.NotNull(deleteDataButton);
            TestUtils.SimulateButtonClick(deleteDataButton);
            yield return null;
            var yesButton = Object.FindObjectsByType<Button>(FindObjectsSortMode.None).FirstOrDefault(b => b.name == "YESButton");
            Assert.NotNull(yesButton);
            TestUtils.SimulateButtonClick(yesButton);
            Assert.LessOrEqual(PlayerData.instance.coins, 0);
            Assert.AreNotSame(PlayerData.instance.tutorialDone, true);
        }

        [Test, Category(TestStrings.UnitCategoryName)]
        public void PlayerDataDeletionTest()
        {
            TestUtils.LogStartTestInformation(nameof(PlayerDataDeletionTest));
            PlayerData.Create();
            Assert.NotNull(PlayerData.instance);
            PlayerData.instance.coins += 100;
            PlayerData.NewSave();
            Assert.Zero(PlayerData.instance.coins);
        }
    }
}