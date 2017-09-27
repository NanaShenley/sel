using DataExchange.Components.Common;
using NUnit.Framework;
using OpenQA.Selenium;
using System.Threading;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace DataExchange.Tests
{
    public class DENIPupilIdentifierForAssessmentKeyStage
    {
        [WebDriverTest(Groups = new[] {"SignIn", "debug"}, Browsers = new[] {"chrome"})]
        public void PupilIdentifierForLeaversDENIReturn()
        {
            var navigatetodeni = new NavigateToDENI();
            navigatetodeni.NavigateToDENIScreen();
            //navigatetodeni.CreateDENIReturn();
            navigatetodeni.SearchDeni();
            WebContext.Screenshot();
            IWebElement AssessmentKeyStage1Result =
                WebContext.WebDriver.FindElement(
                    By.CssSelector("#editableData > div > div:nth-child(10) > div.panel-heading > h4 > a > span.title"));
            AssessmentKeyStage1Result.Click();
            Thread.Sleep(3000);
            IWebElement admissionNumber =
                WebContext.WebDriver.FindElement(
                    By.CssSelector("#AssessmentKeyStage1ResultSection_AssessmentKeyStage1Results td[column='2'] span"));
            IWebElement UPN =
                WebContext.WebDriver.FindElement(
                    By.CssSelector("#AssessmentKeyStage1ResultSection_AssessmentKeyStage1Results td[column='3'] span"));

            Assert.IsTrue(admissionNumber.Text == "Admission Number" && UPN.Text == "UPN");
            WebContext.Screenshot();
        }
    }
}