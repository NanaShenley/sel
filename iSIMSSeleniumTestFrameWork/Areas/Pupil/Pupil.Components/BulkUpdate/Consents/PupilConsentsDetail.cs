using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;

namespace Pupil.Components.BulkUpdate.Consents
{
    /// <summary>
    /// Page object for bulk update pupil consents detail area
    /// </summary>
    public class PupilConsentsDetail: BaseSeleniumComponents
    {

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(30));

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='check-box-Active']")]
        private IWebElement _activeBulkSelectAll;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-Active']")]
        private IWebElement _activeBulkSelectAllApply;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio']")]
        private IWebElement _consentTypeDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Photograph']")]
        private IWebElement _photographsConsent;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='2'] span")]
        private IWebElement _active;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='2'] [class='fa fa-sort-desc fa-fw']")]
        private IWebElement _activeSelectColFloodFiller;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='2'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']")]
        private IWebElement _activeFloodFiller;

        [FindsBy(How = How.CssSelector, Using = "div[data-menu-column-id='_Active'] input[type='checkbox'][data-automation-id='check-box-Active']")]
        private IWebElement _activeSelectFloodFillerCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-Active']")]
        private IWebElement _activeFloodFillerApply;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='3'] span")] 
        private IWebElement _consentStatus;

        //  [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='3'] [class='fa fa-angle-down fa-fw']")]
        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='3'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']")]
        private IWebElement _consentStatusFloodFiller;

        [FindsBy(How = How.CssSelector, Using = "div[data-menu-column-id='_ConsentStatus'] input[type='checkbox']")]
        private IWebElement _consentStatusFloodFillerCheckBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-ConsentStatus']")]
        private IWebElement _consentStatusFloodFillerApply;       

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='4'] span")]
        private IWebElement _consentDate;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_ss_header'] td[column='4'] [class='fa fa-caret-down fa-fw high-volume-grid-spreadsheet-menu']")]
        private IWebElement _consentDateFloodFiller;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='floodfill-selected-ConsentDate']")]
        private IWebElement _consentDateFloodFillerApply;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save'] div[inline-alert-title]")]
        private IWebElement _CheckStatus;

        [FindsBy(How = How.TagName, Using = "strong")]
        private IList<IWebElement> _status;

        /// <summary>
        /// Constructor to initialise the page area
        /// </summary>
        public PupilConsentsDetail()
        {            
            PageFactory.InitElements(WebContext.WebDriver, this);
            WaitForElement(PupilBulkUpdateElements.BulkUpdate.Detail.Consents.ConsentTypeDropDownButton);
            WaitUntillAjaxRequestCompleted();
        }

        public PupilConsentsDetail SelectConsentType(string typeDescription)
        {
            WaitUntillAjaxRequestCompleted();
            _consentTypeDropDown.Click();
            WaitUntillAjaxRequestCompleted();
            var consentType =
                WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId(typeDescription)));
            WaitUntillAjaxRequestCompleted();
            consentType.Click();
 
            return this;
        }

        public PupilConsentsDetail WithFloodFillActiveAs(bool state)
        {
            WaitUntillAjaxRequestCompleted();
            _active.Click();
            WaitUntillAjaxRequestCompleted();
            _activeFloodFiller.Click();


            WaitUntillAjaxRequestCompleted();

            if (state)
                {
                    _activeBulkSelectAll.Click();
                }
                else
                {
                     _activeBulkSelectAll.Click();
                     _activeBulkSelectAll.Click();
                }
            WaitUntillAjaxRequestCompleted();
            _activeBulkSelectAllApply.Click();

            return this;
        }

        public PupilConsentsDetail WithFloodFillConsentStatusAs(string status)
        {
            WaitUntillAjaxRequestCompleted();
            _consentStatus.Click();
            WaitUntillAjaxRequestCompleted();
            _consentStatusFloodFiller.Click();
            WaitUntillAjaxRequestCompleted();
            SetConsentStatusDropDown(status);
            WaitUntillAjaxRequestCompleted();
            var checkedValue = _consentStatusFloodFillerCheckBox.GetAttribute("checked");
            if (checkedValue == null)
                _consentStatusFloodFillerCheckBox.Click();

            _consentStatusFloodFillerApply.Click();

            return this;
        }

        public PupilConsentsDetail WithFloodFillConsentDateAsDefault()
        {
            WaitUntillAjaxRequestCompleted();
            _consentDate.Click();
            WaitUntillAjaxRequestCompleted();
            _consentDateFloodFiller.Click();
            WaitUntillAjaxRequestCompleted();
            _consentDateFloodFillerApply.Click();

            return this;
        }

        private void SetConsentStatusDropDown(string status)
        {
            var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            WaitLogic(wait, By.CssSelector("[name='ConsentStatus']"));
            var selectElement = WebContext.WebDriver.FindElement(By.CssSelector("[name='ConsentStatus']"));
            var mySelect = new SelectElement(selectElement);
            mySelect.SelectByText(status);
        }

        public PupilConsentsDetail Save()
        {
            this.HasSavedSuccessfully = false;
            try
            {
                Actions actions = new Actions(WebContext.WebDriver);
                var saveBtnLocator = By.CssSelector("[data-automation-id='well_know_action_save']");
                actions.MoveToElement(SeleniumHelper.Get(saveBtnLocator)).Perform();
                SeleniumHelper.WaitForElementClickableThenClick(saveBtnLocator);

                var saveSuccessLocator = By.CssSelector(SeleniumHelper.AutomationId("status_success"));
                waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessLocator));
                HasSavedSuccessfully = true;
            }
            catch (Exception e)
            {
                HasSavedSuccessfully = false;
                //throw e ;
            }

            return this;
        }

        public bool HasSavedSuccessfully { get; set;}
    }
}
