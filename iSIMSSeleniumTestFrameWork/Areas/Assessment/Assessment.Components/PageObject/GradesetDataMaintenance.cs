using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;
using OpenQA.Selenium.Interactions;
using SharedServices.Components.PageObjects;

namespace Assessment.Components.PageObject
{
    public class GradesetDataMaintenance
    {
        public GradesetDataMaintenance()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        //Gradeset Details
        [FindsBy(How = How.CssSelector, Using = "#editableData input[name='Name']")]
        private IWebElement Name;

        [FindsBy(How = How.CssSelector, Using = "#editableData textarea[name='Description']")]
        private IWebElement Description;

        [FindsBy(How = How.CssSelector, Using = "form[id='editableData'] input[name='Code']")]
        private IWebElement Code;

        [FindsBy(How = How.CssSelector, Using = "form[id='editableData'] input[name='AssessmentGradesetType.dropdownImitator']")]
        private IWebElement GradesetType;

        [FindsBy(How = How.CssSelector, Using = "div[data-grid-id='AssessmentGradesetInstancesGrid'] button[data-automation-id='add_assessment_gradeset_version_button']")]
        private IWebElement AddGradeVersionLink;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_grade_button']")]
        private IWebElement addGradeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-section-id='palette-editor-container']")]
        private IWebElement GradesetConfirmationDialog;

        [FindsBy(How = How.CssSelector, Using = "div[class='modal-content'] button[data-automation-id='ok_button']")]
        private IWebElement GradesetConfirmationSaveButton;

        //Gradeset Version

        [FindsBy(How = How.CssSelector, Using = "input[name='currentAssessmentGradesetInstanceDetail.MinimumValue']")]
        private IWebElement MinimumValue;

        [FindsBy(How = How.CssSelector, Using = "input[name='currentAssessmentGradesetInstanceDetail.MaximumValue']")]
        private IWebElement MaximumValue;

        [FindsBy(How = How.CssSelector, Using = "input[name*='StartDate']")]
        private IWebElement StartDate;

        [FindsBy(How = How.CssSelector, Using = "input[name*='EndDate']")]
        private IWebElement EndDate;

