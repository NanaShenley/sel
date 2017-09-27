using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using Staff.POM.Helper;

namespace Staff.POM.Components.LoginPages
{
    public class SelectTenantPage //: BaseSeleniumComponents
    {
        #pragma warning disable 0649
        
        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.Id, Using = "TenantID")]
        private IWebElement _tenant;

        [FindsBy(How = How.CssSelector, Using = "button[type='submit']")]
        private IWebElement _next;

        [FindsBy(How = How.LinkText, Using = "Change your password")]
        private IWebElement _changePassword;

        [FindsBy(How = How.LinkText, Using = "Sign in with a different account")]
        private IWebElement _changeAccount;

        [FindsBy(How = How.ClassName, Using = "text-danger")]
        public IWebElement Error;

        // ReSharper restore UnassignedField.Compiler
        #pragma warning restore 0649

        public SelectTenantPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public object ErrorText { get; set; }

        public SelectTenantPage EnterTenant(string tenantId)
        {
            Wait.WaitForElement(By.Id("TenantID"));
            _tenant.SendKeys(tenantId);
            return this;
        }

        public void Submit()
        {
            _next.Click();
        }

        //public void ValidateElements()
        //{
        //    Assert.IsTrue(_tenant.Displayed);
        //    Assert.IsTrue(_next.Displayed);
        //    Assert.IsTrue(_changePassword.Displayed);
        //    Assert.IsTrue(_changeAccount.Displayed);
        //}
    }
}
