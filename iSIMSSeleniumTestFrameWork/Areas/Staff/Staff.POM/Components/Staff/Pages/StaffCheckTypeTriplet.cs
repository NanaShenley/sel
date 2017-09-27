﻿using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff
{
    public class StaffCheckTypeTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public StaffCheckTypeTriplet()
        {
            _searchCriteria = new StaffCheckTypeSearchPage(this);
        }

        #region Search

        private readonly StaffCheckTypeSearchPage _searchCriteria;

        public StaffCheckTypeSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        #endregion
    }

    public class StaffCheckTypeSearchPage : SearchTableCriteriaComponent
    {
        public StaffCheckTypeSearchPage(BaseComponent context) : base(context) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _codeOrDescription;

        public string CodeOrDescription
        {
            set { _codeOrDescription.SetText(value); }
        }

        #endregion
    }
}