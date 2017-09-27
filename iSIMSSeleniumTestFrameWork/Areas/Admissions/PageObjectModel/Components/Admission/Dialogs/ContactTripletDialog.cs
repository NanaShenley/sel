using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using PageObjectModel.Base;
using PageObjectModel.Helper;


namespace PageObjectModel.Components.Admission
{
    public class ContactTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("pupil_contact_record_triplet"); }
        }

        public ContactTripletDialog()
        {
            _searchCriteria = new ContactSearchDialog(this);
        }

        #region Search

        public class ContactSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly ContactSearchDialog _searchCriteria;
        public ContactSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;

        #endregion

        #region Actions

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>

        public void SelectSearchTile(ContactSearchResultTile contactTile)
        {
            if (contactTile != null)
            {
                contactTile.Click();
            }
        }

        public ContactDialog Create()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new ContactDialog();
        }

        public ApplicationPage OK()
        {
            if (_okButton.IsExist())
            {
                _okButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Wait.WaitForControl(SimsBy.CssSelector("[data-automation-id='status_success']"));
            Refresh();
        }

        public ApplicationPage Cancel()
        {
            if (_cancelButton.IsExist())
            {
                _cancelButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            }
            return new ApplicationPage();
            Refresh();
        }

        #endregion
    }

    public class ContactSearchDialog : SearchCriteriaComponent<ContactTripletDialog.ContactSearchResultTile>
    {
        public ContactSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _sureName;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _foreName;

        public string SearchBySureName
        {
            set { _sureName.SetText(value); }
            get { return _sureName.GetValue(); }
        }
        public string SearchByForeName
        {
            set { _foreName.SetText(value); }
            get { return _foreName.GetValue(); }
        }
        #endregion
    }
}
