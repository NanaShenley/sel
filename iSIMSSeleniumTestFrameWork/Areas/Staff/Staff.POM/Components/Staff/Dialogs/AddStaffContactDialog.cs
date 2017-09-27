using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;

using System.Threading;
using SeSugar.Automation;
namespace Staff.POM.Components.Staff
{
    public class AddStaffContactDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_contact_palette_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _titleDropdown;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreNameTextBox;

        [FindsBy(How = How.Name, Using = "MiddleName")]
        private IWebElement _middleNameTextBox;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surNameTextBox;

        public string Title
        {
            get { return _titleDropdown.GetAttribute("value"); }
            set { _titleDropdown.EnterForDropDown(value); }
        }

        public string ForeName
        {
            get { return _foreNameTextBox.GetText(); }
            set { _foreNameTextBox.SetText(value); }
        }

        public string MiddleName
        {
            get { return _middleNameTextBox.GetText(); }
            set { _middleNameTextBox.SetText(value); }
        }

        public string SurName
        {
            get { return _surNameTextBox.GetText(); }
            set { _surNameTextBox.SetText(value); }
        }

        #endregion
    }
}
