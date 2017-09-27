using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolManagement
{
    public class AcademicYearTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public AcademicYearTriplet()
        {
            _searchCriteria = new AcademicYearSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Actions

        public AcademicYearDetailPage Create()
        {
            _createButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AcademicYearDetailPage();
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
    }
}
