using POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;

namespace Facilities.POM.Components.SchoolGroups.Dialog
{
    public class AddTeachingGroupDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.Name, Using = "DestinationGroup.ShortName")]
        private IWebElement _shortName;

        [FindsBy(How = How.Name, Using = "DestinationGroup.FullName")]
        private IWebElement _fullName;

        [FindsBy(How = How.Name, Using = "DestinationGroup.DisplayOrder")]
        private IWebElement _displayOrder;

        [FindsBy(How = How.Name, Using = "DestinationGroup.AssessmentSubject.dropdownImitator")]
        private IWebElement _subject;

        public string ShortName
        {
            get { return _shortName.GetValue(); }
            set { _shortName.SetText(value); }
        }

        public string FullName
        {
            get { return _fullName.GetValue(); }
            set { _fullName.SetText(value); }
        }

        public string DisplayOrder
        {
            get { return _displayOrder.GetValue(); }
            set { _displayOrder.SetText(value); }
        }

        public string Subject
        {
            set { _subject.EnterForDropDown(value); }
            get { return _subject.GetValue(); }
        }

        #endregion

        #region Action
        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("ok_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion 
    }
}
