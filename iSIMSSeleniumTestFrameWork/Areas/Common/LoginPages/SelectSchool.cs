using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace SharedComponents.LoginPages
{
    public class SelectSchool: BaseSeleniumComponents
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
            By by = By.Id("DomainID");
            WaitForElement(by);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        public void SelectBySchoolName(string schoolName)
        {
            By loc =  By.TagName("button");
            WaitForElement(loc);
            new SelectElement(_schoolDropdown).SelectByText(schoolName);
        }

        public void SelectBySchoolValue(string value)
        {
            new SelectElement(_schoolDropdown).SelectByText(value);

        }

        public void SignIn()
        {
            _signInButton.Click();
        }

        public void ValidateElements()
        {
            Assert.IsTrue(_schoolDropdown.Displayed);
        }
    }
}
