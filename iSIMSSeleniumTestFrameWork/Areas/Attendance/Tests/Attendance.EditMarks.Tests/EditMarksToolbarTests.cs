using Attendance.Components.Common;
using Attendance.Components.AttendancePages;
using NUnit.Framework;
using OpenQA.Selenium;
using POM.Helper;
using System.Linq;
using TestSettings;
using WebDriverRunner.internals;
using Selene.Support.Attributes;
using System;
using WebDriverRunner.webdriver;
using Attendance.Components;
using System.Collections.Generic;

namespace Attendance.EditMarks.Tests
{
    public class EditMarksToolbarTests
    {

        #region Summary Section

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ShouldHaveSummarySection()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();

            Assert.IsTrue(editMarksPage.IsSummarySectionDisplayed());
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void VerifySummarySectionRows()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var group = new AttendanceSearchPanel();
            group.Select("Year Group", "Year 1", "Year 2");
            group.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            AttendanceDetails editMarksPage = group.EditMarksSearchButton();
            var summaryRows=editMarksPage.SummarySection().ToList();
            Assert.AreEqual(summaryRows[0].Text, "Total Present + AEA");
            Assert.AreEqual(summaryRows[1].Text, "Total Unrecorded");
            Assert.AreEqual(summaryRows[2].Text, "Total Authorised Absence");
            Assert.AreEqual(summaryRows[3].Text, "Total Unauthorised Absence");

        }
        #endregion

