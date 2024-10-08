using System.Collections;
using NUnit.Framework;
using Tests.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class GeneralTests
    {
        #region Level Loading

        [UnityTest, Category(TestStrings.UnitCategoryName)]
        public IEnumerator LevelLoader_LoadLevel()
        {
            TestUtils.LogStartTestInformation(nameof(LevelLoader_LoadLevel));
            var levelLoader = TestableMonobehaviourFactory.Create<LevelLoader>();
            levelLoader.LoadLevel(TestStrings.MainSceneName);
            yield return AssertForSceneLoaded(TestStrings.MainSceneName);
        }

        // // Test was implemented in a code not executed
        // [UnityTest, Category(TestStrings.UnitCategoryName)]
        // public IEnumerator MainMenu_LoadLevel()
        // {
        //     TestUtils.LogStartTestInformation(nameof(MainMenu_LoadLevel));
        //     var levelLoader = TestUtils.TestableObjectFactory.Create<MainMenu>();
        //     levelLoader.LoadScene(TestStrings.MainSceneName);
        //     yield return AssertForSceneLoaded(TestStrings.MainSceneName);
        // }

        IEnumerator AssertForSceneLoaded(string sceneName)
        {
            yield return null; // waiting for scene activation
            var activeScene = SceneManager.GetActiveScene();
            Assert.IsTrue(activeScene.isLoaded, "Scene was not loaded");
            Assert.AreEqual(sceneName, activeScene.name, "Loaded scene name does not match");
        }

        #endregion Level Loading
    }
}