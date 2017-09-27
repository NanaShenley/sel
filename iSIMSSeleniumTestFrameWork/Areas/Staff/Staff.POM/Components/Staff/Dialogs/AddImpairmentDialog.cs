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
    public class AddImpairmentDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_impairment_dialog"); }
        }

        #region Page propertise

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.Name, Using = "Impairment")]
        private IWebElement _impairment;

        [FindsBy(How = How.Name, Using = "ImpairmentCategory.dropdownImitator")]
        private IWebElement _impairmentCategory;

        [FindsBy(How = How.Name, Using = "DateAdvised")]
        private IWebElement _adviseDate;

        public string Impairment
        {
            get { return _impairment.GetText(); }
            set { _impairment.SetText(value); }
        }

        public string ImpairmentCategory
        {
            get { return _impairmentCategory.GetText(); }
            set { _impairmentCategory.EnterForDropDown(value); }
        }

        public string AdviseDate
        {
            get { return _adviseDate.GetDateTime(); }
            set { _adviseDate.SetDateTime(value); }
        }

        #endregion

        #region Page actions

        public StaffRecordPage AddImpairment()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffRecordPage();
        }

        #endregion

    }
}
