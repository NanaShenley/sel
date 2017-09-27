using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.POM.Components.Census
{
    public class CensusDetailsSection
    {

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='']")]
        private IWebElement _censusDetailSection;

        [FindsBy(How = How.Name, Using = "CensusDate")]
        private IWebElement _censusDate;

        [FindsBy(How = How.Name, Using = "ReturnDesc")]
        private IWebElement _returnDesc;

        [FindsBy(How = How.Name, Using = "AttendanceCollectedFrom")]
        private IWebElement _attendanceCollectedFrom;

        [FindsBy(How = How.Name, Using = "AttendanceCollectedTo")]
        private IWebElement _attendanceCollectedTo;

        [FindsBy(How = How.Name, Using = "ExclusionsCollectedFrom")]
        private IWebElement _exclusionsCollectedFrom;

        [FindsBy(How = How.Name, Using = "ExclusionsCollectedTo")]
        private IWebElement _exclusionsCollectedTo;

        [FindsBy(How = How.Name, Using = "FSMCollectedFrom")]
        private IWebElement _fSMCollectedFrom;

        [FindsBy(How = How.Name, Using = "FSMCollectedTo")]
        private IWebElement _fSMCollectedTo;

        [FindsBy(How = How.Name, Using = "LearnerSupportCollectedFrom")]
        private IWebElement _learnerSupportCollectedFrom;

        [FindsBy(How = How.Name, Using = "LearnerSupportCollectedTo")]
        private IWebElement _learnerSupportCollectedTo;

        public CensusDetailsSection()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
        public IWebElement CensusDetail
        {
            get
            {
                return _censusDetailSection;
            }
        }
        public IWebElement CensusDate
        {
            get
            {
                return _censusDate;
            }
        }
        public IWebElement ReturnDesc
        {
            get
            {
                return _returnDesc;
            }
        }
        public IWebElement AttendanceCollectedFrom
        {
            get
            {
                return _attendanceCollectedFrom;
            }
        }
        public IWebElement AttendanceCollectedTo
        {
            get
            {
                return _attendanceCollectedTo;
            }
        }
        public IWebElement ExclusionsCollectedFrom
        {
            get
            {
                return _exclusionsCollectedFrom;
            }
        }
        public IWebElement ExclusionsCollectedTo
        {
            get
            {
                return _exclusionsCollectedTo;
            }
        }

        public IWebElement FSMCollectedFrom
        {
            get
            {
                return _fSMCollectedFrom;
            }
        }

        public IWebElement FSMCollectedTo
        {
            get
            {
                return _fSMCollectedTo;
            }
        }

        public IWebElement LearnerSupportCollectedFrom
        {
            get
            {
                return _learnerSupportCollectedFrom;
            }
        }
        
        public IWebElement LearnerSupportCollectedTo
        {
            get
            {
                return _learnerSupportCollectedTo;
            }
        }

        /// <summary>
        /// Check if Section Exist
        /// </summary>
        /// <returns></returns>
        public bool IsSectionExist()
        {
            return CensusDetail.IsElementDisplayed();
        }

        /// <summary>
        /// Open On section
        /// </summary>
        public void OpenSection()
        {
            if (CensusDetail.IsElementDisplayed())
            {
                CensusDetail.Click();
            }
        }

        /// <summary>
        /// This method returns true if all required Parameters exits and has default values
        /// </summary>
        /// <returns></returns>
        public bool HasRecords()
        {
            if(!(string.IsNullOrEmpty(CensusDate.GetValue())) &&
                !(string.IsNullOrEmpty(ReturnDesc.GetValue())) &&
                !(string.IsNullOrEmpty(AttendanceCollectedFrom.GetValue())) && 
                !(string.IsNullOrEmpty(AttendanceCollectedTo.GetValue())) && 
                !(string.IsNullOrEmpty(ExclusionsCollectedFrom.GetValue())) && 
                !(string.IsNullOrEmpty(ExclusionsCollectedTo.GetValue())) && 
                !(string.IsNullOrEmpty(FSMCollectedFrom.GetValue())) && 
                !(string.IsNullOrEmpty(FSMCollectedTo.GetValue())) && 
                !(string.IsNullOrEmpty(LearnerSupportCollectedFrom.GetValue())) &&
                !(string.IsNullOrEmpty(LearnerSupportCollectedTo.GetValue())))
            {
                return true;
            }
            else
                return false;
           
        }
    }
}
