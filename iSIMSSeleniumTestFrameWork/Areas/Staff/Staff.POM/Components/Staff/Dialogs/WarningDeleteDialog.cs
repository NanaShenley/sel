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
    public class WarningDeleteDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("palette-editor-container"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueDeleteButton;
     
        #endregion

        #region Page actions

        public StaffRecordPage ContinueDeleteStaff()
        {
            StaffRecordPage staffRecordPage = null;
            if (_continueDeleteButton.IsExist())
            {
                _continueDeleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

                staffRecordPage = new StaffRecordPage();
            }

            return staffRecordPage;
        }

        #endregion
    }
}
