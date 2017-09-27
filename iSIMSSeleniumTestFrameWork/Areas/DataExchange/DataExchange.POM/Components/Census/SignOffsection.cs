using DataExchange.Components.Common;
using DataExchange.POM.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.Census
{
    public class SignOffsection
    {        
        public SignOffsection()
        {                               
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Sign Off']")]
        private IWebElement _signOffDetailSection;

        [FindsBy(How = How.Name, Using = "SignOffSection.AdditionalText")]
        private IWebElement _additionalText;
               

        public IWebElement SignOffSectionDetail
        {
            get { return _signOffDetailSection; }
        }

        public IWebElement AdditionalTextBox
        {
            get { return _additionalText; }
        }

        //check if Sign Off button exists after authorising a return
        private IWebElement SignOffButtoncheck
        {
            get
            {
                Wait.WaitUntilDisplayed(SimsBy.AutomationId(DataExchangeElement.SignOffButton));
                return SeleniumHelper.Get(SimsBy.AutomationId(DataExchangeElement.SignOffButton));
            }
        }

        /// <summary>
        /// Open sign off section
        /// </summary>
        public void OpenSection()
        {
            By loc = SimsBy.AutomationId("section_menu_Sign Off");
            Wait.WaitUntilDisplayed(loc);          
            Assert.IsNotNull(SignOffSectionDetail);
            if (SignOffSectionDetail.IsElementDisplayed())
            {
                SignOffSectionDetail.Click();
            }
        }

        //This will return true if Sign Off section exists      
        public bool HasSectiondisplayed()
        {
            if (AdditionalTextBox.Displayed || SignOffButtoncheck.Displayed)
                return true;
            return false;
        }

    }
}
