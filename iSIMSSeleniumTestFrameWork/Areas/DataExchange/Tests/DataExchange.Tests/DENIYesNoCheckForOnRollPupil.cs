using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests
{
    public class DENIYesNoCheckForOnRollPupil
    {
        [WebDriverTest(Groups = new[] { "SignIn", "debug" }, Enabled = true)]
        public void DENIYesNoCheckForOnRollPupilDENIReturn()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement onRollPupil = WebContext.WebDriver.FindElement(By.CssSelector("#editableData > div > div:nth-child(7) > div.panel-heading > h4 > a > span.title"));
            onRollPupil.Click();
            Thread.Sleep(4000);

            IWebElement educationSite = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='7'] div[class='webix_cell']"));
            Assert.IsTrue(educationSite.Text == "No" || educationSite.Text == "Yes");

            educationSite.Click();

            Actions action = new Actions(WebContext.WebDriver);
            for (var index = 0; index < 5; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement incomeSupport = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='9'] div[class='webix_cell']"));
            IWebElement irishMedium = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='10'] div[class='webix_cell']"));

            Assert.IsTrue(incomeSupport.Text == "No" || incomeSupport.Text == "Yes");
            Assert.IsTrue(irishMedium.Text == "No" || irishMedium.Text == "Yes");

            for (var index = 0; index < 2; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement isDisabled = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='13'] div[class='webix_cell']"));

            Assert.IsTrue(isDisabled.Text == "No" || isDisabled.Text == "Yes");

            for (var index = 0; index < 8; index++)
                action.SendKeys(Keys.Right).Perform();

            IWebElement FSMEligible = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='18'] div[class='webix_cell']"));
            IWebElement isBorder = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='21'] div[class='webix_cell']"));

            Assert.IsTrue(FSMEligible.Text == "No" || FSMEligible.Text == "Yes");
            Assert.IsTrue(isBorder.Text == "No" || isBorder.Text == "Yes");

            for (var index = 0; index < 7; index++)
                action.SendKeys(Keys.Right).Perform();
            IWebElement compositeClass = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='24'] div[class='webix_cell']"));

            Assert.IsTrue(compositeClass.Text == string.Empty || compositeClass.Text == "No" || compositeClass.Text == "Yes");

            for (var index = 0; index < 6; index++)
                action.SendKeys(Keys.Right).Perform();
            IWebElement feePayer = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='31'] div[class='webix_cell']"));

            Assert.IsTrue(feePayer.Text == "No" || feePayer.Text == "Yes");

            for (var index = 0; index < 7; index++)
                action.SendKeys(Keys.Right).Perform();
            IWebElement uniformGrant = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='38'] div[class='webix_cell']"));
            IWebElement zeroRated = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='39'] div[class='webix_cell']"));
            IWebElement specialUnit = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSection_OnRollPupils div[column='42'] div[class='webix_cell']"));

            Assert.IsTrue(uniformGrant.Text == "No" || uniformGrant.Text == "Yes");
            Assert.IsTrue(zeroRated.Text == "No" || zeroRated.Text == "Yes");
            Assert.IsTrue(specialUnit.Text == "No" || specialUnit.Text == "Yes");

            WebContext.Screenshot();
        }
    }
}
