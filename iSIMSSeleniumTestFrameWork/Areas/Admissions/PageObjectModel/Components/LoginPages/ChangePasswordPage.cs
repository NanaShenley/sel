using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;

namespace PageObjectModel.Components.LoginPages
{
    public class ChangePasswordPage
    {
#pragma warning disable 0649

        // ReSharper disable UnusedField.Compiler
        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.Id, Using = "CurrentPassword")]
        private IWebElement _currentPassword;

        [FindsBy(How = How.Id, Using = "Password")]
        private IWebElement _password;

        [FindsBy(How = How.Id, Using = "ConfirmPassword")]
        private IWebElement _confirmPassword;

        [FindsBy(How = How.LinkText, Using = "Cancel")]
        private IWebElement _cancel;

        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        private IWebElement _change;
        // ReSharper restore UnusedField.Compiler
        // ReSharper restore UnassignedField.Compiler
#pragma warning restore 0649


        public ChangePasswordPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }


        //public void ValidateElements()
        //{
        //    Assert.IsTrue(_currentPassword.Displayed);
        //    Assert.IsTrue(_password.Displayed);
        //    Assert.IsTrue(_confirmPassword.Displayed);
        //    Assert.IsTrue(_cancel.Displayed);
        //    Assert.IsTrue(_change.Displayed);
        //}
    }
}
