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
    public class ManageUserDefinedTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _createNewButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_error']")]
        private IWebElement _errorMessage;

        
        #endregion

        #region Actions

        public ManageUserDefinedPage ClickCreateNew()
        {
            _createNewButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageUserDefinedPage();
        }


        public bool IsSuccessMessageExist()
        {

            Wait.WaitForControl(SimsBy.AutomationId("status_success"));
            return _successMessage.IsExist();
        }

        public bool IsErrorMessageExist() {

            Wait.WaitForControl(SimsBy.AutomationId("status_error"));
            return _errorMessage.IsExist();
        }

        #endregion

        #region Search

        public class SearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title = 'Group Full Name']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[title = 'Purpose']")]
            private IWebElement _purpose;

            [FindsBy(How = How.CssSelector, Using = "[title = 'Visibility']")]
            private IWebElement _visibility;

            public string GroupName
            {
                get { return _name.Text; }
            }

            public string Purpose
            {
                get { return _purpose.Text; }
            }

            public string Visibility
            {
                get { return _visibility.Text; }
            }

        }

        private readonly ManageUserDefinedSearchPage _searchCriteria;

        public ManageUserDefinedSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

        public ManageUserDefinedTriplet()
        {
            _searchCriteria = new ManageUserDefinedSearchPage(this);
        }
    }
}
