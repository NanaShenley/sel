using POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using Facilities.POM.Components.SchoolGroups.Dialog;

namespace Facilities.POM.Components.SchoolGroups.Page
{
    public class CurriculumStructurePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_source_class_button']")]
        private IWebElement _addSourceClassButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_teaching_group_button']")]
        private IWebElement _addTeachingGroupButton;

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schemeName;

        [FindsBy(How = How.Name, Using = "AcademicYearBegins.dropdownImitator")]
        private IWebElement _schemeBeginDropDown;

        [FindsBy(How = How.Name, Using = "AcademicYearEnds.dropdownImitator")] 
        private IWebElement _schemeEndDropDown;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string SchemeName
        {
            set { _schemeName.SetText(value); }
            get { return _schemeName.GetValue(); }
        }

        public string AcademicYearBeginDropDown
        {
            set { _schemeBeginDropDown.EnterForDropDown(value); }
            get { return _schemeBeginDropDown.GetValue(); }
        }

        public string AcademicYearEndDropDown
        {
            set { _schemeEndDropDown.EnterForDropDown(value); }
            get { return _schemeEndDropDown.GetValue(); }
        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public DeleteConfirmationDialog Delete()
        {
            _deleteButton.ClickByJS();
            return new DeleteConfirmationDialog();
        }

        public AddSourceClassDialog ClickAddSourceClass()
        {

            _addSourceClassButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSourceClassDialog();

        }

        public AddTeachingGroupDialog ClickAddTeachingGroup()
        {

            _addTeachingGroupButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddTeachingGroupDialog();

        }

        #endregion

    }
}
