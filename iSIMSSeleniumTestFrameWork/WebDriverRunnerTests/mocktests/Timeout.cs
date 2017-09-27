using Selene.Support.Attributes;
using System.Threading;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class Timeout
    {
        [UnitTest(TimeoutSeconds = 1,Groups = new[] { "all"})]
        public void TestPass()
        {
            Thread.Sleep(2000);
        }
    }
}
