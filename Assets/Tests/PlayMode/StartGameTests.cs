using System.Collections;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.PlayMode
{
    public class StartGameTests : PlayModeTests
    {
        [UnityTest, Category(TestStrings.IntegrationCategoryName)]
        public IEnumerator StartButtonStartGame()
        {
            TestUtils.LogStartTestInformation(nameof(StartButtonStartGame));
            var op = SceneManager.LoadSceneAsync("Start");
            yield return op;
            var startButton = Object.FindObjectOfType<StartButton>();
            Assert.NotNull(startButton);
            var buttonComp = startButton.GetComponent<Button>();
            Assert.NotNull(buttonComp);

            var currentScene = SceneManager.GetActiveScene();

            TestUtils.SimulateButtonClick(buttonComp);
            yield return new WaitUntil(() => currentScene == SceneManager.GetActiveScene());

            Assert.IsTrue(currentScene != SceneManager.GetActiveScene());
        }
    }
}