using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriverRunner.VisualStudioUnitTesting
{
    [TestClass]
    public class MsBasePrePostConditions
    {
        #region MS Unit Testing support
        public static TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void Init(TestContext TestContex) 
        {
            TestRunner.VSSeleniumTest.Init(new MsBasePrePostConditions(), TestContext);
        }
        [ClassCleanup]
        public static void Cleanup()
        {
            TestRunner.VSSeleniumTest.Cleanup(TestContext);
        }
        #endregion
    }
}
