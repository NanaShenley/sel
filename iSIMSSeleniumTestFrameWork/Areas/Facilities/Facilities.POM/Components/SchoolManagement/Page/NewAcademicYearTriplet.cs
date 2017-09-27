using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.SchoolGroups;
using POM.Helper;


namespace Facilities.POM.Components.SchoolManagement.Page
{
    public class NewAcademicYearTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public NewAcademicYearTriplet()
        {
            _searchCriteria = new AcademicYearSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_academic_year_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='service_navigation_contextual_link_Delete_Latest_Future_Academic_Year']")]
        private IWebElement _deleteLatestAYButton;

        #endregion

        #region Actions

        public NewAcademicYearDetailPage Create()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new NewAcademicYearDetailPage();
        }

        public AcademicYearDeletePage DeleteLatestAY()
        {
            _deleteLatestAYButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AcademicYearDeletePage();
        }

        #endregion

        #region Search

        public class SearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _academicYear;

            public string AcademicYear
            {
                get { return _academicYear.GetText().Trim(); }
            }
        }

        private readonly AcademicYearSearchPage _searchCriteria;

        public AcademicYearSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

        public class AcademicYearSearchPage : SearchCriteriaComponent<NewAcademicYearTriplet.SearchResultTile>
        {
            public AcademicYearSearchPage(BaseComponent parent) : base(parent) { }

            #region Properties

            [FindsBy(How = How.Name, Using = "Name")]
            private IWebElement _academicYearNameTextbox;

            public string AcademicYearName
            {
                set { _academicYearNameTextbox.SetText(value); }
            }


            #endregion
        }
    }
}