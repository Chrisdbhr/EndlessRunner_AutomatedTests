using NUnit.Framework;
using Tests.PlayMode.Utils;
using UnityEngine;

namespace Tests.PlayMode
{
    public abstract class PlayModeTests
    {
        protected TestHelperGameObject TestHelperGameObject;

        #region Setup and Teardown

        [SetUp]
        public virtual void Setup()
        {
            Time.timeScale = 2f;
            TestHelperGameObject = new GameObject(nameof(TestHelperGameObject)).AddComponent<TestHelperGameObject>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Time.timeScale = 1f;
            Object.Destroy(TestHelperGameObject);
        }

        #endregion Setup and Teardown

    }
}