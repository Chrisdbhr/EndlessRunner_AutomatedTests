using NUnit.Framework;
using Tests.Utils;

namespace Tests.EditMode
{
    public class GeneralTests
    {
        [Test, Order(9999), Category(TestStrings.UnitCategoryName)]
        public void OpenLink_Calls_OpenURL()
        {
            TestUtils.LogStartTestInformation(nameof(OpenLink_Calls_OpenURL));
            if (TestContext.CurrentContext.Result.FailCount < 0) {
                Assert.Ignore($"Test '{nameof(OpenLink_Calls_OpenURL) }' ignored because some other test failed. This tests opens an external URL so it can be done by last to prevent opening too many URLs thorugh test sessions");
            }
            var openUrlMonoBehaviour = TestUtils.TestableObjectFactory.Create<OpenURL>();
            openUrlMonoBehaviour.websiteAddress = "https://chrisjogos.com";
            openUrlMonoBehaviour.OpenURLOnClick();
            // No assertions since there is no way to know if the URL opened correctly because the
            // user can deny the Administrator Popup, so we are just testing the proccess so far
        }

    }
}