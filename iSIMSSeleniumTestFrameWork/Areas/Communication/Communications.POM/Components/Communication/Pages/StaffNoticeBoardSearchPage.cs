using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using POM.Base;
using POM.Helper;
using Attendance.POM.Components.Communications;

namespace POM.Components.Communications
{
    public class StaffNoticeBoardSearchPage : SearchCriteriaComponent<StaffNoticeBoardTriplet.StaffNoticeBoardSearchResultTile>
    {
        public StaffNoticeBoardSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "TitleOfSearch")]
        private IWebElement _title;


        [FindsBy(How = How.Name, Using = "PostingDate")]
        private IWebElement _postingDate;

        [FindsBy(How = How.Name, Using = "PostingExpiryDate")]
        private IWebElement _postingExpiryDate;


        public string Title
        {
            set { _title.SetText(value); }
            get { return _title.GetValue(); }
        }

        public string StartDate
        {
            set { _postingDate.SetDateTime(value); }
            get { return _postingDate.GetDateTime(); }
        }

        public string EndDate
        {
            set { _postingExpiryDate.SetDateTime(value); }
            get { return _postingExpiryDate.GetDateTime(); }
        }

        #endregion

    }
}
