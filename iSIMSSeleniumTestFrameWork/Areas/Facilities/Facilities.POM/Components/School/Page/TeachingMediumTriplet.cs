using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.School
{
    public class TeachingMediumTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("allocate_future_pupil_main_id"); }
        }

        public TeachingMediumTriplet()
        {
            _searchCriteria = new TeachingMediumSearchPage(this);
        }

        #region Search

        private readonly TeachingMediumSearchPage _searchCriteria;

        public TeachingMediumSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion

    }

}
