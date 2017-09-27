using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolGroups
{
    public class SelectEffectiveDateRangeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("#dialog-palette-editor .modal-content"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "AcademicYear.dropdownImitator")]
        private IWebElement _academicYearDropdown;

        [FindsBy(How = How.Name, Using = "EffectiveStartDate")]
        private IWebElement _fromTextbox;

        [FindsBy(How = How.Name, Using = "EffectiveEndDate")]
        private IWebElement _toTextbox;

        public string AcademicYear
        {
            set
            {
                _academicYearDropdown.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
        }

        public string From
        {
            set { _fromTextbox.SetDateTime(value); }
            get
            {
                return _fromTextbox.GetDateTime();
            }
        }

        public string To
        {
            set { _toTextbox.SetDateTime(value); }
            get { return _toTextbox.GetDateTime(); }
        }

        #endregion

    }
}
