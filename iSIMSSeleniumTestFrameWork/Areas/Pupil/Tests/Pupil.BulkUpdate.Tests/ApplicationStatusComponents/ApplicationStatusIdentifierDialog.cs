using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Pupil.Components.Common;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Pupil.BulkUpdate.Tests.ApplicationStatusComponents
{
    public class ApplicationStatusIdentifierDialog : BaseSeleniumComponents
    {
        private readonly static By PersonalDetails = By.CssSelector("#fixedcolumntreenode > div > div > div > div:nth-child(1) > div.webix_tree_item > input");
        private readonly static By DateOfBirth = By.CssSelector("div[webix_tm_id='Application.Learner.DateOfBirth'] > input");
        private readonly static By Gender = By.CssSelector("div[webix_tm_id='Application.Learner.Gender'] > input");

        private readonly static By RegistrationDetails = By.CssSelector("#fixedcolumntreenode > div > div > div > div:nth-child(2) > div.webix_tree_item > input");
        private readonly static By AdmissionGroup = By.CssSelector("div[webix_tm_id='Application.AdmissionGroup'] > input");
        private readonly static By ApplicationStatus = By.CssSelector("div[webix_tm_id='ApplicantApplicationStatus'] > input");
        private readonly static By ProposedDoa = By.CssSelector("div[webix_tm_id='Application.LearnerEnrolment.DOA'] > input");
        private readonly static By SchoolIntake = By.CssSelector("div[webix_tm_id='Application.AdmissionGroup.SchoolIntake'] > input");
        private readonly static By YearGroup = By.CssSelector("div[webix_tm_id='Application.Learner.LearnerYearGroupMemberships.YearGroup'] > input");
        private readonly static By Class = By.CssSelector("div[webix_tm_id='Application.Learner.LearnerPrimaryClassMemberships.PrimaryClass'] > input");

        private readonly static By ButtonOk = By.CssSelector("button[id='identifiers-ok-button-id']");
        public static By ClearSelection = By.CssSelector("a[data-clear-container-id='fixedcolumntreenode']");

        public IWebElement PersonalDetailIdentifierCheckBox = SeleniumHelper.Get(PersonalDetails);
        public IWebElement DateOfBirthCheckBox = SeleniumHelper.Get(DateOfBirth);
        public IWebElement GenderCheckBox = SeleniumHelper.Get(Gender);
        
        public IWebElement RegisterationDetailCheckBox = SeleniumHelper.Get(RegistrationDetails);
        public IWebElement AdmissionGroupCheckBox = SeleniumHelper.Get(AdmissionGroup);
        public IWebElement ApplicationStatusCheckBox = SeleniumHelper.Get(ApplicationStatus);
        public IWebElement ProposedDoaCheckBox = SeleniumHelper.Get(ProposedDoa);
        public IWebElement SchoolIntakeCheckBox = SeleniumHelper.Get(SchoolIntake);
        public IWebElement YearGroupCheckBox = SeleniumHelper.Get(YearGroup);
        public IWebElement ClassCheckBox = SeleniumHelper.Get(Class);


        public ApplicationStatusIdentifierDialog()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickOnIdentifierDialogOKButton()
        {
            SeleniumHelper.FindAndClick(ButtonOk);
        }

        public void ClickOnClearSelection()
        {
            SeleniumHelper.FindAndClick(ClearSelection);
        }

    }
}
