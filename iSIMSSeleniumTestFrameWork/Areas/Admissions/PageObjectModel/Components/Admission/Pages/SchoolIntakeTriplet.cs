using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using PageObjectModel.Base;
using PageObjectModel.Helper;
using System;
using System.Threading;
using WebDriverRunner.webdriver;



namespace PageObjectModel.Components.Admission
{
    public class SchoolIntakeTriplet : BaseComponent
    {
        #region Properties
        private static By saveSuccessMessage_New = By.CssSelector("div[data-automation-id='status_success']");      
        private static By Name = By.CssSelector("input[name='IntakeName']");
        private static By Add_Button = By.CssSelector(SeleniumHelper.AutomationId("add_new_group_button"));        
        private static By Admission_Year = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='YearOfAdmission.dropdownImitator']");
        private static By Admission_Term = By.Name("AdmissionTerm.dropdownImitator");
        private static By Admission_YearGroup  = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='YearGroup.dropdownImitator']");
        private static By Planned_Admission_Number = By.Name("PlannedAdmissionNumber");
        private static By Add_Admission_Group = By.CssSelector(SeleniumHelper.AutomationId("add_admission_group_button"));
        private static By Admission_Date = By.CssSelector("input[name$='.DateOfAdmission'");
        private static By Admission_Name = By.CssSelector("div[data-section-id='IntakeDetailsDependenciesSection'] [name='IntakeName']");
        private static By Admission_Capacity = By.CssSelector("input[name$='.Capacity'");            
        private static By Save_Button = By.CssSelector(SeleniumHelper.AutomationId("well_know_action_save"));
        private static By Copy_Button = By.CssSelector(SeleniumHelper.AutomationId("create_SchoolIntakeCopy"));               
        #endregion
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public SchoolIntakeTriplet()
        {
            _searchCriteria = new SchoolIntakeSearchPage(this);
        }

        #region Search

        private readonly SchoolIntakeSearchPage _searchCriteria;
        public SchoolIntakeSearchPage SearchCriteria { get { return _searchCriteria; } }
             
        public class SchoolIntakeSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
            private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='create_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;
        //private object waiter;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public SchoolIntakePage Create()
        {
            SeleniumHelper.Get(Add_Button).Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new SchoolIntakePage();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing School intake
        /// </summary>

        public void SelectSearchTile(SchoolIntakeSearchResultTile schoolIntakeTile)
        {
            if (schoolIntakeTile != null)
            {
                schoolIntakeTile.Click();
            }
        }

        public void Delete()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.ConfirmDelete();
            }
        }
        public SchoolIntakePage CancelDeleteSchoolIntake()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                var confirmDialog = new DeleteConfirmationPage();
                confirmDialog.CancelDelete();
            }
            return new SchoolIntakePage();
        }

        
        /// <summary>
        /// Create SchoolIntake with randon postfix clone SchoolIntake
        /// </summary>
        /// <param name="admissionYear"></param>
        /// <param name="admissionTerm"></param>
        /// <param name="yearGroup"></param>
        /// <param name="plannedAdmissions"></param>
        /// <param name="schoolIntakeName"></param>
        /// <param name="admissionGroupName"></param>
        /// <param name="dateOfAdmission"></param>
        /// <param name="capacity"></param>
        /// <returns></returns>
        public string CloneSchoolIntake(string admissionYear, string admissionTerm, string yearGroup, string plannedAdmissions, string schoolIntakeName, string dateOfAdmission, string capacity)
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(50));
            // Intake Group
            Thread.Sleep(1000);
            SeleniumHelper.Get(Add_Button).Click();
            Thread.Sleep(1000);           
            Wait.WaitForAndGet(Admission_Year).ChooseSelectorOption(admissionYear);            
            Thread.Sleep(1000);            
            Wait.WaitForAndGet(Admission_Term).ChooseSelectorOption(admissionTerm);
            Thread.Sleep(1000);            
            Wait.WaitForAndGet(Admission_YearGroup).ChooseSelectorOption(yearGroup);            
            Thread.Sleep(1000);
            var plannedAdmissionNumber = SeleniumHelper.Get(By.Name("PlannedAdmissionNumber"));
            plannedAdmissionNumber.Clear();
            plannedAdmissionNumber.SendKeys(plannedAdmissions);

            Thread.Sleep(1000);            
            var name = SeleniumHelper.Get(Admission_Name);
            schoolIntakeName = name.GetValue() + schoolIntakeName;
            name.Clear();
            name.SendKeys(schoolIntakeName);
            // Admission Group
            Thread.Sleep(1000);            
            SeleniumHelper.Get(Add_Admission_Group).Click();
                        
            var admissionGroupGridDateOfAdmission = SeleniumHelper.Get(Admission_Date);
            admissionGroupGridDateOfAdmission.Clear();
            admissionGroupGridDateOfAdmission.SendKeys(dateOfAdmission);
            
            var admissionGroupGridCapacity = SeleniumHelper.Get(Admission_Capacity);
            admissionGroupGridCapacity.Clear();
            admissionGroupGridCapacity.SendKeys(capacity);

            //Save Intake Group
            Thread.Sleep(1000);
            SeleniumHelper.Get(Save_Button).Click();

            //Clone Intake Group
            Thread.Sleep(1000);
            SeleniumHelper.Get(Copy_Button).Click();

            //Save Cloned Marksheet 
            Thread.Sleep(1000);
            SeleniumHelper.Get(Save_Button).Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));    
            IWebElement SaveSuccessMessage = WebContext.WebDriver.FindElement(By.CssSelector("div[data-automation-id='status_success']"));
            waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Intake and Admission group record saved"));                        
            return schoolIntakeName;

        }

        public string getIntakeName()
        {
            return WebContext.WebDriver.FindElement(Admission_Name).GetValue();
        }              
        #endregion
    }

    public class SchoolIntakeSearchPage : SearchCriteriaComponent<SchoolIntakeTriplet.SchoolIntakeSearchResultTile>
    {
        public SchoolIntakeSearchPage(BaseComponent parent) : base(parent) { }

        #region Search properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _searchByName;

        [FindsBy(How = How.Name, Using = "YearOfAdmission.dropdownImitator")]
        private IWebElement _searchByAdmissionYear;

        [FindsBy(How = How.Name, Using = "YearGroup.dropdownImitator")]
        private IWebElement _searchYearGroup;

        [FindsBy(How = How.Id, Using = "IncludeInactiveSchoolIntakes")]
        private IWebElement _activeOrInActive;

        public string SearchByName
        {
            get { return _searchByName.GetValue(); }
            set { _searchByName.SetText(value); }
        }
        public string SearchYearAdmissionYear
        {
            get { return _searchByAdmissionYear.GetValue(); }
            set { _searchByAdmissionYear.EnterForDropDown(value); }
        }
        public string SearchByYearGroup
        {
            get { return _searchYearGroup.GetValue(); }
            set { _searchYearGroup.EnterForDropDown(value); }
        }

        public bool SetActiveOrInActive
        {
            set { _activeOrInActive.Set(value); }
            get { return _activeOrInActive.IsChecked(); }
        }
        #endregion
    }
}
