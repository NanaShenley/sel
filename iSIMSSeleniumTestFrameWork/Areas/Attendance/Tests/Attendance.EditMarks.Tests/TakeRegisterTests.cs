using Attendance.POM.DataHelper;
using Attendance.POM.Entities;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Components.Attendance;
using POM.Components.HomePages;
using POM.Components.Pupil;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar;
using SeSugar.Automation;
using SeSugar.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Attendance.EditMarks.Tests
{
    public class TakeRegisterTests
    {
        [WebDriverTest(Enabled = false, Browsers = new[] { BrowserDefaults.Chrome })]
        public void Take_Register_Navigation_From_QuickLinks()
        {
            //Login and navigate to Take Register page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher, "ClassPicker");

            Wait.WaitForDocumentReady();
            var homePage = new HomePage();
            homePage.SelectClassFromQuickLink("6A");
            var quicklink = homePage.MenuBar();
            quicklink.TakeRegister();

            //Search class 1A at 9/30/2015"
            var takeRegisterTriplet = new TakeRegisterTriplet();

            var classregisterheading = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='take_register_header_display_name']")).Text;
            Assert.IsTrue(classregisterheading.Contains("6A"));
        }

        /// <summary>
        /// Ba.Truong
        /// Description: Take a class register and varify a range of functionality.
        /// </summary>
        /// <param name="registerDate"></param>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT001_DATA")]
        public void Take_Class_Register_And_Verify_Preserve_OverWrite_FloodFill_Functionality(string dateSearch, string pupilForeName, string pupilSurName,
            string gender, string dateOfBirth, string DateOfAdmission, string YearGroup, string pupilName)
        {
            #region Pre-Conditions:

            // Navigate to Pupil Record
            DateTime dobSetup = Convert.ToDateTime(dateOfBirth);
            DateTime dateOfAdmissionSetup = Convert.ToDateTime(DateOfAdmission);

            var learnerIdSetup = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();

            BuildPupilRecord.CreatePupil(learnerIdSetup, pupilSurName, pupilForeName, dobSetup, dateOfAdmissionSetup, YearGroup);

            DataSetup DataSetPupil = new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord);

            #endregion

            //Login and navigate to Take Register page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            Wait.WaitForDocumentReady();

            var homePage = new HomePage();
            var quicklink = homePage.MenuBar();
            quicklink.TakeRegister();

            //Enter Search Criteria
            var takeRegisterTriplet = new TakeRegisterTriplet();
            takeRegisterTriplet.SearchCriteria.StartDate = dateSearch;
            takeRegisterTriplet.SearchCriteria.Week = true;
            takeRegisterTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var takeRegisterDetail = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();

            var takeRegisterTable = takeRegisterDetail.Marks;

            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(takeRegisterTable[0][1].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }

            //Verify the Preserve Mode Functionality
            takeRegisterDetail.ModePreserve = true;

            //Enter Marks using Keyboard and Ensure blank marks can be edited while 'preserve' is enabled.
            var preserveModeMark = takeRegisterDetail.Marks[pupilName][1].Text = "C";
            Assert.AreEqual(preserveModeMark, takeRegisterDetail.Marks[pupilName][1].Value, "Mark is updated when modePreserve is true");

            takeRegisterDetail.ClickSave();

            //Edit saved mark using the code list
            takeRegisterDetail.Marks[pupilName][1].Select();
            takeRegisterDetail.CodeList = "B";

            //VP: Mark can not be changed
            Assert.AreNotEqual("B", takeRegisterDetail.Marks[pupilName][1].Text, "Mark is updated when modePreserve is true");

            //Ensure a selection of cells can't be flood filled for Preserve Mode
            takeRegisterDetail.Marks.Columns[1].TimeIndicatorSelected = "AM";
            takeRegisterDetail.CodeList = "/";
            Assert.AreNotEqual("/", takeRegisterDetail.Marks[pupilName][1].Text, "The selected cells can not be flood filled");
            Assert.AreNotEqual("/", takeRegisterDetail.Marks[Convert.ToInt16(takeRegisterDetail.Marks.RowCount / 2)][1].Text, "The selected cells can not be flood filled");

            //Edit the mark with invalid value
            SeSugar.Automation.Retry.Do(takeRegisterDetail.Marks[pupilName][1].Select, 200, 100);
            takeRegisterDetail.Marks[pupilName][1].Text = "K";

            //VP: Value of mark can not be "K"
            Assert.AreNotEqual("K", takeRegisterDetail.Marks[pupilName][1].Text, "The cell should not update 'T' value");

            //Select Overwrite  Mode : Verify the Overwrite Mode Functionality
            takeRegisterDetail.ModePreserve = false;          

            //Enter the mark "Late" to row 1, column 2 using the code list
            takeRegisterDetail.Marks[pupilName][1].Select();
            takeRegisterDetail.CodeList = "L";

            //Check value the mark changed 
            Assert.AreEqual("L", takeRegisterDetail.Marks[pupilName][1].Text, "Mark is updated");

            //Enter the mark "Late" to row 1, column 1 using the keyboad
            takeRegisterDetail.Marks[pupilName][1].Text = "N";

            //Check value the mark changed
            Assert.AreEqual("N", takeRegisterDetail.Marks[pupilName][1].Text, "Mark is overwritten");

            //Enter the mark to row 1, column 1 using the UI buttons
            takeRegisterDetail.Marks[pupilName][3].Select();
            takeRegisterDetail.CodeUI = "L";

            //Check value the mark changed 
            Assert.AreEqual("L", takeRegisterDetail.Marks[pupilName][3].Text, "Mark is overwritten");

            //Enter the mark "Absent" to row 1, column 1, using the drop-down
            takeRegisterDetail.Marks[pupilName][4].DoubleClick();
            takeRegisterDetail.CodeItemDropDown = "N";

            //Check value the mark changed
            Assert.AreEqual("N", takeRegisterDetail.Marks[pupilName][4].Text, "Mark is overwritten");

            //Ensure a selection of cells can be flood filled
            takeRegisterDetail.Marks.Columns[1].TimeIndicatorSelected = "AM";
            takeRegisterDetail.CodeList = "L";
            Assert.AreEqual("L", takeRegisterDetail.Marks[pupilName][1].Text, "The selected cells can not be flood filled");
            Assert.AreEqual("L", takeRegisterDetail.Marks[Convert.ToInt16(takeRegisterDetail.Marks.RowCount / 2)][1].Text, "The selected cells can not be flood filled");

            //Click Save
            takeRegisterDetail.ClickSave();
            Assert.AreEqual(true, takeRegisterDetail.IsSuccessMessageDisplayed(), "Success message doesn't display");
        }

        ///<summary>
        /// Ba.Truong
        /// Description: Add Comments on register 
        ///</summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT005_Data")]
        public void Take_Class_Register_And_Verify_Comments(string registerDate)
        {
            //Login and navigate to Take Register page
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);
            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Take Register");

            //Search future class
            var takeRegisterTriplet = new TakeRegisterTriplet();
            takeRegisterTriplet.SearchCriteria.StartDate = registerDate;
            takeRegisterTriplet.SearchCriteria.Week = true;
            takeRegisterTriplet.SearchCriteria.SelectYearGroup("Year 1");
            var takeRegisterDetail = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();

            var takeRegisterTable = takeRegisterDetail.Marks;
            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(takeRegisterTable[0][1].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }

            //Select Overwrite  Mode
            takeRegisterDetail.ModePreserve = false;
            takeRegisterDetail.Marks[0][1].Text = "L";

            //Add comment to cell
            takeRegisterDetail.Marks[0][1].Focus();
            takeRegisterDetail.Marks[0][1].OpenComment();
            var commentDialog = new AddCommentDialog();
            Wait.WaitLoading();
            commentDialog.Comment = "Test Comment";
            commentDialog.MinuteLate = "100";
            commentDialog.ClickOk();

            //Check comment added
            takeRegisterDetail.Marks[0][2].Focus();
            takeRegisterDetail.Marks[0][1].Focus();
            takeRegisterDetail.Marks[0][1].OpenComment();
            commentDialog = new AddCommentDialog();
            Assert.AreEqual("Test Comment", commentDialog.Comment, "Comment is not saved");
            commentDialog.ClickOk();

            //Click Save
            takeRegisterDetail.ClickSave();

            //Check Message success display
            Assert.AreEqual(true, takeRegisterDetail.IsSuccessMessageDisplayed(), "Success message doesn't display");
        }

        /// <summary>
        /// TC_ATT004
        /// Au : Hieu Pham
        /// Description: Validate additional information can be displayed when taking register.
        /// Role: School Adminstrator
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT006_DATA")]
        public void TC_ATT006_Validate_Additional_Information_Displayed(string pupilSurName, string pupilForeName, string gender, DateTime dateOfBirth,
            DateTime DateOfAdmission, string YearGroup, string className, string pupilName, string contactSurname, string startDate, string startDateBefore)
        {
            #region Pre-Conditions:

            #region Pre-Condition: Create a new pupil for test

            AutomationSugar.Log("Data Creation started");        
            var learnerId = Guid.NewGuid();
            var dataPackage = this.BuildDataPackage();
            dataPackage.CreatePupil(learnerId, pupilSurName, pupilForeName, dateOfBirth, DateOfAdmission, YearGroup);

            AutomationSugar.Log("***Pre-Condition: Create new contact1 and refer to pupil");
            var dataPackageNew = this.BuildDataPackage();
            Guid learnerContactId = Guid.NewGuid();
            Guid pupilContactRelationshipId1 = Guid.NewGuid();

            //Add pupil contact
            var contactSurname1 = Utilities.GenerateRandomString(10, "SeleniumPupilContact1_Surname" + Thread.CurrentThread.ManagedThreadId);
            var contactForename1 = Utilities.GenerateRandomString(10, "SeleniumPupilContact1_Forename" + Thread.CurrentThread.ManagedThreadId);
            var titleContact1 = "Mrs";
            dataPackageNew.AddPupilContact(learnerContactId, contactSurname1, contactForename1);
            dataPackageNew.AddPupilContactRelationship(pupilContactRelationshipId1, learnerId, learnerContactId);

            #endregion

            using (new DataSetup(packages: new DataPackage[] {dataPackage, dataPackageNew}))
            {
                // Login
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ClassTeacher);

                // Navigate to take register
                AutomationSugar.NavigateMenu("Tasks", "Attendance", "Take Register");

                // Enter values to search
                var takeRegisterTriplet = new TakeRegisterTriplet();
                takeRegisterTriplet.SearchCriteria.StartDate = startDateBefore;
                takeRegisterTriplet.SearchCriteria.Week = true;
                takeRegisterTriplet.SearchCriteria.SelectYearGroup(YearGroup);
                var takeRegisterPage = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();
                var takeRegisterTable = takeRegisterPage.Marks;

                // Enter pre-condition mark
                takeRegisterTable[pupilName][1].Text = "/";
                takeRegisterTable[pupilName][2].Text = "N";
                takeRegisterTable[pupilName][3].Text = "N";
                takeRegisterTable[pupilName][4].Text = "N";
                takeRegisterTable[pupilName][5].Text = "/";
                takeRegisterTable[pupilName][6].Text = "/";

                // Save Value
                takeRegisterPage.ClickSave();

                #endregion

                #region Steps

                // Enter values to search again
                takeRegisterTriplet = new TakeRegisterTriplet();
                takeRegisterTriplet.SearchCriteria.StartDate = startDate;
                takeRegisterTriplet.SearchCriteria.Day = true;
                takeRegisterPage = takeRegisterTriplet.SearchCriteria.Search<TakeRegisterDetailPage>();
                takeRegisterTable = takeRegisterPage.Marks;

                // Verify birthday cake displays
                takeRegisterTable = takeRegisterPage.Marks;
                Assert.AreEqual(true, takeRegisterTable[pupilName].IsBirthdayCakeDisplay(), "Birthday cake does not displays");

                // Click pupil name
                takeRegisterTable[pupilName].ClickCellPupilName();
                var pupilDetailDialog = new PupilDetailDialog();

                // Verify pupil name and DOB is displayed.
                Assert.IsTrue(pupilDetailDialog.PupilName.Contains(pupilName), "Pupil name is not correct");
                Assert.IsTrue(pupilDetailDialog.PupilDOB.Contains(dateOfBirth.ToString("dd MMMM yyyy")), "Date of Birth is not correct");

                // Verify contact detail displays
                Assert.AreEqual(true, pupilDetailDialog.IsContactDetailDisplay(), "Contact detail does not display");

                #endregion
            }

        }


        #region DATA
        public List<object[]> TC_ATT001_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "AH_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateSearch = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString();

            var data = new List<Object[]>
            {
                new object[] { dateSearch, foreName, surName, "Male", dateOfBirth, DateOfAdmission, "Year 2", pupilName }

            };
            return data;
        }

        public List<object[]> TC_ATT005_Data()
        {
            string pattern = "M/d/yyyy";
            string registerDate = SeleniumHelper.GetFirstDayOfWeek(DateTime.Now.AddDays(-1)).ToShortDateString();


            var res = new List<Object[]>
            {                
                new object[] {
                    registerDate,
                },
            };
            return res;
        }

        public List<object[]> TC_ATT006_DATA()
        {
            string pattern = "dd/mm/yyyy";
            string surName = SeleniumHelper.GenerateRandomString(6);
            string foreName = SeleniumHelper.GenerateRandomString(6);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            DateTime dateOfBirth = new DateTime(2006, 10, 18);
            DateTime DateOfAdmission = new DateTime(2012, 12, 19);
            string startDate = DateTime.ParseExact("18/10/2006", pattern, CultureInfo.InvariantCulture).AddYears(DateTime.Today.Year - int.Parse("18/10/2006".Split('/')[2])).ToString(pattern);
            string startDateBefore = SeleniumHelper.GetDayAfter(startDate, -3);


            var data = new List<Object[]>
            {
                new object[] {surName, foreName, "Male", dateOfBirth, DateOfAdmission, "Year 2", "1A", pupilName, "ContactName", startDate, startDateBefore }

            };
            return data;
        }
        #endregion
    }
}
