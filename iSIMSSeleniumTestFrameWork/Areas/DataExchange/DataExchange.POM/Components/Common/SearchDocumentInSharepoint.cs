using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Components.Common
{
    public class SearchDocumentInSharepoint : BaseSeleniumComponents
    {
        public SearchDocumentInSharepoint()
        {
            Thread.Sleep(1000);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        private const string OkButton = "ok_button";

        public IWebElement SearchButtonElement
        {
            get
            {
                WaitUntilEnabled(DataExchangeElements.Import.UploadSearchButton);
                return WebContext.WebDriver.FindElement(DataExchangeElements.Import.UploadSearchButton);
            }
        }

        public IWebElement LopSearchButtonElement
        {
            get
            {
                WaitUntilEnabled(DataExchangeElements.Import.UploadLOPSearchButton);
                return WebContext.WebDriver.FindElement(DataExchangeElements.Import.UploadLOPSearchButton);
            }
        }

        public IWebElement OkButtonElement
        {
            get
            {
                WaitUntilEnabled(SeleniumHelper.SelectByDataAutomationID(OkButton));
                return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID(OkButton));
            }
        }

        public void ClickCtfSearchButton()
        {
            SearchButtonElement.Click();
        }

        public void ClickLopSearchButton()
        {
            LopSearchButtonElement.Click();
        }


        public void SelectFirstRecord()
        {
            WaitUntilEnabled(DataExchangeElements.Import.SearchBtnEnabled);
            SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElements.Import.FirstRecordSelect);
        }

        public void ClickOkButton()
        {
            OkButtonElement.Click();
        }
    }
}
