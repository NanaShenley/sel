using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using Staff.POM.Helper;
using SeSugar.Automation;

namespace Staff.POM.Components.LoginPages
{
    public class SelectSchool
    {
        #pragma warning disable 0649
        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.Id, Using = "DomainID")]
        IWebElement _schoolDropdown;

        [FindsBy(How = How.TagName, Using = "button")]
        private IWebElement _signInButton;
        // ReSharper restore UnassignedField.Compiler
        #pragma warning restore 0649


        public SelectSchool()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public void SelectBySchoolName(string schoolName)
        {
            By loc =  By.TagName("button");
            Wait.WaitForElement(loc);
            new SelectElement(_schoolDropdown).SelectByText(schoolName);
        }

        public void SelectBySchoolValue(string value)
        {
            new SelectElement(_schoolDropdown).SelectByText(value);

        }

        public void SignIn()
        {
            AutomationSugar.WaitFor("sign-in-organisation-home");
            AutomationSugar.ClickOnAndWaitFor(SimsBy.AutomationId("sign-in-organisation-home"), By.Id("shell"));
        }

        //public void ValidateElements()
        //{
        //    Assert.IsTrue(_schoolDropdown.Displayed);
        //}
    }
}
