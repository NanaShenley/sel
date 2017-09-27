using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Assessment.Components.PageObject;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedComponents.HomePages;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class GroupFilterDetail : BaseSeleniumComponents
    {
        public GroupFilterDetail()
        {
            
        }
        public GroupFilterDetail(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.AssessmentCoordinator)
        {
            ////WebContext.WebDriver.Manage().Window.Maximize();
            ////PageFactory.InitElements(WebContext.WebDriver, this);

            ////SeleniumHelper.Login(userType);
            ////TaskMenuBar bar = new TaskMenuBar();
            ////bar.WaitFor().ClickTaskMenuBar().ClickAssessmentLink();

            ////////WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText("Marksheets"));
            ////PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public GroupFilterDetail OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public GroupFilterDetail CreateMarksheet(string groupFilter)
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator, false);
            SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
            //Create page object of marksheet home
            CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
            MarksheetBuilder marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Marksheet");

            //Randomly generate Unique Marksheet Name
            var MarksheetTemplateName = marksheetBuilder.RandomString(8);
            //Verifying the saved marksheet     
            marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
            marksheetBuilder.SelectPropertiesTab();

            AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
            AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

            addAspects.SelectNReturnSelectedAssessments(2);
            AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
            addAssessmentPeriod.AspectAssessmentPeriodSelection(1);
            addAssessmentPeriod.ClickAspectAssessmentPeriodDone();

            marksheetBuilder.NavigateAssessments();
            AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

            addSubjects.SelectSubjectResultfromName("Language and Literacy");
            AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

            addModeMethodPurpose.purposeAssessmentPeriodSelection(1);
            addModeMethodPurpose.modeAssessmentPeriodSelection(2);
            addModeMethodPurpose.methodAssessmentPeriodSelection(1);


            addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
            //addAssessmentPeriod.subjectAssessmentPeriodSelection(2);
            marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();


            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectYearGroup("Year 1");
            List<string> selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            addgroups.ClickDoneButton();
            GroupFilters groupfilter = new GroupFilters();
            groupfilter.SelectClassFilterName("");
            WebContext.WebDriver.FindElements(By.CssSelector("[name='ClassesFilter.SelectedIds']"))
                .FirstOrDefault()
                .Click();
            marksheetBuilder = groupfilter.ClickDoneButton();

            ReadOnlyCollection<IWebElement> hiddenElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-hidden='']"));
            int count = hiddenElementsCollection.Count;

            hiddenElementsCollection[count - 1].Click();
            hiddenElementsCollection[count - 2].Click();
            
            IWebElement propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
            propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
            IWebElement gradesetColumn = propertiesGrid.FindElement(MarksheetConstants.GradesetColumn);
            ReadOnlyCollection<IWebElement> gradesetColumnCell = gradesetColumn.FindElements(MarksheetConstants.ColumnCell);
            IWebElement gradeDetailsLink;
            int posMode = -1;
            int endOfYear = 0;

            gradesetColumn = propertiesGrid.FindElement(MarksheetConstants.GradesetColumn);
            gradesetColumnCell = gradesetColumn.FindElements(MarksheetConstants.ColumnCell);

            posMode = 0;

            int newi = 6;
            Assert.AreEqual(gradesetColumnCell[newi].Text, "Assign");
            gradeDetailsLink = gradesetColumnCell[newi].FindElement(By.TagName("button"));
            gradeDetailsLink.Click();
            MarksheetGridHelper.GradesetDetailsSelector("Age -99/11-99/11");
            // waiter.Until(ExpectedConditions.ElementToBeClickable(gradeDetailsLink));
            posMode++;

            //To avoid stale references.
            propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
            gradesetColumn = propertiesGrid.FindElement(MarksheetConstants.GradesetColumn);
            gradesetColumnCell = gradesetColumn.FindElements(MarksheetConstants.ColumnCell);
            // Assert.AreEqual("MIST Grades", gradesetColumnCell[newi].Text);

            gradeDetailsLink = gradesetColumnCell[newi + 1].FindElement(By.TagName("button"));
            gradeDetailsLink.Click();
            MarksheetGridHelper.GradesetDetailsSelector("Marks 0 - 6");
           // marksheetBuilder.CheckMarksheetIsAvailable();
            marksheetBuilder.Save();
            marksheetBuilder.SaveMarksheetAssertionSuccess();

            //Navigate to Marksheet Data Entry and check the marksheet is displayed
            SeleniumHelper.NavigatebackToMenu("Tasks", "Assessment", "Marksheets");
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(MarksheetTemplateName + " - " + selectedGroups[0]));
            Thread.Sleep(3000);

            AdditionalColumns addcolumn = new AdditionalColumns();
            addcolumn.ClickAdditionalColumns();
            addcolumn.SelectClassAdditionalColumns();
            addcolumn.ClickOk();
            addcolumn.ClickMarksheeyColumns();
            Thread.Sleep(2000);
            return this;
        }
    }
}
