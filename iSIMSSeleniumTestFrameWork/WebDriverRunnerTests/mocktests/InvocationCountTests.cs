using Selene.Support.Attributes;
using Selene.Support.Attributes;
using WebDriverRunner.internals;

namespace WebDriverRunnerTests.mocktests
{
    class InvocationCountTests
    {
        [UnitTest(InvocationCount = 50,Groups = new[] { "all"})]
        public void Count()
        {
            
        }
    }
}
