using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;

namespace Staff.Components.StaffRegression
{
    public class StaffSearch : SearchCriteriaComponent<StaffRecordTriplet.StaffRecordSearchResultTile>
    {
        public StaffSearch(BaseComponent parent) : base(parent)
        {
        }

        [FindsBy(How = How.Name, Using = "tri_chkbox_StatusCurrentCriterion")]
        private IWebElement _statusCurrentCriterion;

        public bool StatusCurrentCriterion
        {
            set { _statusCurrentCriterion.SetCheckBox(value); }
            get { return _statusCurrentCriterion.IsCheckboxChecked(); }
        }
    }
}
