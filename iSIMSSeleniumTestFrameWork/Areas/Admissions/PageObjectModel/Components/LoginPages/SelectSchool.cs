using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using PageObjectModel.Helper;
using WebDriverRunner.webdriver;

namespace PageObjectModel.Components.LoginPages
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
            By loc = By.TagName("button");
            Wait.WaitForElement(loc);
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

        //public void ValidateElements()
        //{
        //    Assert.IsTrue(_schoolDropdown.Displayed);
        //}
    }
}
