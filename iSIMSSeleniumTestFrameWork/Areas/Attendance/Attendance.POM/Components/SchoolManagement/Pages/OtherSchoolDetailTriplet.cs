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
    public class OtherSchoolDetailTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public OtherSchoolDetailTriplet()
        {
            _searchCriteria = new OtherSchoolDetailSearchPage(this);
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        #endregion

        #region Actions

        public OtherSchoolDetailPage Create()
        {
            try
            {
                if (_createButton.IsExist())
                {
                    _createButton.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                    return new OtherSchoolDetailPage();
                }
                else
                {
                    return new OtherSchoolDetailPage();
                }

            }
            catch (Exception) 
            {
                return new OtherSchoolDetailPage(); 
            }
        }

        #endregion

        #region Search

        public class SearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='School Name']")]
            private IWebElement _schoolName;

            public string SchoolName
            {
                get { return _schoolName.GetText().Trim(); }
            }
        }

        private readonly OtherSchoolDetailSearchPage _searchCriteria;

        public OtherSchoolDetailSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }
}
