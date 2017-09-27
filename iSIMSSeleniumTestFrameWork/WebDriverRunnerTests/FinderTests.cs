/*************************************************************************
* 
* Copyright © Capita Children's Services 2015
* All Rights Reserved.
* Proprietary and confidential
* Written by Steve Gray <steve.gray@capita.co.uk> and Francois Reynaud<Francois.Reynaud@capita.co.uk> 2015
* 
* NOTICE:  All Source Code and information contained herein remains
* the property of Capita Children's Services. The intellectual and technical concepts contained
* herein are proprietary to Capita Children's Services 2015 and may be covered by U.K, U.S and Foreign Patents,
* patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material
* is strictly forbidden unless prior written permission is obtained
* from Capita Children's Services.
*
* Source Code distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied.  
*/
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDriverRunner.Filters;
using WebDriverRunner.testfinder;

namespace WebDriverRunnerTests
{
    [TestClass]
    public class FinderTests
    {
        private IFilter<MethodInfo> test0 = Helper.ByClass("WebDriverRunnerTests.mocktests.Tests0");
        private IFilter<MethodInfo> test1 = Helper.ByClass("WebDriverRunnerTests.mocktests.Tests1");

        

        [TestMethod]
        public void FilterByAssemblyWithoutDll()
        {
            var a = Assembly.GetExecutingAssembly();
            var finder = new TestFinder(a, test1);
            var methods = finder.FindAllMethods();
            Assert.AreEqual(2, methods.Count());
        }

        [TestMethod]
        public void NoTests()
        {
            var a = Assembly.GetExecutingAssembly();
            var finder = new TestFinder(a, test0);
            var methods = finder.FindAllMethods();
            Assert.AreEqual(0, methods.Count());
        }
    }
}
