using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.SchoolGroups
{
    public class AddSupervisorsDialogSearchPage : SearchTableCriteriaComponent
    {
       
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='search_criteria']"); }
        }

        public AddSupervisorsDialogSearchPage(BaseComponent parent) : base(parent) 
        {
         
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "StaffCode")]
        private IWebElement _staffCode;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurName;

        [FindsBy(How = How.Name, Using = "StaffRole.dropdownImitator")]
        private IWebElement _staffRole;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surNameTextbox;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreNameTextbox;

        [FindsBy(How = How.Name, Using = "Role.dropdownImitator")]
        private IWebElement _roleDropdown;

        public string LegalSurName
        {
            set { _legalSurName.SetText(value); }
            get { return _legalSurName.GetValue(); }
        }

        public string StaffRole
        {
            set { _staffRole.EnterForDropDown(value); }
            get { return _staffRole.GetValue(); }
        }

        public string SurName
        {
            set { _surNameTextbox.SetText(value); }
        }

        public string ForeName
        {
            set { _foreNameTextbox.SetText(value); }
        }

        public string Role
        {
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _roleDropdown.EnterForDropDown(value); 
                }
                else
                {
                    IWebElement parent = SeleniumHelper.FindElement(SimsBy.Id("dialog-palette-editor"));
                    IWebElement clearDropdown = parent.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));
                    clearDropdown.Click();
                }
                
            }
        }
        #endregion

    }
}
