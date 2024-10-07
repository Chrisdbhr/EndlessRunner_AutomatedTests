using NUnit.Framework;
using Tests.PlayMode.Utils;
using UnityEngine;

namespace Tests.PlayMode
{
    public abstract class PlayModeTest
    {
        protected TestHelperGameObject TestHelperGameObject;

        #region Setup and Teardown

        [SetUp]
        public virtual void Setup()
        {
            TestHelperGameObject = new GameObject(nameof(TestHelperGameObject)).AddComponent<TestHelperGameObject>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            Object.Destroy(TestHelperGameObject);
        }

        #endregion Setup and Teardown

    }
}