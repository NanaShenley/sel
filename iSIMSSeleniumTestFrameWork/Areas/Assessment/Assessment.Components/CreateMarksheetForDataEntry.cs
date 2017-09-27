using System.Collections.ObjectModel;
using System.Threading;
using Assessment.Components.Common;
using Assessment.Components.PageObject;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class CreateMarksheetForDataEntry : BaseSeleniumComponents
   {
       private MarksheetBuilder marksheetBuilder = null;
       private string MarksheetTemplateName = null;
       private List<string> selectedGroups = null;

       public CreateMarksheetForDataEntry(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.AssessmentCoordinator)
       {
           SeleniumHelper.Login(userType, false);
           SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Set Up Marksheets and Parental Reports");
           //Create page object of marksheet home
           CreateMarksheetTypeSelectionPage marksheetTypeMenuPage = new CreateMarksheetTypeSelectionPage();
           marksheetBuilder = (MarksheetBuilder)marksheetTypeMenuPage.MarksheetTypeSelection("New Template");

           //Randomly generate Unique Marksheet Name
            MarksheetTemplateName = marksheetBuilder.RandomString(8);
           //Verifying the saved marksheet     
           marksheetBuilder.setMarksheetProperties(MarksheetTemplateName, "Description", true);
           marksheetBuilder.SelectPropertiesTab();
       }


       public CreateMarksheetForDataEntry CreateMarksheetfromExistingColumns()
       {
           AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
           AddAspects addAspects = addAssessments.NavigateAssessmentsviaAssessment();

           addAspects.SelectNReturnSelectedAssessments(3);
           AddAssessmentPeriod addAssessmentPeriod = addAspects.AspectNextButton();
           addAssessmentPeriod.AspectAssessmentPeriodSelection(1);
           addAssessmentPeriod.ClickAspectAssessmentPeriodDone();
           return this;
       }


       public CreateMarksheetForDataEntry CreateMarksheetFromNewColumns()
       {
           AddAssessmentPeriod addAssessmentPeriod = new AddAssessmentPeriod();
           AddAssessments addAssessments = marksheetBuilder.NavigateAssessments();
      
           AddSubjects addSubjects = addAssessments.NavigateAssessmentsviaSubject();

           addSubjects.SelectSubjectResultfromName("Language and Literacy");
           AddModeMethodPurpose addModeMethodPurpose = addSubjects.SubjectNextButton();

           addModeMethodPurpose.purposeAssessmentPeriodSelection(1);
           addModeMethodPurpose.modeAssessmentPeriodSelection(2);
           addModeMethodPurpose.methodAssessmentPeriodSelection(1);


           addAssessmentPeriod = addModeMethodPurpose.modeMethodPurposeNextButton();
           addAssessmentPeriod.subjectAssessmentPeriodSelection(1);
           marksheetBuilder = addAssessmentPeriod.ClickSubjectAssessmentPeriodDone();


          
           
            
           return this;
       }


        public CreateMarksheetForDataEntry AssignGroup()
        {
            AddGroups addgroups = marksheetBuilder.NavigateGroups();
            addgroups.SelectYearGroup("Year 1");
            selectedGroups = addgroups.GetSelectedGroupOrGroupfilter(MarksheetConstants.SelectYearGroups);
            addgroups.ClickDoneButton();
            return this;
        }

        public CreateMarksheetForDataEntry AssignGroupFilter(string Filter)
        {
            GroupFilters groupfilter = marksheetBuilder.NavigateAdditionalFilter();
            groupfilter.SelectClassFilterName(Filter);
            //WebContext.WebDriver.FindElements(By.CssSelector("[name='ClassesFilter.SelectedIds']"))
            //     .FirstOrDefault()
            //     .Click();
            marksheetBuilder = groupfilter.ClickDoneButton();
            return this;
        }


        public CreateMarksheetForDataEntry HiddenColumnTest()
       {
           ReadOnlyCollection<IWebElement> hiddenElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-hidden='']"));
           int count = hiddenElementsCollection.Count;

           hiddenElementsCollection[count - 1].Click();
           hiddenElementsCollection[count - 2].Click();
           return this;
       }


       public CreateMarksheetForDataEntry AssignGradeSet()
       {
           IWebElement propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
           IWebElement gradesetColumn = propertiesGrid.FindElement(MarksheetConstants.GradesetColumn);
           ReadOnlyCollection<IWebElement> gradesetColumnCell = gradesetColumn.FindElements(MarksheetConstants.ColumnCell);
           
           const int newi = 2;
           IWebElement gradeDetailsLink = gradesetColumnCell[newi].FindElement(By.TagName("button"));
           gradeDetailsLink.Click();
           MarksheetGridHelper.GradesetDetailsSelector("Age -99/11-99/11");

           Thread.Sleep(5000);

           //To avoid stale references.
           IWebElement propertiesGrid1 = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
           IWebElement gradesetColumn1 = propertiesGrid1.FindElement(MarksheetConstants.GradesetColumn);
           ReadOnlyCollection<IWebElement> gradesetColumnCell1 = gradesetColumn1.FindElements(MarksheetConstants.ColumnCell);

           IWebElement gradeDetailsLink1 = gradesetColumnCell1[newi + 1].FindElement(By.TagName("button"));
           gradeDetailsLink1.Click();
           MarksheetGridHelper.GradesetDetailsSelector("Marks 0 - 6");
           return this;
       }


       public CreateMarksheetForDataEntry SaveMarksheet()
       {
           marksheetBuilder.Save();
           marksheetBuilder.SaveMarksheetAssertionSuccess();
           return this;
       }


       public CreateMarksheetForDataEntry NavigateToDataEntryScreen()
       {
           SeleniumHelper.NavigatebackToMenu("Tasks", "Assessment", "Marksheets");
           WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(MarksheetTemplateName + " - " + selectedGroups[0]));
           return this;
       }



       public CreateMarksheetForDataEntry ShowPropertyScreenValidations()
       {
          
           IWebElement propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);

           IWebElement cellDiv = propertiesGrid.FindElement(By.CssSelector(MarksheetGridHelper.propertycolumnsselection));
           IWebElement parnetDiv = cellDiv.FindElement(By.CssSelector("[column='2']"));

           List<IWebElement> ElementList = MarksheetGridHelper.GetPropertiesHeaderList(parnetDiv);

           ElementList[1].Click();
           MarksheetGridHelper.GetEditor().Clear();
           MarksheetGridHelper.GetEditor().SendKeys("Test");
           MarksheetGridHelper.PerformEnterKeyBehavior();

           List<IWebElement> ElementList2 = MarksheetGridHelper.GetPropertiesHeaderList(parnetDiv);

           ElementList2[0].Click();
           MarksheetGridHelper.GetEditor().Clear();
           MarksheetGridHelper.PerformEnterKeyBehavior();

           MarksheetGridHelper.GetEditor().SendKeys("Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual Actual"); //Check 100 character length in header text

           MarksheetGridHelper.PerformEnterKeyBehavior();

           List<IWebElement> ElementList3 = MarksheetGridHelper.GetPropertiesHeaderList(parnetDiv);

           ElementList3[2].Click();
           return this;
       }

        public CreateMarksheetForDataEntry SetReadOnlyHiddenColumns()
        {
            ReadOnlyCollection<IWebElement> readOnlyElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-readonly='']"));
            int i = 0;
            int j = 0;
            foreach (IWebElement readonlyElement in readOnlyElementsCollection)
            {
                if (i == 1)
                readonlyElement.Click();
                i++;
            }


            //Make hidden
            ReadOnlyCollection<IWebElement> hiddenElementsCollection = WebContext.WebDriver.FindElements(By.CssSelector("[data-createmarksheet-hidden='']"));
            foreach (IWebElement hiddenElement in hiddenElementsCollection)
            {
               if(j == 2)
                hiddenElement.Click();

                j++;
            }
            return this;
        }


        public CreateMarksheetForDataEntry RearrangeColumns()
        {
            IWebElement propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
            IWebElement cellDiv = propertiesGrid.FindElement(By.CssSelector(MarksheetGridHelper.propertycolumnsselection));
            IWebElement parnetDiv = cellDiv.FindElement(By.CssSelector("[column='1']"));

            List<IWebElement> ElementList = MarksheetGridHelper.GetPropertiesHeaderList(parnetDiv);

            ElementList[1].Click();

            IJavaScriptExecutor js = WebContext.WebDriver as IJavaScriptExecutor;
           
            js.ExecuteScript("document.getElementsByClassName('webix_row_select')[1].getElementsByClassName('marksheet-properties-move up')[0].click();");
            return this;
        }


        public CreateMarksheetForDataEntry DeleteColumn()
        {
            ReadOnlyCollection<IWebElement> deleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            int count = 0;
            foreach (IWebElement deleteElement in deleteList)
            {
                if (count == 1)
                {
                    deleteElement.Click();
                   
                }
                count++;


            }
            return this;
        }


        public CreateMarksheetForDataEntry ApplyColouronHeader(int cellNumber,int Colour)
        {
               
            IWebElement propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);            

            IWebElement colourColumn = propertiesGrid.FindElement(MarksheetConstants.ColourColumn);
            ReadOnlyCollection<IWebElement> colourColumnCell = colourColumn.FindElements(MarksheetConstants.ColumnCell);

            int newi = cellNumber;
            colourColumnCell[newi].Click();
         
            propertiesGrid = MarksheetGridHelper.GetGridDetails(MarksheetGridHelper.PropertiesGridId);
            ReadOnlyCollection<IWebElement> colourColumnEditedCell = propertiesGrid.FindElements(MarksheetConstants.ColumnEditedCell);
            if (colourColumnEditedCell.Count == 0)
            {
                colourColumnEditedCell = propertiesGrid.FindElements(MarksheetConstants.ColumnEditedCellUp);
            }
            ReadOnlyCollection<IWebElement> colourCells = colourColumnEditedCell[0].FindElements(By.CssSelector("ul[class='stylepicker'] > li[class='stylepicker-swatch-wrapper']"));                
            IWebElement selectedColour = colourCells[Colour].FindElement(By.CssSelector("input[type='radio']"));
            selectedColour.Click();            
       
            return this;
        }
   }
}
