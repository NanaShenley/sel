using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;

namespace POM.Components.SchoolGroups
{
    public class ManageUserDefinedSearchPage : SearchCriteriaComponent<ManageUserDefinedTriplet.SearchResultTile>
    {
        public ManageUserDefinedSearchPage(BaseComponent component) : base(component) { }

        #region Properties
        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _groupFullNameTextbox;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _groupShortNameTextbox;

        [FindsBy(How = How.Name, Using = "UserDefinedGroupPurpose.dropdownImitator")]
        private IWebElement _purposeDropdown;

        [FindsBy(How = How.Name, Using = "IsVisibleTemp")]
        private IWebElement _visibilityCheckbox;

        public string FullName
        {
            set { _groupFullNameTextbox.SetText(value); }
        }

        public string ShortName
        {
            set { _groupShortNameTextbox.SetText(value); }
        }

        public string Purpose
        {
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _purposeDropdown.EnterForDropDown(value); 
                }
                else
                {
                    IWebElement clearDropdown = SeleniumHelper.FindElement(SimsBy.CssSelector(".select2-search-choice-close"));
                    clearDropdown.Click();
                }
                
            }
        }

        public bool Visibility
        {
            set { _visibilityCheckbox.Set(value); }
        }

        #endregion
    }
}
