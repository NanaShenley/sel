using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests
{
    public class DENIYesNoCheckForLeaver
    {
        [WebDriverTest(Groups = new[] { "SignIn", "debug" }, Enabled = true)]
        public void DENIYesNoCheckForLeaverDENIReturn()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement leaver = WebContext.WebDriver.FindElement(By.CssSelector("#editableData > div > div:nth-child(11) > div.panel-heading > h4 > a > span.title"));
            leaver.Click();
            Thread.Sleep(4000);

            IWebElement surName = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='0'] div[class='webix_cell']"));
            surName.Click();

            Actions action = new Actions(WebContext.WebDriver);
            for (var index = 0; index < 7; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement educationSite = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='7'] div[class='webix_cell']"));
            Assert.IsTrue(educationSite.Text == "No" || educationSite.Text == "Yes");

            for (var index = 0; index < 5; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement FSMEligible = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='11'] div[class='webix_cell']"));

            Assert.IsTrue(FSMEligible.Text == "No" || FSMEligible.Text == "Yes");

            for (var index = 0; index < 15; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement leaverData = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='23'] div[class='webix_cell']"));
            IWebElement zeroRated = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='24'] div[class='webix_cell']"));
            IWebElement specialUnit = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils div[column='27'] div[class='webix_cell']"));

            Assert.IsTrue(leaverData.Text == "No" || leaverData.Text == "Yes");
            Assert.IsTrue(zeroRated.Text == "No" || zeroRated.Text == "Yes");
            Assert.IsTrue(specialUnit.Text == "No" || specialUnit.Text == "Yes");

            WebContext.Screenshot();
        }
    }
}
