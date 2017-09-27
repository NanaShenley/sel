using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace POM.Components.SchoolManagement
{
    public class OtherSchoolDetailSearchPage : SearchCriteriaComponent<OtherSchoolDetailTriplet.SearchResultTile>
    {
        public OtherSchoolDetailSearchPage(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _schoolNameTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_advanced']")]
        private IWebElement _advancedButton;

        [FindsBy(How = How.Name, Using = "AddressTown")]
        private IWebElement _townTextbox;

        public string SchoolName
        {
            set { _schoolNameTextbox.SetText(value); }
        }

        public string Town
        {
            set { _townTextbox.SetText(value); }
        }

        #endregion

        #region Actions

        public void ClickShowMore()
        {
            IWebElement element = SeleniumHelper.FindElement(SimsBy.Id("ShowAdvanced"));
            if (!element.GetValue().Trim().Equals("True"))
            {
                _advancedButton.ClickByJS();
            }
        }

        #endregion
    }
}
