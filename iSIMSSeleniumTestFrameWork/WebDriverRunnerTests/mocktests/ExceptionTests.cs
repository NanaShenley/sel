using Selene.Support.Attributes;
using System.Reflection;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace WebDriverRunnerTests.mocktests
{
    public class ExceptionTests
    {
        [UnitTest]
        public void ExceptionTest()
        {
            Assembly.LoadFrom("somewhere.");
        }


        [UnitTest]
        public void ScreenshotMethod()
        {
            var s = new ScreenshotLog("page title", "reports/screenshot/lkfdkjdf.jpg", "name");
            TestResultReporter.Log(s);
            Assembly.LoadFrom("somewhere.");
        }

        public class Screenshot
        {
            public readonly string Type = "Screenshot";
            public string Name;
            public string Title;
            public string Path;

        }
    }
}
