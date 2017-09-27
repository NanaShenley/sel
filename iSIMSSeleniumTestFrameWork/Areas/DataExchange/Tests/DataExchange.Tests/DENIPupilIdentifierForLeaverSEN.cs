using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests
{
    class DENIPupilIdentifierForLeaverSEN
    {
        [WebDriverTest(Groups = new[] { "SignIn", "debug" })]
        public void PupilIdentifierDENIReturn_LeaverSEN()
        {

            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement LeaverSEN = WebContext.WebDriver.FindElement(By.CssSelector("#editableData > div > div:nth-child(12) > div.panel-heading > h4 > a > span.title"));
            LeaverSEN.Click();
            Thread.Sleep(300);
            IWebElement admissionNumberSENStatus = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversSENSection_LeaversSENStatuses td[column='2'] span"));
            IWebElement UPNSENStatus = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversSENSection_LeaversSENStatuses td[column='3'] span"));
            IWebElement admissionNumberSENNeed = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversSENSection_LeaversSENNeeds td[column='2'] span"));
            IWebElement UPNSENNeed = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversSENSection_LeaversSENNeeds td[column='3'] span"));
            Assert.IsTrue(admissionNumberSENStatus.Text == "Admission Number" && UPNSENStatus.Text == "UPN" && admissionNumberSENNeed.Text == "Admission Number" && UPNSENNeed.Text == "UPN");
            WebContext.Screenshot();
        }

    }
}
