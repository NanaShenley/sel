using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System.Threading;
using SharedComponents.Helpers;
using SharedServices.Components.Common;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.PageObjects
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
                WaitUntilEnabled(SharedServicesElements.Import.UploadSearchButton);
                return WebContext.WebDriver.FindElement(SharedServicesElements.Import.UploadSearchButton);
            }
        }

        public IWebElement LopSearchButtonElement
        {
            get
            {
                WaitUntilEnabled(SharedServicesElements.Import.UploadLOPSearchButton);
                return WebContext.WebDriver.FindElement(SharedServicesElements.Import.UploadLOPSearchButton);
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
            WaitUntilEnabled(SharedServicesElements.Import.SearchBtnEnabled);
            SeleniumHelper.WaitForElementClickableThenClick(SharedServicesElements.Import.FirstRecordSelect);
        }

        public void ClickOkButton()
        {
            OkButtonElement.Click();
        }
    }
}
