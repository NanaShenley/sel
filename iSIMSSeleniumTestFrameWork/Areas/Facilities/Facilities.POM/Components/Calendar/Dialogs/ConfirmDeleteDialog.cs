using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;

namespace Facilities.POM.Components.Calendar.Dialogs
{
    public class ConfirmDeleteDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".modal-dialog"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Actions

        public void ContinueWithDelete()
        {
            Retry.Do(_deleteButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public bool DeleteSingleEvent
        {
            set
            {
                string javascript = "document.getElementsByName('EventDeleteType')[0].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }
        public bool DeleteEventSeries
        {
            set
            {
                string javascript = "document.getElementsByName('EventDeleteType')[1].checked = '{0}';";
                SeleniumHelper.ExecuteJavascript(String.Format(javascript, value));
            }
        }

        

        #endregion
    }
}
