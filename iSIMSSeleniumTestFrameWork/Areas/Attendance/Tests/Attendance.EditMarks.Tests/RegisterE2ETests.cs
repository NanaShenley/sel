using Attendance.POM.DataHelper;
using NUnit.Framework;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Selene.Support.Attributes;
using SeSugar.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using TestSettings;
using Attendance.POM.Entities;
using System.Linq;
using POM.Components.SchoolManagement;
using POM.Base;
using WebDriverRunner.webdriver;
using OpenQA.Selenium;
using SeSugar.Automation;

namespace Attendance.EditMarks.Tests
{
    public class RegisterE2ETests
    {
        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Add Inset Day so that it is available on the register.
        /// Status: PASS
        /// </summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT001_Data")]
        public void EditMarks_Register_And_Verify_InsetDay_Functionality(string academicYearName, Dictionary<string, DateTime> Inset)
        {
           // #region Data Preperation

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitLoading();
            var academicYearTriplet = new AcademicYearTriplet();
           
            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget=academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget.Click();
            var academicYearDetailPage = new AcademicYearDetailPage();
            
            // Inset Day
            academicYearDetailPage.ClickInsetDayLink();
            academicYearDetailPage.InsetDayTable.GetLastRow().Name = "InsetDay";
            academicYearDetailPage.InsetDayTable.GetLastRow().Date = Inset["InsetDay"].Date.ToShortDateString();
            academicYearDetailPage.InsetDayTable.GetLastRow().AM = true;
            academicYearDetailPage.InsetDayTable.GetLastRow().PM = true;

            academicYearDetailPage.Save();
            if (academicYearDetailPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("Can't have duplicate inset day for the same date range");
                return;
            }

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = Inset["InsetDay"].Date.ToShortDateString();
            editMarksTripletPage.SearchCriteria.Day = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup("Year 1");
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarksPage.Marks;

            if (editMarksPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("It's Non-working Day");
                return;
            }
            IEnumerable<SchoolAttendanceCode> getHolidays = Queries.GetHolidays();
            List<string> codes = getHolidays.Select(x => x.Code).ToList<string>();
            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }

            IEnumerable<SchoolAttendanceCode> getInsetDays = Queries.GetInsetDay();
            List<string> inset = getInsetDays.Select(d => d.Code).ToList<string>();

            Assert.That(inset[0].Equals(editmarkTable[1][2].Text), "For Existing records, Marks InsetDay can't be applied");

            #region : Post-Condition : Delete Inset Day

            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            //Wait.WaitLoading();
            var academicYearTriplet1 = new AcademicYearTriplet();

            var academicYearResult1 = academicYearTriplet1.SearchCriteria.Search();
            var academicYearTarget1 = academicYearResult1.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget1.Click();
            var academicYearDetail1 = new AcademicYearDetailPage();

            academicYearDetail1.InsetDayTable.DeleteRowIfExist(academicYearDetail1.InsetDayTable.Rows.FirstOrDefault(x => Convert.ToDateTime(x.Date, CultureInfo.InvariantCulture) == Convert.ToDateTime(Inset["InsetDay"], CultureInfo.InvariantCulture).Date));
            academicYearDetail1.Save();
          
            #endregion
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Add Public HoliDay so that it is available on the register.
        /// Status: PASS
        /// </summary>

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT002_Data")]
        public void EditMarks_Register_And_Verify_PublicHoliday_Functionality(string academicYearName, string publicHoliday, DateTime publicHolidayDate)
        {
            //#region Data Preperation

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitLoading();

            var academicYearTriplet = new AcademicYearTriplet();

            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget.Click();
            var academicYearDetailPage = new AcademicYearDetailPage();

            // Public Holiday
            academicYearDetailPage.ClickAddPublicHolidayLink();
            academicYearDetailPage.PublicHolidayTable.GetLastRow().Name = publicHoliday;
            academicYearDetailPage.PublicHolidayTable.GetLastRow().Date = publicHolidayDate.ToShortDateString();

            academicYearDetailPage.Save();

            if (academicYearDetailPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("Can't have duplicate Holidays day for the same date range");
                return;
            }

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = publicHolidayDate.ToShortDateString();
            editMarksTripletPage.SearchCriteria.Day = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup("Year 1");
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarksPage.Marks;
            if (editMarksPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("It's Non-working Day");
                return;
            }
            Assert.AreEqual("#", editmarkTable[1][2].Text, "For Existing records, Marks InsetDay can't be applied");

            #region : Post-Condition : Delete Public HoliDay

            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            var academicYearTriplet1 = new AcademicYearTriplet();

            var academicYearResult1 = academicYearTriplet1.SearchCriteria.Search();
            var academicYearTarget1 = academicYearResult1.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget1.Click();
            var academicYearDetail1 = new AcademicYearDetailPage();

            academicYearDetail1.PublicHolidayTable.DeleteRowIfExist(academicYearDetail1.PublicHolidayTable.Rows.FirstOrDefault(x => Convert.ToDateTime(x.Date, CultureInfo.InvariantCulture) == Convert.ToDateTime(publicHolidayDate, CultureInfo.InvariantCulture).Date));
            academicYearDetail1.Save();

            #endregion
        }

