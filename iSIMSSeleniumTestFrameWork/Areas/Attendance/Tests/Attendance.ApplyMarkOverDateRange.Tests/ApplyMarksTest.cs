using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;
using NUnit.Framework;
using System.Globalization;
using TestSettings;
using POM.Components.Attendance;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;

namespace Attendance.ApplyMarkOverDateRange.Tests
{
    public class ApplyMarksTest
    {    
        [WebDriverTest(TimeoutSeconds = 1000, Enabled = true, Browsers = new[] { BrowserDefaults.Chrome }, Groups = new[] { "g1" }, DataProvider = "TC_ATT001_Data")]
        public void TC_ATT002_EnterMarksOVerDateRange(string fullName, string sureName, string foreName, string classesData, string mark, string academicYear)
        {

            #region PRE-CONDITIONS:

            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator);
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editMarksTripletPage = new EditMarksTriplet();
            editMarksTripletPage.SearchCriteria.Week = true;
            editMarksTripletPage.SearchCriteria.SelectClass("1A");
            var editMarksPage = editMarksTripletPage.SearchCriteria.Search<EditMarksPage>();

            editMarksPage.ModePreserve = false;

            var marks = editMarksPage.Marks;
            marks[fullName][1].Text = String.Empty;
            marks[fullName][2].Text = String.Empty;
            marks[fullName][3].Text = String.Empty;
            marks[fullName][4].Text = String.Empty;
            marks[fullName][5].Text = String.Empty;
            marks[fullName][6].Text = String.Empty;
            marks[fullName][7].Text = String.Empty;
            marks[fullName][8].Text = String.Empty;
            marks[fullName][9].Text = String.Empty;
            marks[fullName][10].Text = String.Empty;

            editMarksPage.SaveConfirmDelete(true);
            SeleniumHelper.CloseTab("Edit Marks");

            #endregion

            #region STEPS:
            //Navigate to Apply Marks Over Date Range page
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Apply Marks Over Date Range");

            var applyMarksOverDateRange = new ApplyMarksOverDateRangeDialog();
            applyMarksOverDateRange.IsPreserve = true;

            //Click on Add Pupil Link
            var addPupilsDialogTriplet = applyMarksOverDateRange.AddPupil();

            // Select some options
            addPupilsDialogTriplet.SearchCriteria.SearchByPupilName = String.Format("{0}, {1}", sureName, foreName);
            var addPupilDialogSearchPage = addPupilsDialogTriplet.SearchCriteria;
            var classes = addPupilDialogSearchPage.Classes;
            classes[classesData].Select = true;

            // Search and select a pupil
            var addPupilsDetailsPage = addPupilDialogSearchPage.Search<AddPupilsDetailsPage>();
            addPupilsDetailsPage.Pupils.FirstOrDefault(x => x.Text.Trim().Equals(String.Format("{0}, {1}", sureName, foreName))).Click(); ;
            addPupilsDetailsPage.AddSelectedPupils();

            // Click on OK button
            addPupilsDialogTriplet.ClickOk();

            //Select Academic Year and Code from the dropdown
            applyMarksOverDateRange.SelectAcademicYear = academicYear;
            applyMarksOverDateRange.SelectMark = mark;

            //Apply Marks
            var confirmRequiredDialog = applyMarksOverDateRange.ClickApply();
            confirmRequiredDialog.ClickOk();
            applyMarksOverDateRange.ClosePatternDialog();

            // Navigate to Edit marks
            SeleniumHelper.NavigateMenu("Tasks", "Attendance", "Edit Marks");

            var editmarkTriplet = new EditMarksTriplet();
            editmarkTriplet.SearchCriteria.Week = true;
            editmarkTriplet.SearchCriteria.SelectClass("1A");
            editMarksPage = editmarkTriplet.SearchCriteria.Search<EditMarksPage>();
            var editMarksTable = editMarksPage.Marks;
           
            //Verify that Pattern is applied successfully
            var marksRow = editMarksPage.Marks[String.Format("{0}, {1}", sureName, foreName)];
            Assert.AreEqual(marksRow[1].Text, "Mark has applied to Monday AM");
            Assert.AreEqual(marksRow[2].Text, "Mark has applied to Monday PM");
            Assert.AreEqual(marksRow[3].Text, "Mark has applied to Tuesday AM");
            Assert.AreEqual(marksRow[4].Text, "Mark has applied to Tuesday PM");
            Assert.AreEqual(marksRow[5].Text, "Mark has applied to Wednesday AM");
            Assert.AreEqual(marksRow[6].Text, "Mark has applied to Wednesday PM");
            Assert.AreEqual(marksRow[7].Text, "Mark has applied to Thursday AM");
            Assert.AreEqual(marksRow[8].Text, "Mark has applied to Thursday PM");
            Assert.AreEqual(marksRow[9].Text, "Mark has applied to Friday AM");
            Assert.AreEqual(marksRow[10].Text, "Mark has applied to Friday AM");

            #endregion
        }

        #region DATA
        public List<object[]> TC_ATT_01_Data()
        {
            string fullName = "Surename 01, Forname01";
            string sureName = "Surename 01";
            string foreName = "Forname01";
            string mark = "C";
            string academicYear = "Academic Year 2015/2015";
 
            var data = new List<Object[]>
            {                
                new object[] {fullName, sureName,foreName,"1A"}
            };
            return data;
        }
        #endregion
    }
}
