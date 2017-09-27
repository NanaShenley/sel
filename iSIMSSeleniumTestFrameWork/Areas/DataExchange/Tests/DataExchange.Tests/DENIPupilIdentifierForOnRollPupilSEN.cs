using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;


namespace DataExchange.Tests
{
    public class DENIPupilIdentifierForOnRollPupilSEN
    {
        [WebDriverTest(Groups = new[] { "SignIn", "debug" })]
        public void PupilIdentifierDENIReturn_PupilSEN()
        {

            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement pupilSEN = WebContext.WebDriver.FindElement(By.CssSelector("#editableData > div > div:nth-child(9) > div.panel-heading > h4 > a > span.title"));
            pupilSEN.Click();
            Thread.Sleep(300);
            IWebElement admissionNumber = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSENSection_OnRollPupilSENStatuses td[column='2'] span"));
            IWebElement UPN = WebContext.WebDriver.FindElement(By.CssSelector("#OnRollPupilSENSection_OnRollPupilSENStatuses td[column='3'] span"));

            Assert.IsTrue(admissionNumber.Text == "Admission Number" && UPN.Text == "UPN");
            WebContext.Screenshot();
        }

    }
}