        /// <summary>
        /// Author: Goyal, Gaurav
        /// Description: As School Administrator : Add School HoliDay so that it is available on the register.
        /// Status: PASS
        /// </summary>

        [WebDriverTest(TimeoutSeconds = 1000, Groups = new[] { "P2" }, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT003_Data")]
        public void EditMarks_Register_And_Verify_SchoolHoliday_Functionality(string academicYearName, string schoolHoliday, DateTime[] SchoolHoliday)
        {
            //#region Data Preperation

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            Wait.WaitForDocumentReady();
            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            Wait.WaitLoading();

            var academicYearTriplet = new AcademicYearTriplet();

            var academicYearResult = academicYearTriplet.SearchCriteria.Search();
            var academicYearTarget = academicYearResult.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget.Click();
            var academicYearDetailPage = new AcademicYearDetailPage();

            // School Holiday            
            academicYearDetailPage.ClickAddSchoolHolidayLink();
            academicYearDetailPage.SchoolHolidayTable.GetLastRow().Name = schoolHoliday;
            academicYearDetailPage.SchoolHolidayTable.GetLastRow().StartDate = SchoolHoliday[0].ToShortDateString();
            academicYearDetailPage.SchoolHolidayTable.GetLastRow().EndDate = SchoolHoliday[1].ToShortDateString();

            academicYearDetailPage.Save();

            if (academicYearDetailPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("Can't have duplicate Holidays day for the same date range");
                return;
            }

            AutomationSugar.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.StartDate = SchoolHoliday[0].ToShortDateString();
            editMarksTripletPage.SearchCriteria.Day = true;
            editMarksTripletPage.SearchCriteria.SelectYearGroup("Year 1");
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarksPage.Marks;
            if (editMarksPage.IsValidationMessageDisplay())
            {
                Console.WriteLine("It's Non-working Day");
                return;
            }
            Assert.AreEqual("#", editmarkTable[1][2].Text, "For Existing records, Marks InsetDay can't be applied");
            
            #region : Post-Condition : Delete Public HoliDay

            AutomationSugar.NavigateMenu("Tasks", "School Management", "Academic Years");
            var academicYearTriplet1 = new AcademicYearTriplet();

            var academicYearResult1 = academicYearTriplet1.SearchCriteria.Search();
            var academicYearTarget1 = academicYearResult1.FirstOrDefault(p => p.AcademicYear.Contains(academicYearName));
            academicYearTarget1.Click();
            var academicYearDetail1 = new AcademicYearDetailPage();

            academicYearDetail1.PublicHolidayTable.DeleteRowIfExist(academicYearDetail1.PublicHolidayTable.Rows.FirstOrDefault(x => Convert.ToDateTime(x.Date, CultureInfo.InvariantCulture) == Convert.ToDateTime(SchoolHoliday[0], CultureInfo.InvariantCulture).Date));
            academicYearDetail1.Save();

            #endregion
        }

        ///<summary>
        /// Verfiy Pupil Not On Roll Functionality For Day & Week View
        /// For Day View : Pupil Should not be visible on the register from the next day he made the leaver.
        /// For Week View : There should be '*' mark on the register from the next day he made the leaver.
        ///</summary>
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_PupilNotOnRoll_DATA")]
        public void EditMarks_verifyPupilNotOnRollFunctinality(string dateSearch, string pupilForeName, string pupilSurName,
            string dateOfBirth, string DateOfAdmission, string YearGroup, string Class, string pupilName, string leavingDate)
        {
            //Insert Pupil Record and make it leaver
            var learnerIdSetup = Guid.NewGuid();
            var learnerEnrolmentId = Guid.NewGuid();
            var BuildPupilRecord = this.BuildDataPackage();
            BuildPupilRecord.AddLeaver(learnerIdSetup, pupilSurName, pupilForeName, Convert.ToDateTime(dateOfBirth), Convert.ToDateTime(DateOfAdmission), Convert.ToDateTime(leavingDate), YearGroup, Class);

            using(new DataSetup(purgeBeforeInsert: false, purgeAfterTest: true, packages: BuildPupilRecord))
            {
                #region TEST-STEPS
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
                SeleniumHelper.NavigateQuickLink("Edit Marks");

                var editMarksTriplet = new EditMarksTriplet();
                editMarksTriplet.SearchCriteria.StartDate = dateSearch;
                
                //Verify Pupil Not On Roll Functionality For Day View
                editMarksTriplet.SearchCriteria.Day = true;
                editMarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
                
                var editMarkPage = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();

                var editmarkTable = editMarkPage.Marks;
                IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetHolidaysAndInsetDays();
                List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

                if (codes.Contains(editmarkTable[1][2].Text))
                {
                    Console.WriteLine("Marks can't be overwritten on Holidays");
                    return;
                }
                Assert.IsTrue(editmarkTable[pupilName]==null);

                //Verify Pupil Not On Roll Functionality For Day View
                editMarksTriplet.SearchCriteria.Week = true;
                var editMarkPageNew = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();
                var editmarkTableNew = editMarkPageNew.Marks;
                Assert.AreEqual("*", editmarkTableNew[pupilName][4].Text, "For Existing records, Pupil Not On Roll can't be applied");

                #endregion
            }
        }

        /// <summary>
        /// TC_ATT006
        /// Role: School Adminstrator
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome }, DataProvider = "TC_ATT004_DATA")]
        public void EditMarks_Register_Verify_Range_Of_functionalities(string dateSearch, string pupilForeName, string pupilSurName,
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

            #region TEST-STEPS
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateQuickLink("Edit Marks");

            var editMarksTriplet = new EditMarksTriplet();
            editMarksTriplet.SearchCriteria.StartDate = dateSearch;
            editMarksTriplet.SearchCriteria.SelectYearGroup(YearGroup);
            var editMarkPage = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();

            var editmarkTable = editMarkPage.Marks;
            IEnumerable<SchoolAttendanceCode> getANRs = Queries.GetAttendanceNotRequiredCodes();
            List<string> codes = getANRs.Select(x => x.Code).ToList<string>();

            if (codes.Contains(editmarkTable[1][2].Text))
            {
                Console.WriteLine("Marks can't be overwritten on Holidays");
                return;
            }      

            //Verify the Preserve Mode Functionality
            //editMarkPage = editMarksTriplet.SearchCriteria.Search<EditMarksPage>();
            editMarkPage.ModePreserve = true;

            //Enter Marks using Keyboard and Ensure blank marks can be edited while 'preserve' is enabled.
            var preserveModeMark = editMarkPage.Marks[pupilName][2].Text = "C";

            //Mark should be changed For Blank Cells in Preserve Mode
            Assert.AreEqual(preserveModeMark, editMarkPage.Marks[pupilName][2].Value, "Mark is updated when modePreserve is true");

            editMarkPage.ClickSave();

            //Enter Marks using Keyboard and Ensure previously existing marks cannot be edited while 'preserve' is enabled.
            var newmark = editMarkPage.Marks[pupilName][2].Text = "B";
            Assert.AreNotEqual(newmark, editMarkPage.Marks[pupilName][2].Value, "Mark is updated when modePreserve is true");

            //Enter mark using the code Dropdown List
            editMarkPage.Marks[pupilName][2].Focus();
            editMarkPage.CodeList = "B";

            //Mark can not be changed                
            Assert.AreNotEqual("B", editMarkPage.Marks[pupilName][2].Value, "Mark is updated when modePreserve is true");

            //Select Overwrite  Mode : Verify the Overwrite Mode Functionality
            editMarkPage.ModePreserve = false;

            //Ensure attendance marks can be entered using the keyboard   
            var originalMark = editMarkPage.Marks[pupilName][2].Text = "A";

            //Mark changed in Overwrite Mode
            Assert.AreEqual(originalMark, editMarkPage.Marks[pupilName][2].Value, "Mark is not updated when ModeOverwrite is true");

            //Enter mark using the code Dropdown List
            editMarkPage.Marks[pupilName][2].Focus();
            editMarkPage.CodeList = "A";

            //Check value the mark changed 
            Assert.AreEqual("A", editMarkPage.Marks[pupilName][2].Value, "Mark is not updated when ModeOverwrite is true");
            editMarkPage.ClickSave();

            // FloodFill data
            var markGridColumns = editMarkPage.Marks.Columns;
            markGridColumns[2].TimeIndicatorSelected = "AM";
            var floodFillMark = editMarkPage.CodeList = "L";
            editMarkPage.ClickSave();

            Assert.AreEqual(floodFillMark, editMarkPage.Marks[pupilName][2].Value, "The selected cells can not be flood filled");
            Assert.AreEqual(floodFillMark, editMarkPage.Marks[Convert.ToInt16(editMarkPage.Marks.RowCount / 2)][2].Value, "The selected cells can not be flood filled");
            Assert.AreEqual(floodFillMark, editMarkPage.Marks[Convert.ToInt16(editMarkPage.Marks.RowCount - 1)][2].Value, "The selected cells can not be flood filled");

            //Edit the mark with invalid value
            var inVaildMark = editMarkPage.Marks[pupilName][2].Text = "K";

            //VP: Value of mark can not be "abc"
            Assert.AreNotEqual(inVaildMark, editMarkPage.Marks[pupilName][2].Value, "The cell should not update 'K' value");

            //Add comment to cell
            editMarkPage.Marks[pupilName][2].Focus();
            editMarkPage.Marks[pupilName][2].OpenComment();
            var commentDialog = new AddCommentDialog();
            commentDialog.Comment = "Test Comment";
            commentDialog.MinuteLate = "100";
            commentDialog.ClickOk();

            //Check comment added
            editMarkPage.Marks[pupilName][3].Focus();
            editMarkPage.Marks[pupilName][2].Focus();
            editMarkPage.Marks[pupilName][2].OpenComment();
            commentDialog = new AddCommentDialog();
            Assert.AreEqual("Test Comment", commentDialog.Comment, "Comment is not saved");
            commentDialog.ClickOk();

            //Click Save
            editMarkPage.ClickSave();

            //Check Message success display
            Assert.AreEqual(true, editMarkPage.IsSuccessMessageDisplayed(), "Success message doesn't display");

             #endregion
        }
      

