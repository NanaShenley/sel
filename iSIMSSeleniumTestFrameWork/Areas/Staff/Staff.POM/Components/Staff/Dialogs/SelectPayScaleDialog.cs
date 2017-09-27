﻿using System;
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
    public class SelectPayScaleDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("#dialog-dialog-palette-editor"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "PayScale.dropdownImitator")]
        private IWebElement _scale;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _addPayScaleButton;

        [FindsBy(How = How.Name, Using = "Point")]
        private IWebElement _point;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDate;
        
        public string ScaleField
        {
            set
            {
                _scale.EnterForDropDown(value);
                AutomationSugar.WaitUntilStale(By.Name("Point"));
                Refresh();
            }
            get { return _scale.GetText(); }
        }

        public string PointField
        {
            set
            {
                _point.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            get { return _point.GetText(); }
        }

        public string StartField
        {
            set
            {
                _startDate.SetText(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            get { return _startDate.GetText(); }
        }

        #endregion

        #region Actions

        public StaffRecordPage AddPayScale()
        {            
            Retry.Do(_addPayScaleButton.Click);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Wait.WaitLoading();
            return new StaffRecordPage();
        }        
        #endregion
    }
}
