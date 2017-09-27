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
using Selene.Support.Attributes;
using WebDriverRunner.internals;
using WebDriverRunner.results;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.Listners
{
    class WebDriverTestListener : TestlistenerBase
    {

        public WebDriverTestListener(Configuration config)
            : base(config)
        {
        }

        public override void OnTestStart(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(WebDriverTestAttribute)
                || instance.Attr.GetType() == typeof(ChromeUiTestAttribute)
                || instance.Attr.GetType() == typeof(IeUiTestAttribute)
                || instance.Attr.GetType() == typeof(AllBrowserUiTestAttribute)
                )
            {
                SetUpAndStartBrowsers(instance,result);
            }

        }

        public override void OnTestFinished(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(WebDriverTestAttribute)
                 || instance.Attr.GetType() == typeof(ChromeUiTestAttribute)
                 || instance.Attr.GetType() == typeof(IeUiTestAttribute)
                 || instance.Attr.GetType() == typeof(AllBrowserUiTestAttribute)
                 )
            {
                CloseBrowser(WebContext.WebDriver);
            }

        }

        public override void OnTestPassed(TestMethodInstance instance, ITestResult result)
        {
        }

        public override void OnTestFailed(TestMethodInstance instance, ITestResult result)
        {
            if (instance.Attr.GetType() == typeof(WebDriverTestAttribute)
                 || instance.Attr.GetType() == typeof(ChromeUiTestAttribute)
                 || instance.Attr.GetType() == typeof(IeUiTestAttribute)
                 || instance.Attr.GetType() == typeof(AllBrowserUiTestAttribute)
                 )
            {
                Screenshot();
            }
        }



        public override void OnTestSkipped(TestMethodInstance instance, ITestResult result)
        {
        }

    }
}
