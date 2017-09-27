using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class AddStaffContactTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("staff_contact_palette_triplet"); }
        }

        public AddStaffContactTripletDialog()
        {
            _searchCriteria = new AddStaffContactSearchDialog(this);
        }

        #region Search
        public class StaffContactSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }

        private readonly AddStaffContactSearchDialog _searchCriteria;
        public AddStaffContactSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Page Action

        public AddStaffContactDialog CreateContact()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStaffContactDialog();
        }

        #endregion
    }
}
