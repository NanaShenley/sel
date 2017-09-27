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
    public class BandSchemePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Page properties
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_year_group_button']")]
        private IWebElement _addYearGroupButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_structural_group_button']")]
        private IWebElement _addStructuralGroupButton;

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _bandSchemeName;

        [FindsBy(How = How.Name, Using = "AcademicYearBegins.dropdownImitator")]
        private IWebElement _academicYearDropDown;
        

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        public static readonly By ValidationWarning = By.CssSelector("[data-automation-id='status_error']");

        #endregion

        #region Action

        public string BandSchemeName
        {
            set { _bandSchemeName.SetText(value); }
            get { return _bandSchemeName.GetValue(); }
        }

        public string AcademicYearDropDown
        {
            set { _academicYearDropDown.EnterForDropDown(value); }
            get { return _academicYearDropDown.GetValue(); }
        }
        
        
        public bool IsSuccessMessageDisplayed()
        {
            return _successMessage.IsExist();
        }

        public AddStructuralGroupDialog ClickAddTeachingGroup()
        {

            _addStructuralGroupButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStructuralGroupDialog();

        }

        public AddSourceYearGroupDialog ClickAddSourceClass()
        {
            _addYearGroupButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSourceYearGroupDialog();

        }
        #endregion

    }
}
