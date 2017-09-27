using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace POM.Components.SchoolManagement
{
    public class AcademicYearSearchPage : SearchCriteriaComponent<AcademicYearTriplet.SearchResultTile>
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

        #region Actions

        public AcademicYearDetailPage ClickAcademicYear(string academicYearName)
        {
            //academicYearName.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AcademicYearDetailPage();
        }

        #endregion
    }
}
