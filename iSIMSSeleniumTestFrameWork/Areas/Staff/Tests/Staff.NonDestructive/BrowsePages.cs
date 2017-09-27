using NUnit.Framework;
using Selene.Support.Attributes;
using SeSugar.Automation;
using SeSugar.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Environment = SeSugar.Environment;

namespace Staff.NonDestructive
{
    //Use SeSugar only
    public class BrowsePages
    {
        [Variant(Variant.EnglishStatePrimary | Variant.NorthernIrelandStatePrimary | Variant.WelshStatePrimary)]
        [ChromeUiTest("P1")]
        public void Load_a_Staff_record()
        {
            var wd = Environment.WebContext.WebDriver;
            wd.Url = GetSutUrl();

            //Login (SIMS ID login page)
            var usernameLoc = SimsBy.Id("username");
            var passwordLoc = SimsBy.Id("password");
            var loginButtonLoc = SimsBy.Id("login-button");

            AutomationSugar.WaitFor(usernameLoc);
            wd.FindElement(usernameLoc).SendKeys(TestSettings.TestDefaults.Default.TestUser);

            AutomationSugar.WaitFor(passwordLoc);
            wd.FindElement(passwordLoc).SendKeys(TestSettings.TestDefaults.Default.TestUserPassword);

            AutomationSugar.WaitFor(loginButtonLoc);
            AutomationSugar.ClickOn(loginButtonLoc);

            //Wait for something on the homepage
            AutomationSugar.WaitFor("StaffAbsenceWidget");

            //Go to Staff record
            AutomationSugar.NavigateMenu("Tasks", "Staff", "Staff Records");
            
            //Wait for the search button and then click it, then wait for some search results
            var searchButtonLoc = SimsBy.AutomationId("search_criteria_submit");
            AutomationSugar.WaitFor(searchButtonLoc);

            var searchResultLoc = SimsBy.AutomationId("search_result");
            AutomationSugar.ClickOnAndWaitFor(searchButtonLoc, searchResultLoc);

            //Get search results and the click the first one
            var searchResults = wd.FindElements(searchResultLoc);
            searchResults.First().Click();

            //Wait for an arbitrary element to load on the clocked staff record 
            AutomationSugar.WaitFor(SimsBy.Name("LegalForename"));

            Assert.IsTrue(searchResults.Any(), "No Staff search results.");
        }

        private string GetSutUrl()
        {
            const string slash = "/";
            bool baseEndsWithSlash = TestSettings.TestDefaults.Default.URL.EndsWith(slash);
            return baseEndsWithSlash 
                ? string.Concat(TestSettings.TestDefaults.Default.URL, TestSettings.TestDefaults.Default.Path)
                : string.Concat(TestSettings.TestDefaults.Default.URL, slash, TestSettings.TestDefaults.Default.Path);
        }
    }
}
