using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
{
    public class FindTrainingCourseEventDetailsDialog : BaseDialogComponent
    {       
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _addCourseButton;

        #endregion

        #region Public methods

        public StaffRecordPage AddCourseButton()
        {
            _addCourseButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        #endregion

       
    }
}
