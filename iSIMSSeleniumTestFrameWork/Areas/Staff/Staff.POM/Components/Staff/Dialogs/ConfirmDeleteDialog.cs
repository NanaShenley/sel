using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
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

        public void Delete()
        {
            Retry.Do(_deleteButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public static ConfirmDeleteDialog Create()
        {
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ConfirmDeleteDialog();
        }

        #endregion
    }
}
