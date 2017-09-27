using System;
using System.Runtime.InteropServices;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AllDocumentsComponent
{
    public class DocumentLink:BaseSeleniumComponents
    {

        private const string MoreTabsLink="[data-section-menu-more]";

        private const string DocumentAccordLink = "staff_record_dropdown_accordion";


        [FindsBy(How = How.CssSelector, Using = DocumentAccordLink)] 
        public IWebElement _DocumentLinkElement;


        [FindsBy(How = How.CssSelector, Using = MoreTabsLink)]
        public IWebElement _MoretabsLinkElement;

        public IWebElement MoreTabsElement
        {
            get { return WebContext.WebDriver.FindElement(By.CssSelector(MoreTabsLink)); }
        }


        public IWebElement Document
        {
            get { return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("staff_record_dropdown_accordion")); }
        }

        public void ClicDocumentLink()
        {
            WaitForAndClick(TimeSpan.FromSeconds(100),By.CssSelector(SeleniumHelper.AutomationId(DocumentAccordLink)));
            Document.Click();
        }
    }
}
