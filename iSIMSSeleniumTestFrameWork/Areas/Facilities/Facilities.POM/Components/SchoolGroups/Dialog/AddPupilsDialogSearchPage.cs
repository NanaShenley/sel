using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.SchoolGroups
{
    public class AddPupilsDialogSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='search_criteria']"); }
        }

        public AddPupilsDialogSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _pupilName;

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _searchPupilName;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _yearGroupDropDown;

        [FindsBy(How = How.Name, Using = "PrimaryClass.dropdownImitator")]
        private IWebElement _classDropDown;

        [FindsBy(How = How.Name, Using = "StatusCurrentCriterion")]
        private IWebElement _isCurrentCheckbox;

        [FindsBy(How = How.Name, Using = "StatusFutureCriterion")]
        private IWebElement _isFutureCheckbox;

        [FindsBy(How = How.Name, Using = "StatusFormerCriterion")]
        private IWebElement _isLeaverCheckbox;

        public string PupilName
        {
            set { _pupilName.SetText(value); }
            get { return _pupilName.GetValue(); }
        }

        public string SearchPupilName
        {
            set { _searchPupilName.SetText(value); }
        }

        public string YearGroup
        {
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _yearGroupDropDown.EnterForDropDown(value); 
                }
                else
                {
                    IWebElement parent = SeleniumHelper.FindElement(SimsBy.Id("dialog-palette-editor"));
                    IWebElement clearDropdown = parent.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));

                    clearDropdown.Click();
                }
            }
            get { return _yearGroupDropDown.GetValue(); }
        }

        public string Class
        {
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _classDropDown.EnterForDropDown(value);
                }
                else
                {
                    IWebElement parent = SeleniumHelper.FindElement(SimsBy.Id("dialog-palette-editor"));
                    IWebElement clearDropdown = parent.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));
                    clearDropdown.Click();
                }
            }
            get { return _classDropDown.GetValue(); }
        }

        public bool IsCurrent
        {
            set { _isCurrentCheckbox.Set(value); }
        }

        public bool IsFuture
        {
            set { _isFutureCheckbox.Set(value); }
        }

        public bool IsLeaver
        {
            set { _isLeaverCheckbox.Set(value); }
        }


        #endregion

    }
}
