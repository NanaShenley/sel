using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests
{
    public class DENIPupilIdentifierForLeaver
    {
        [WebDriverTest(Groups = new[] { "SignIn", "debug" })]
        public void PupilIdentifierForLeaversDENIReturn()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement leaver = WebContext.WebDriver.FindElement(By.CssSelector("#editableData > div > div:nth-child(11) > div.panel-heading > h4 > a > span.title"));
            leaver.Click();
            Thread.Sleep(500);
            IWebElement admissionNumber = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils td[column='2'] span"));
            IWebElement UPN = WebContext.WebDriver.FindElement(By.CssSelector("#LeaversPupilSection_LeaversPupils td[column='3'] span"));

            Assert.IsTrue(admissionNumber.Text == "Admission Number" && UPN.Text == "UPN");
            WebContext.Screenshot();
        }
    }
}
