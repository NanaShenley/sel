using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilities.POM.Components.Calendar.Page
{
    public class ManageCalendarTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ManageCalendarTriplet()
        {
            this.Refresh();
            _searchCriteria = new ManageCalendarSearchPage(this);
        }

        #region Search

        public class ManageCalendarSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly ManageCalendarSearchPage _searchCriteria;
        public ManageCalendarSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_button']")]
        private IWebElement _CreateButton;
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public ManageCalendarDetailPage Create()
        {
            //SeleniumHelper.Get(SimsBy.AutomationId("create_button")).ClickByJS();
            _CreateButton.ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ManageCalendarDetailPage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Select Class if class is existing
        /// </summary>

        public void SelectSearchTile(ManageCalendarSearchResultTile schemeTile)
        {
            if (schemeTile != null)
            {
                schemeTile.Click();

            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new ConfirmDeleteDialog();
                confirmDialog.ClickContinueDelete();
            }
        }

        #endregion
    }

    public class ManageCalendarSearchPage : SearchCriteriaComponent<ManageCalendarTriplet.ManageCalendarSearchResultTile>
    {
        public ManageCalendarSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _calendarName;

        [FindsBy(How = How.Name, Using = "IncludeInactiveCalendars")]
        private IWebElement _includeInActiveCalendarCheckBox;

        public string SearchBySchemeName
        {
            set { _calendarName.SetText(value); }
            get { return _calendarName.GetValue(); }
        }

        public bool InactiveCalendarSearch
        {
            get { return _includeInActiveCalendarCheckBox.IsChecked(); }
            set { _includeInActiveCalendarCheckBox.Set(value); }
        }

        #endregion Search
    }
}
