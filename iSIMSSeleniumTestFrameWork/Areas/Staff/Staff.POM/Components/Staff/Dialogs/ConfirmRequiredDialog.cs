using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class ConfirmRequiredDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".modal-content"); }
        }

        #region Properties

        [FindsBy (How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_changes_button']")]
        private IWebElement _saveChangesButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueWithDeleteButton;

        #endregion

        #region Actions

        public void ClickYes()
        {
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SeSugar.Automation.SimsBy.AutomationId("yes_button")));
            AutomationSugar.WaitUntilStale(this.ComponentIdentifier);
            AutomationSugar.WaitForAjaxCompletion();
        }


        public StaffRecordPage ClickOk()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
            try
            {
                if (SeleniumHelper.FindElement(SimsBy.CssSelector("[data-section-id='detail']")).IsElementDisplayed())
                {
                    return new StaffRecordPage();
                }
            }
            catch (NoSuchElementException e)
            {
                return null;
            }
            return null;
        }

        public StaffRecordPage ClickSaveChanges()
        {
            _saveChangesButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
            try
            {
                if (SeleniumHelper.FindElement(SimsBy.CssSelector("[data-section-id='detail']")).IsElementDisplayed())
                {
                    return new StaffRecordPage();
                }
            }
            catch (NoSuchElementException e)
            {
                return null;
            }
            return null;
        }

        public StaffRecordPage ClickContinueWithDelete()
        {
            _continueWithDeleteButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Wait.WaitForAjaxReady(By.Id("nprogress"));
            try
            {
                if (SeleniumHelper.FindElement(SimsBy.CssSelector("[data-section-id='detail']")).IsElementDisplayed())
                {
                    return new StaffRecordPage();
                }
            }
            catch (NoSuchElementException e)
            {
                return null;
            }
            return null;
        }

        #endregion
    }
}