        #region Additional Columns Tests
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] {BrowserDefaults.Chrome})]
        public void ShouldDisplayAdditionalColumnDialog()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();

            //Select Class and Year Group
            var search_filter = new AttendanceSearchPanel();
            search_filter.Select("Year Group", "Year 1", "Year 2");
            AttendanceDetails editMarksGrid = search_filter.EditMarksSearchButton();

            // Check if the Identifier button is displayed
            Assert.IsTrue(editMarksGrid.additionalColumnButton.Displayed);
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void AdditionalColumnCountForAdmin()
        {
            Select_Year_And_NavigateToEditMarksScreen();
            AttendanceDetails editMarksGrid = new AttendanceDetails();
            AdditionalCoulmnPage additionalColumns = editMarksGrid.ClickAdditionalColumn();
            Assert.IsTrue(additionalColumns.GetDialogAdditionalColumnCount() == 12);
        }

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ShouldDisplayAdditionalColumns_OnClickOfAdditionalColumnOKButton()
        {
            Select_Year_And_NavigateToEditMarksScreen();
            AttendanceDetails editMarksGrid = new AttendanceDetails();
            AdditionalCoulmnPage additionalColumns = editMarksGrid.ClickAdditionalColumn();
            additionalColumns.ClickParentCheckboxes();
            additionalColumns.ClickOkButton();
            // Check the columns are added to the grid
            bool DOBColumn = SeleniumHelper.FindElement(EditMarksElements.GridColumns.DateOfBirth).Displayed;
            bool genderColumn = SeleniumHelper.FindElement(EditMarksElements.GridColumns.Gender).Displayed;
            Assert.IsTrue(DOBColumn && genderColumn);
        }


        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ShouldRemoveColumnsinGrid_OnClickOfClearSelection_AdditionalColumnOKButton()
        {
            Select_Year_And_NavigateToEditMarksScreen();
            AttendanceDetails editMarksGrid = new AttendanceDetails();
            AdditionalCoulmnPage additionalColumns = editMarksGrid.ClickAdditionalColumn();
            additionalColumns.ClickPersonalDetailsCheckbox();
            AttendanceDetails editMarksGrid1 = additionalColumns.ClickOkButton();
            AdditionalCoulmnPage additionalColumn1 = editMarksGrid1.ClickAdditionalColumn();
            additionalColumn1.ClearAdditionalColumnSelection();
            additionalColumn1.ClickOkButton();

            IWebElement grid = SeleniumHelper.Get(By.CssSelector("[data-section-id=\"searchResults\"]"));
            var columns = grid.FindElements(By.CssSelector("[data-menu-column-id]"));

            // Only the Pupil Name and Session columns should be present in the grid
            Assert.IsTrue(columns.Count == 3);
        }


        #endregion

        #region Dinner Monney

        #region verify flood fill for meal type

        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void VerifyFloodFillForMealType_InEditMarksToolBar()
        {
            //login with Dinner Money feature bee on
            string[] featureList = { "Dinner Money Settings" };
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

            Wait.WaitForDocumentReady();
            SeleniumHelper.NavigateQuickLink("Edit Marks");
            AttendanceNavigations.ClickDayOrWeekRadioButton("Day");
            AttendanceNavigations.SelectClass("6A");
            AttendanceNavigations.ClickEditMarksSearchButton();

            Wait.WaitForDocumentReady();
            SeleniumHelper.Sleep(10);

            AttendanceDetails editMarksGrid = new AttendanceDetails();

            SeleniumHelper.FindAndClick(By.CssSelector("tr[section='header']:nth-child(3) td[column='3']"));
            SeleniumHelper.Sleep(2);
            editMarksGrid.ClickMealColumn();

            // check Overwrite existing values check box  
            SeleniumHelper.FindAndClick(By.CssSelector(".grid-menu:nth-child(9) .checkbox input"));

            //select H (Home) meal choice
            SeleniumHelper.FindElement(By.XPath("//*[@id='screen-viewer']/div/div[2]/div/div/div[2]/div[7]/div[3]/div[2]/div[1]/div/select")).SelectByText("H");

            //click column button
            SeleniumHelper.FindAndClick(By.XPath("//*[@id='screen-viewer']/div/div[2]/div/div/div[2]/div[7]/div[3]/div[2]/div[3]/div/button[1]"));

            List<IWebElement> cells = EditMarksGridHelper.FindAllcells();
            var cellValues = cells.Skip(1).FirstOrDefault().Text;

            string[] stringSeparators = new string[] { "\r\n" };
            IEnumerable<string> distinctMealCode = cellValues.Split(stringSeparators, StringSplitOptions.None).Distinct();

            //if flood fill is successful, all the cells would have been populated with 'H', so distinctMealCode collection should have only one 
            //item 'H'
            Assert.IsTrue(distinctMealCode.Count() == 1 || distinctMealCode.FirstOrDefault() == "H");
        }

        #endregion

        #region Verify Meal codes in Edit mark tool bar
        [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
            public void CheckMealTypeInMealCodesDropDown_InEditMarksToolBar()
            {
                string[] featureList = { "Dinner Money Settings" };
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

                Wait.WaitForDocumentReady();
                SeleniumHelper.NavigateQuickLink("Edit Marks");
                AttendanceNavigations.ClickDayOrWeekRadioButton("Day");
                AttendanceNavigations.SelectClass("6A");
                AttendanceNavigations.ClickEditMarksSearchButton();

                Wait.WaitForDocumentReady();
                SeleniumHelper.Sleep(10);

                var mealCodeDropDown = WebContext.WebDriver.FindElements(EditMarksElements.Toolbar.AllCodessDropdown).Skip(1).FirstOrDefault();
                mealCodeDropDown.Click();
                var absentMeal = WebContext.WebDriver.FindElement(By.XPath("//*[@id='screen-viewer']/div/div[1]/div/div[2]/div[2]/ul[1]/li[3]/ul/li[1]/a/span/span")).Text;
                var hometMeal = WebContext.WebDriver.FindElement(By.XPath("//*[@id='screen-viewer']/div/div[1]/div/div[2]/div[2]/ul[1]/li[3]/ul/li[2]/a/span/span")).Text;
                var packedLunch = WebContext.WebDriver.FindElement(By.XPath("//*[@id='screen-viewer']/div/div[1]/div/div[2]/div[2]/ul[1]/li[3]/ul/li[3]/a/span/span")).Text;
                var schoolMeal = WebContext.WebDriver.FindElement(By.XPath("//*[@id='screen-viewer']/div/div[1]/div/div[2]/div[2]/ul[1]/li[3]/ul/li[4]/a/span/span")).Text;

                SeleniumHelper.Logout();

                Assert.IsTrue(absentMeal.Trim() == "A - Absent" &&
                    hometMeal.Trim() == "H - Home" &&
                    packedLunch.Trim() == "P - Packed Lunch" &&
                    schoolMeal.Trim() == "S - School Meal");
            }
            #endregion

            #region Save Pupil's meal selection (meal codes) in Edit Marks screen
            [WebDriverTest(Enabled = true, Groups = new[] { "P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
            public void SaveMealTypes_InEditMarksToolBar()
            {
                string[] featureList = { "Dinner Money Settings" };
                SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, featureList);

                Wait.WaitForDocumentReady();
                SeleniumHelper.NavigateQuickLink("Edit Marks");
                AttendanceNavigations.ClickDayOrWeekRadioButton("Day");
                AttendanceNavigations.SelectClass("6A");
                AttendanceNavigations.ClickEditMarksSearchButton();

                Wait.WaitForDocumentReady();
                SeleniumHelper.Sleep(5);

                EditMarksGridHelper grid = new EditMarksGridHelper();
                grid.ClickOrientationbutton(grid.preserveButton);
                grid.ClickOrientationbutton(grid.overwriteMode);
                EditMarksGridHelper.ClickFirstCellofColumn("3");
                EditMarksGridHelper.GetEditor().SendKeys("A");
                EditMarksGridHelper.GetEditor().SendKeys("H");
                EditMarksGridHelper.GetEditor().SendKeys("P");
                EditMarksGridHelper.GetEditor().SendKeys("S");

                var registerSave = WebContext.WebDriver.FindElement(EditMarksElements.Toolbar.Save);
                registerSave.Click();
                SeleniumHelper.Sleep(3);
                string saveMessage = WebContext.WebDriver.FindElement(By.XPath("//strong[contains(text(),'Register saved')]")).Text;

                SeleniumHelper.Logout();

                Assert.IsTrue(saveMessage == "Register saved");
            }
            #endregion

        #endregion

        #region Common Stuffs
        private void Select_Year_And_NavigateToEditMarksScreen()
        {
            AttendanceNavigations.NavigateToEditMarksMenuPage();
            var search_filter = new AttendanceSearchPanel();
            search_filter.Select("Year Group", "Year 1", "Year 2");
            search_filter.EnterDate(SeleniumHelper.GetFirstDayOfWeek(DateTime.Now).ToShortDateString());
            search_filter.EditMarksSearchButton();
        }
        #endregion

    }
}
