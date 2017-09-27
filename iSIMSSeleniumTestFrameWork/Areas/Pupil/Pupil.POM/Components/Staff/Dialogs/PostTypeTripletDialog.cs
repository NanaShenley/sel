using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Staff
{
    public class PostTypeTripletDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("service_terms_post_types_dialog"); }
        }

        public PostTypeTripletDialog()
        {
            _searchCriteria = new PostTypeSearchDialog(this);
        }

        #region Search

        public class PostTypeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Code']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        private readonly PostTypeSearchDialog _searchCriteria;
        public PostTypeSearchDialog SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Page Actions

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='cancel_button']")]
        private IWebElement _cancelButton;


        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>

        public void SelectSearchTile(PostTypeSearchResultTile postTypeTile)
        {
            if (postTypeTile != null)
            {
                postTypeTile.Click();
            }
        }

        public void ClickOK()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        public void Cancel()
        {
            _cancelButton.Click();
        }

        #endregion
    }

    public class PostTypeSearchDialog : SearchCriteriaComponent<PostTypeTripletDialog.PostTypeSearchResultTile>
    {
        public PostTypeSearchDialog(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Code")]
        private IWebElement _code;

        public string SearchByCode
        {
            set { _code.SetText(value); }
            get { return _code.GetValue(); }
        }

        #endregion
    }


}