        [FindsBy(How = How.CssSelector, Using = "div[id='dialog-palette-editor'] button[data-automation-id='ok_button']")]
        private IWebElement OkButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-marksheet-cancel-button]")]
        private IWebElement CancelButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Button_Dropdown']")]
        private IWebElement CreateButton;

        [FindsBy(How = How.CssSelector, Using = "button[title='Hide Search']")]
        private IWebElement _hideSearchPanel;

        [FindsBy(How = How.CssSelector, Using = "a[title='Create'] a[data-automation-id='create_age_gradeset']")]
        private IWebElement CreateAgeGradeset;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='edit..._button']")]
        private IWebElement editGradesetVersionButton;

        [FindsBy(How = How.CssSelector, Using = "input[name*='.EndDate'] + span[data-automation-id='date_picker']")]
        private IWebElement versionEndDatePicker;

        [FindsBy(How = How.CssSelector, Using = "input[name*='.StartDate'] + span[data-automation-id='date_picker'] i[class='fa fa-calendar']")]
        private IWebElement versionStartDatePicker;

        [FindsBy(How = How.CssSelector, Using = "div[class*='bootstrap-datetimepicker-widget dropdown-menu picker-open'] td.day.active.today")]
        private IWebElement gradesetVersionActiveDate;
        
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_grade_gradeset']")]
        private IWebElement CreateGradeGradeset;
       

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_marks_Integer_gradeset']")]
        private IWebElement CreateMarksIntegerGradeset;

        [FindsBy(How = How.CssSelector, Using = "a[title='Create'] a[data-automation-id='create_comment_gradeset']")]
        private IWebElement CreateCommentGradeset;

        [FindsBy(How = How.Id, Using = "GradeSetSave")]
        private IWebElement GradesetSaveButton;

        [FindsBy(How = How.CssSelector, Using = "a[title='Cancel the Record'][data-automation-id='B717AAA4-EDC1-42AE-BE46-902F0DAB8BB6']")]
        private IWebElement GradesetCancelButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        private IWebElement GradesetDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Result Type History']")]
        private IWebElement GradesetVersionButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Grades']")]
        private IWebElement GradesetGradesButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='continue_with_delete_button']")]
        private readonly IWebElement GradesetContinueButton = null;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;

        private static By saveSuccessMessage_New = By.CssSelector("div[data-automation-id='status_success']");

        private static By validationMessage = By.CssSelector("div[classname='alert-warning validation-summary-errors']");

        [FindsBy(How = How.CssSelector, Using = "div[classname='alert-warning validation-summary-errors']")]
        public IWebElement ValidationCodeMessage;

        [FindsBy(How = How.CssSelector, Using = "h4[data-automation-id='edit_gradeset_version_popup_header_title']")]
        private IWebElement editGradesetVersionHeader;

        [FindsBy(How = How.CssSelector,Using ="tr[data-row-name='AssessmentGradesetInstances']")]
        private IWebElement defaultGradesetVersion;

        private static By ColourPickerList = By.ClassName("stylepicker-swatch-wrapper");

        // Grade Value List

        private static By DescriptionList = By.CssSelector("tr[data-row-name='AssessmentGrade'] input[name*='Description']");
        private static By ValueList = By.CssSelector("tr[data-row-name='AssessmentGrade'] input[name*='Value']");
        private static By CodeList = By.CssSelector("tr[data-row-name='AssessmentGrade'] input[name*='Code']");

        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='new_version']")]
        private IWebElement newGradesetVersion;

        [FindsBy(How = How.CssSelector, Using = "table[data-maintenance-grid-id='AssessmentGradesetInstanceForGrade']")]
        private IWebElement assessmentGradesetInstanceGrid;

        [FindsBy(How = How.CssSelector, Using = "table[data-maintenance-grid-id='AssessmentGradeGrid']")]
        private IWebElement assessmentGradesetGradeGrid;

        By newVersionStartdate = By.CssSelector("input[name*='InstanceStartDate']");

        /// <summary>
        /// Returns the Gradeset Name
        /// </summary>
        public string GetGradeSetName()
        {
            return Name.GetAttribute("value");
        }

        /// <summary>
        /// Returns the Gradeset Code
        /// </summary>
        public string GetGradeSetCode()
        {
            return Code.GetAttribute("value");
        }

        /// <summary>
        /// Returns the Gradeset Type
        /// </summary>
        public string GetGradeSetType()
        {
            return GradesetType.GetAttribute("value");
        }

        /// <summary>
        /// Returns the Minimun Value
        /// </summary>
        public string GetMinimumValue()
        {
            return MinimumValue.GetAttribute("value");
        }

        /// <summary>
        /// Returns the Maximum Value
        /// </summary>
        public string GetMaximumValue()
        {
            return MaximumValue.GetAttribute("value");
        }

        /// <summary>
        /// Returns the Start Date
        /// </summary>
        public string GetStartDate()
        {
            return StartDate.GetAttribute("value");
        }

        /// <summary>
        /// Returns the End Date
        /// </summary>
        public string GetEndDate()
        {
            return EndDate.GetAttribute("value");
        }

        /// <summary>
        /// Sets a gradeset name
        /// </summary>
        public void SetGradeSetName(string gradesetname)
        {
            Name.Clear();
            Name.SendKeys(gradesetname);
        }

        /// <summary>
        /// Sets Marks result type MinimumValue
        /// </summary>
        public void SetMinimumValue(string minimumValue)
        {
            MinimumValue.Clear();
            MinimumValue.SendKeys(minimumValue);
        }

        /// <summary>
        ///  Sets Marks result type MinimumValue
        /// </summary>
        public void SetMaximumValue(string maximumValue)
        {
            MaximumValue.Clear();
            MaximumValue.SendKeys(maximumValue);
        }

        /// <summary>
        /// Sets a gradeset description
        /// </summary>
        public void SetGradeSetDescription(string gradesetdescription)
        {
            Description.Clear();
            Description.SendKeys(gradesetdescription);
        }

        

        /// <summary>
        /// Sets code, description and value for a gradeset of type grade
        /// </summary>
        public void SetGrades(int rowno, string codename, string description, string value)
        {
            SetCode(rowno, codename);
            SetDescription(rowno, description);
            SetValue(rowno, value);
        }

        /// <summary>
        /// Sets code for a gradeset of type grade
        /// </summary>
        public void SetCode(int rowno, string codename)
        {
            ReadOnlyCollection<IWebElement> GradeSetCodeResultList = WebContext.WebDriver.FindElements(CodeList);
            int rowcount = 0;
            foreach (IWebElement eachelement in GradeSetCodeResultList)
            {
                if (rowcount == rowno - 1)
                {
                    eachelement.Clear();
                    eachelement.SendKeys(codename);
                    break;
                }
                rowcount++;
            }
        }

        /// <summary>
        /// Sets description for a gradeset of type grade
        /// </summary>
        public void SetDescription(int rowno, string description)
        {
            ReadOnlyCollection<IWebElement> GradeSetdescriptionResultList = WebContext.WebDriver.FindElements(DescriptionList);
            int rowcount = 0;
            foreach (IWebElement eachelement in GradeSetdescriptionResultList)
            {
                if (rowcount == rowno - 1)
                {
                    eachelement.Clear();
                    eachelement.SendKeys(description);
                    break;
                }
                rowcount++;
            }
        }

        /// <summary>
        /// Sets value for a gradeset of type grade
        /// </summary>
        public void SetValue(int rowno, string value)
        {
            ReadOnlyCollection<IWebElement> GradeSetValueResultList = WebContext.WebDriver.FindElements(ValueList);
            int rowcount = 0;
            foreach (IWebElement eachelement in GradeSetValueResultList)
            {
                if (rowcount == rowno - 1)
                {
                    eachelement.Clear();
                    eachelement.SendKeys(value);
                    break;
                }
                rowcount++;
            }
        }

        /// <summary>
        /// Returns all the GradeSet Description Results as a part of List and returns "No GradeSet Description Found" in case of No Result found.
        /// </summary>
        public List<string> GetGradeSetDescription()
        {
            List<string> GradeSetDescriptionResults = new List<string>();
            ReadOnlyCollection<IWebElement> GradeSetDescriptionResultList = WebContext.WebDriver.FindElements(DescriptionList);
            if (GradeSetDescriptionResultList.Count == 0)
            {
                GradeSetDescriptionResults.Add("No GradeSet Description Found");
                return GradeSetDescriptionResults;
            }

            else
            {
                foreach (IWebElement eachelement in GradeSetDescriptionResultList)
                {
                    GradeSetDescriptionResults.Add(eachelement.GetAttribute("value"));
                }
                return GradeSetDescriptionResults;
            }
        }

        /// <summary>
        /// Returns all the GradeSet Value Results as a part of List and returns "No GradeSet Value Found" in case of No Result found
        /// </summary>
        public List<string> GetGradeSetValue()
        {
            List<string> GradeSetValueResults = new List<string>();
            ReadOnlyCollection<IWebElement> GradeSetValueResultList = WebContext.WebDriver.FindElements(ValueList);
            if (GradeSetValueResultList.Count == 0)
            {
                GradeSetValueResults.Add("No GradeSet Value Found");
                return GradeSetValueResults;
            }

            else
            {
                foreach (IWebElement eachelement in GradeSetValueResultList)
                {
                    GradeSetValueResults.Add(eachelement.GetAttribute("value"));
                }
                return GradeSetValueResults;
            }
        }

        /// <summary>
        /// Finds the first instance field prefix.
        /// </summary>
        /// <returns></returns>
        public string FindDefaultInstanceFieldPrefix()
        {
            string Id = defaultGradesetVersion.GetAttribute("data-fieldprefix");
            return Id;
        }

        /// <summary>
        ///  Sets the value in the grade row.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="fieldPrefix"></param>
        /// <param name="code"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="colourStyle"></param>
        /// <param name="parentCode"></param>
        public void SetGradeRow(int index,string fieldPrefix,string code, string description, string value,bool colourStyle=false,string parentCode =null)
        {
            string prefix = fieldPrefix + ".AssessmentGrades";
            IWebElement defaultGradeRow = WebContext.WebDriver.FindElements(By.CssSelector("tr[data-row-name='" + prefix + "']"))[index];
            string gradeRowPrefix = defaultGradeRow.GetAttribute("data-fieldprefix");
            string gradeCode = gradeRowPrefix+".Code";
            string gradeDesc = gradeRowPrefix+".Description";
            string gradeValue = gradeRowPrefix+".Value";
            IWebElement gradesetCodeInput = WebContext.WebDriver.FindElement(By.Name(gradeCode));
            IWebElement gradesetDescInput = WebContext.WebDriver.FindElement(By.Name(gradeDesc));
            IWebElement gradesetValueInput = WebContext.WebDriver.FindElement(By.Name(gradeValue));
            gradesetCodeInput.Clear();
            gradesetCodeInput.SendKeys(code);
            gradesetDescInput.Clear();
            gradesetDescInput.SendKeys(description);
            gradesetValueInput.Clear();
            gradesetValueInput.SendKeys(value);
            // find the colour palette and set the colour style.
            if (colourStyle)
                this.AssignColour();      
            if(!String.IsNullOrEmpty(parentCode))
            {
                string gradeParent = gradeRowPrefix + ".Parent.dropdownImitator";
                IWebElement parentSelector = WebContext.WebDriver.FindElement(By.Name(gradeParent));
                SeleniumHelper.ChooseSelectorOption(parentSelector, parentCode);
            }
        }

        public GradesetDataMaintenance ConfirmUpdateToExistingInstance()
        {
            Thread.Sleep(5000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetConfirmationDialog));
            GradesetConfirmationSaveButton.Click();
            return new GradesetDataMaintenance();
        }

        public GradesetDataMaintenance SelectNewInstance()
        {
            Thread.Sleep(2000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetConfirmationDialog));
            newGradesetVersion.Click();
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// Assigns the first style from the colour picker.
        /// </summary>
        public void AssignColour()
        {
            // find the colour palette and set the colour style.
            IWebElement gradesetColourButton = WebContext.WebDriver.FindElement(By.CssSelector("div[data-stylepicker]"));
            gradesetColourButton.Click();
            Thread.Sleep(1000);
            //get the colour picker by class name.
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(ColourPickerList);
            int count = 0;
            foreach (IWebElement eachelement in List)
            {
                //skip the first one - the default is no colour.
                if (count != 0)
                {
                    eachelement.Click();
                    break;
                }
                count++;
            }
        }

        /// <summary>
        /// clicks the OK button the data maintenance screen
        /// </summary>
        public MarksheetTemplateProperties ClickOkButton()
        {
            OkButton.Click();
            while (true)
            {
                try
                {
                    if (OkButton.GetAttribute("disabled") != "true")
                        break;
                }
                catch
                {
                    break;
                }
            }
            return new MarksheetTemplateProperties();
        }


        /// <summary>
        /// clicks the Cancel button on the gradeset data maintenance screen
        /// </summary>
        public GradesetDataMaintenance ClickCancelButton()
        {
            CancelButton.Click();
            return null ;
        }

        /// <summary>
        /// clicks the Create button on the gradeset data maintenance screen
        /// </summary>
        public GradesetDataMaintenance ClickCreateButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateButton)).Click();            
            while (true)
            {
                if (CreateButton.GetAttribute("aria-expanded") == "true")
                    break;
            }
            return new GradesetDataMaintenance();
        }


        /// <summary>
        /// clicks to hide search Criateria
        /// </summary>
        public GradesetDataMaintenance ClickToHideSearchCriateriaButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_hideSearchPanel)).Click();
            //wait logic
            return this;
        }

        /// <summary>
        /// clicks the Add Grade Link for adding Grades in Grid on the gradeset data maintenance screen.
        /// </summary>
        public GradesetDataMaintenance ClickAddGradeVersionLink()
        {
            Thread.Sleep(2000);
            AddGradeVersionLink.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(versionStartDatePicker));
            while (true)
            {
                if (AddGradeVersionLink.GetAttribute("disabled") != "true")
                {
                    break;
                }
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// Adds a new row for the Grade grid.
        /// </summary>
        /// <param name="fieldPrefix"></param>
        /// <returns></returns>
        public GradesetDataMaintenance ClickAddGradeLink(string fieldPrefix)
        {
            addGradeButton.Click();
            Thread.Sleep(2000);            
            string prefix = fieldPrefix + ".AssessmentGrades";
            IWebElement newGradeRow = WebContext.WebDriver.FindElements(By.CssSelector("tr[data-row-name='" + prefix + "']"))[1];
            string gradeRowPrefix = newGradeRow.GetAttribute("data-fieldprefix");
            string gradeCode = gradeRowPrefix + ".Code";
            waiter.Until(ExpectedConditions.ElementIsVisible(By.Name(gradeCode)));
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks to open pop up for adding grades.
        /// </summary>
        /// <returns></returns>
        public EditGradesetVersion ClickEditGradesetVersionButton()
        {
            editGradesetVersionButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(editGradesetVersionHeader, "Edit Gradeset Version"));
            while (true)
            {
                if (editGradesetVersionButton.GetAttribute("disabled") != "true")
                {
                    break;
                }
            }
            return new EditGradesetVersion();
        }

        

        /// <summary>
        /// click to open Start date date picker.
        /// </summary>
        /// <returns></returns>
        public GradesetDataMaintenance ClickVersionStartDateButton()
        {
            Thread.Sleep(4000);
            versionStartDatePicker.Click();
            Thread.Sleep(2000);
            //versionStartDatePicker.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(gradesetVersionActiveDate));
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// click to set the current date from date picker.
        /// </summary>
        /// <returns></returns>
        public GradesetDataMaintenance ClickGradesetVersionActiveDate()
        {
            gradesetVersionActiveDate.Click();
            Thread.Sleep(2000);
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// click to expand the Version section.
        /// </summary>
        /// <returns></returns>
        public GradesetDataMaintenance ClickVersionLink()
        {
            GradesetVersionButton.Click();
           // waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetGradesButton));
            while (true)
            {
                if (GradesetVersionButton.GetAttribute("aria-expanded") == "true")
                {
                    break;
                }
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clickc to expand the Grade Section.
        /// </summary>
        /// <returns></returns>
        public GradesetDataMaintenance ClickGradesLink()
        {
            GradesetGradesButton.Click();
            while (true)
            {
                if (GradesetGradesButton.GetAttribute("aria-expanded") == "true")
                {
                    break;
                }    
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks the delete button.
        /// </summary>
        public GradesetDataMaintenance DeleteButtonClick()
        {
            GradesetDeleteButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(GradesetContinueButton));
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks to continue deletion.
        /// </summary>
        public GradesetDataMaintenance ContinueButtonClick()
        {
            GradesetContinueButton.Click();
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// return random text.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string GenerateRandomString(int length)
        {
            Random _random = new Random((int)DateTime.Now.Ticks);
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuwxyz";
            StringBuilder builder = new StringBuilder(length);

            for (int i = 0; i < length; ++i)
                builder.Append(chars[_random.Next(chars.Length)]);

            return builder.ToString();
        }

        /// <summary>
        /// Validates that the expected types of gradeset types are present.
        /// </summary>
        /// <returns></returns>
        public bool ValidateAllExpectedTypesArePresent()
        {
            bool allExpectedTypesPresent = false;
            if (CreateGradeGradeset != null && CreateMarksIntegerGradeset != null && CreateCommentGradeset!=null)
            {
                allExpectedTypesPresent = true;
            }
            return allExpectedTypesPresent;
        }

        /// <summary>
        /// clicks the Create button on the gradeset data maintenance screen
        /// </summary>
        public GradesetDataMaintenance SelectGradesetOption(string gradesettype)
        {
            switch (gradesettype)
            {
                case "Age":
                    CreateAgeGradeset.Click();
                    break;

                case "Grade":
                    waiter.Until(ExpectedConditions.ElementToBeClickable(CreateGradeGradeset));
                    CreateGradeGradeset.Click();
                    break;

                case "Marks":
                    waiter.Until(ExpectedConditions.ElementToBeClickable(CreateMarksIntegerGradeset));
                    CreateMarksIntegerGradeset.Click();
                    break;               

                case "Comment":
                    CreateCommentGradeset.Click();
                    break;
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks the Save button on the data maintenance screen
        /// </summary>
        public GradesetDataMaintenance ClickSaveButton()
        {
            GradesetSaveButton.Click();
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// save the gradeset.
        /// </summary>
        /// <returns></returns>
        public bool SaveMarksheetAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Assessment Result Type record saved"));
        }

        public bool ValidationMessageAssertion()
        {
            if (validationMessage != null)
                return true;
            return false;
        }

        /// <summary>
        /// Delete seleted gradeset.
        /// </summary>
        /// <returns></returns>
        public bool DeleteGrateSetAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(saveSuccessMessage_New));
            return waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "AssessmentGradeset record deleted"));
        }

        /// <summary>
        /// clicks the Cancel button on the data maintenance screen
        /// </summary>
        public GradesetDataMaintenance ClickToolBarCancelButton()
        {
            GradesetCancelButton.Click();
            while (true)
            {
                if (GradesetCancelButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// clicks the Delete button on the data maintenance screen
        /// </summary>
        public GradesetDataMaintenance ClickDeleteButton()
        {
            GradesetDeleteButton.Click();
            while (true)
            {
                if (GradesetDeleteButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new GradesetDataMaintenance();
        }

        /// <summary>
        /// Gradeset Insatnce Row count
        /// </summary>
        public string GetGradesetInstanceRowCount()
        {
            return assessmentGradesetInstanceGrid.GetAttribute("data-grid-row-count");
        }

        /// <summary>
        /// Gets the number of rows in the Grades grid.
        /// </summary>
        /// <returns></returns>
        public string getGradesetGradesRowCount()
        {
            return assessmentGradesetGradeGrid.GetAttribute("data-grid-row-count");
        }

        /// <summary>
        /// Sets a new version start date
        /// </summary>
        public void SetNewVersionStartdate(string date)
        {
            waiter.Until(ExpectedConditions.ElementExists(newVersionStartdate));           
            IWebElement startdate = WebContext.WebDriver.FindElement(newVersionStartdate);
            startdate.Clear();
            startdate.SendKeys(date);
        }

        public void ClickDeleteRowButton(int rowNumber)
        {
            IWebElement gridRow = assessmentGradesetGradeGrid.GetGridRow(rowNumber);
            if (gridRow != null)
            {
                IWebElement DeleteRowButton = gridRow.FindElement(SeleniumHelper.SelectByDataAutomationID("remove_button"));

                if (DeleteRowButton.Enabled == true)
                {
                    Thread.Sleep(1000);
                    DeleteRowButton.Click();
                    ClickYesButton();
                }
            }
        }

        public static void ClickDeleteRowButton()
        {
            throw new NotImplementedException();
        }

        public void ClickYesButton()
        {
            DeleteConfirmationDialog deletedialog = new DeleteConfirmationDialog();
            deletedialog.ClickYesButton();
        }
    }
}