        #region DATA
        public List<object[]> TC_ATT001_Data()
        {
            DateTime Inset = DateTime.Now;
            var res = new List<Object[]>
            {                
                new object[] 
                {
                     SeleniumHelper.GetAcademicYear(DateTime.Now),
                     new Dictionary<string, DateTime>(){{"InsetDay", Inset}},
                }
            };
            return res;
        }

        public List<object[]> TC_ATT002_Data()
        {
            DateTime publicHolidayDate = DateTime.Now.AddDays(1);
            var res = new List<Object[]>
            {                
                new object[] 
                {
                     SeleniumHelper.GetAcademicYear(DateTime.Now), 
                     "PublicHoliday",
                     publicHolidayDate,
                }
            };
            return res;
        }

        public List<object[]> TC_ATT003_Data()
        {
            DateTime startSchoolHolidayDate = DateTime.Now.AddDays(2);
            DateTime endSchoolHolidayDate = DateTime.Now.AddDays(2);

            var res = new List<Object[]>
            {                
                new object[] 
                {
                     SeleniumHelper.GetAcademicYear(DateTime.Now),
                     "SchoolHoliday",
                     new DateTime[]{startSchoolHolidayDate, endSchoolHolidayDate},
                }
            };
            return res;
        }

        public List<object[]> TC_ATT004_DATA()
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

        public List<object[]> TC_PupilNotOnRoll_DATA()
        {
            string pattern = "M/d/yyyy";
            string surName = "AH_" + SeleniumHelper.GenerateRandomString(8);
            string foreName = SeleniumHelper.GenerateRandomString(8);
            string pupilName = String.Format("{0}, {1}", surName, foreName);
            string dateOfBirth = DateTime.ParseExact("10/01/2006", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string DateOfAdmission = DateTime.ParseExact("10/09/2010", pattern, CultureInfo.InvariantCulture).ToString(pattern);
            string dateSearch = "04/10/2016";
            string leavingDate = "03/10/2016";

            var data = new List<Object[]>
            {
                new object[] { dateSearch, foreName, surName, dateOfBirth, DateOfAdmission, "Year 2", "2A", pupilName, leavingDate }

            };
            return data;
        }

        #endregion
    }
}