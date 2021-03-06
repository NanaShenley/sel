﻿using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace POM.Components.Pupil
{
    public class PupilContactSearchPage : SearchCriteriaComponent<PupilRecordTriplet.PupilRecordSearchResultTile>
    {
        public PupilContactSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surname;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forename;

        public string Surname
        {
            set { _surname.SetText(value); }
            get { return _surname.GetValue(); }
        }

        public string Forename
        {
            set { _forename.SetText(value); }
            get { return _forename.GetValue(); }
        }

        #endregion

    }
}
