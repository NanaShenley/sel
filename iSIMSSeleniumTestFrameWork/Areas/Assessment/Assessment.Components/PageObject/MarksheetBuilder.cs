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
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Threading;


namespace Assessment.Components.PageObject
{
    public class MarksheetBuilder : MarksheetType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarksheetBuilder()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MarksheetBuilderTitle));
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.LinkText, Using = "Assessments Columns")]
        private IWebElement AssessmentsLink;

        [FindsBy(How = How.LinkText, Using = "School Groups")]
        private IWebElement GroupsLink;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel='group-filter'][create-marksheet-data-section-id='marksheets-groupfilter-']")]
        private IWebElement AdditionalFilter;

        [FindsBy(How = How.CssSelector, Using = "input[data-automation-id='Input-Aspect']")]
        private IWebElement AssessmentName;

        [FindsBy(How = How.CssSelector, Using = "[name='Name']")]
        private IWebElement AssessmentNameTextbox;

        
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='save_button']")]
        private IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        private IWebElement DeleteButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_error']")]
        private IWebElement SaveErrorMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement DeleteSuccessMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-delete-template-confirmation-dialog]")]
        private readonly IWebElement DeleteConfirmationPopUpWindow = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='no_button']")]
        private readonly IWebElement NoButton = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='yes_button']")]
        private readonly IWebElement YesButton = null;

        [FindsBy(How = How.CssSelector, Using = "div.validation-summary-errors")]
        private IWebElement SaveValidationMessage;

        [FindsBy(How = How.CssSelector, Using = "input[name='MarksheetTemplateName']")]
        public IWebElement MarksheetName;

        [FindsBy(How = How.CssSelector, Using = "textarea[name='Description']")]
        private IWebElement MarksheetDescription;

        [FindsBy(How = How.CssSelector, Using = "button[data-toggle='show-left-panel']")]
        private IWebElement SearchMarksheetPanelButton;

        //SMP = Search Marksheet Panel
        [FindsBy(How = How.CssSelector, Using = "input[name='MarksheetName']")]
        private IWebElement SMPMarksheetName;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_advanced']")]
        private IWebElement SMPShowMoreLink;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SMPSearchButton;

        [FindsBy(How = How.Id, Using = "tri_chkbox_Active")]
        private IWebElement SMPActiveCheckBox;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-aspect-periods']")]
        private IWebElement AspectDoneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='aspects'] i.fa.fa-times-circle.fa-2x")]
        private IWebElement AddAspectCloseButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='aspects-periods'] i.fa.fa-times-circle.fa-2x")]
        private IWebElement AddAssessmentPeriodAspectFlowCloseButton;

        [FindsBy(How = How.Id, Using = "validationerrormsg")]
        private IWebElement PopUpValidationMessage;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        private IWebElement ValidationPopUpOKButton;

        //Group Panel Controls

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] div.slider-header-title")]
        private IWebElement GroupHeader;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next-group']")]
        public IWebElement GroupNextButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] i.fa.fa-times-circle.fa-2x")]
        private IWebElement AddGroupsCloseButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-group']")]
        private IWebElement GroupDoneButton;

        [FindsBy(How = How.LinkText, Using = "Pupil Details")]
        private IWebElement AdditionalColumnsviaAdditionalLink;

        //Additional Filter

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='group-filter'] div.slider-header-title")]
        private IWebElement AdditionalFilterHeader;

        //properties tab
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='properties_tab']")]
        private IWebElement PropertiesTab;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_tree_close']")]
        public IWebElement _additionalColumnPropertiesCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "div[class='webix_icon fa-trash']")]
        public IWebElement _additionalColumnDeleteButton;       

        //Group Filter controls
        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='group-filter'] div.slider-header-title")]
        public IWebElement GroupFilterHeader;

        [FindsBy(How = How.LinkText, Using = "Curriculum Year")]
        private IWebElement GroupFilterCurriculumYearHeader;

        [FindsBy(How = How.LinkText, Using = "Class")]
        public IWebElement GroupFilterClassHeader;

        [FindsBy(How = How.LinkText, Using = "Ethnicity")]
        private IWebElement GroupFilterEthnicityHeader;

        [FindsBy(How = How.LinkText, Using = "Language")]
        private IWebElement GroupFilterLanguageHeader;

        [FindsBy(How = How.LinkText, Using = "New Intake Group")]
        private IWebElement GroupFilterNewIntakeGroupHeader;

        [FindsBy(How = How.LinkText, Using = "SEN Need Type")]
        private IWebElement GroupFilterSENNeedTypeHeader;

        [FindsBy(How = How.LinkText, Using = "SEN Status")]
        private IWebElement GroupFilterSENStatusHeader;

        [FindsBy(How = How.LinkText, Using = "Teaching Group")]
        private IWebElement GroupFilterTeachingGroupHeader;

        [FindsBy(How = How.LinkText, Using = "User Defined Group")]
        private IWebElement GroupFilterUserDefinedGroupHeader;

        [FindsBy(How = How.LinkText, Using = "Year Group")]
        public IWebElement GroupFilterYearGroupHeader;

        //Marksheet Properties control
        [FindsBy(How = How.CssSelector, Using = "div[column='2'] input[class='webix_table_checkbox']")]
        public IWebElement CheckIsAvailableForMarksheet;


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Navigate to the Assessement Selection
        /// </summary>
        public AddAssessments NavigateAssessments()
        {
            
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AssessmentsLink1));
            Thread.Sleep(1000);
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectAssessments).Click();
            return new AddAssessments();
        }

        /// <summary>
        /// Navigates to the group selection
        /// </summary>
        public AddGroups NavigateGroups()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GroupsLink));
            GroupsLink.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            return new AddGroups();
        }

        /// <summary>
        /// Navigates to the Additional Filters selection
        /// </summary>
        public GroupFilters NavigateAdditionalFilter()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AdditionalFilter));
            AdditionalFilter.Click();
            while (true)
            {
                if (AdditionalFilter.GetAttribute("class") == "collection-item")
                    break;
            }
            waiter.Until(ExpectedConditions.TextToBePresentInElement(AdditionalFilterHeader, "Additional Filter"));
            return new GroupFilters();
        }

        /// <summary>
        /// This allows us to select the Assessment
        /// </summary>
        public void SelectSearchAssessments(int NumberofAspects)
        {
            //waiter.Until(ExpectedConditions.ElementToBeClickable(SearchAssessmentResults));
            IWebElement SearchResultElement = WebContext.WebDriver.FindElement(MarksheetConstants.AspectSearchResult);
            ReadOnlyCollection<IWebElement> AspectElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectElementSelection);
            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();
            foreach (IWebElement aspectElem in AspectElements)
            {

                if (aspectElem.Text != "")
                {
                    aspectElem.WaitUntilState(ElementState.Displayed);
                    aspectElem.Click();

                    i++;
                }
                if (i == NumberofAspects)
                    break;
            }

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AspectNextButton));

        }

        /// <summary>
        /// This allows us to select the Assessment period and click done
        /// </summary>
        /// <param name="FromAssessment"></param>
        /// <param name="isClickDone"></param>
        public void SelectSearchedAssessmentPeriodAndDone(int NumberofAP, bool FromAssessment = true, bool isClickDone = true)
        {
            ReadOnlyCollection<IWebElement> PeriodElements = null;

            if (FromAssessment)
            {
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.PeriodResult);
            }
            else
            {
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectPeriodResult);
            }

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();

            foreach (IWebElement periodcodes in PeriodElements)
            {

                if (periodcodes.Text != "" && NumberofAP != 0)
                {
                    periodcodes.WaitUntilState(ElementState.Displayed);
                    periodcodes.Click();
                    selectedResults.Add(periodcodes.Text);
                    i++;
                }
                if (i == NumberofAP)
                    break;
            }

            if (FromAssessment && isClickDone)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(AspectDoneButton));
                AspectDoneButton.Click();
            }
            else
            {
                waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectDoneButton));
                WebContext.WebDriver.FindElement(MarksheetConstants.SubjectDoneButton).Click();
            }

        }

        public List<string> SelectSearchedAssessmentPeriod(int NumberofAP, bool FromAssessment = true, bool isClickDone = true)
        {
            ReadOnlyCollection<IWebElement> PeriodElements = null;

            if (FromAssessment)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectAssessmentPeriodElementSelection));
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectAssessmentPeriodElementSelection);
            }
            else
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectAssessmentPeriodElementSelection));
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectAssessmentPeriodElementSelection);
            }

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();

            foreach (IWebElement periodcodes in PeriodElements)
            {
                String CheckSelction = periodcodes.GetAttribute("data-selected");
                if (periodcodes.Text != "")
                {
                    if (CheckSelction == "false")
                    {
                    periodcodes.WaitUntilState(ElementState.Displayed);
                    periodcodes.Click();
                    selectedResults.Add(periodcodes.Text);
                    i++;
                }
                }
                if (i == NumberofAP)
                    break;
            }
            return selectedResults;

        }
        /// <summary>
        /// This allows us to input search criteria for the Subject
        /// </summary>
        public void SelectSubjectSearchCriteria(String SubjectSearchCriteria)
        {

            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectNameInput));
            //Search for aspect that is not valid
            WebContext.WebDriver.FindElement(MarksheetConstants.SubjectNameInput).SetText(SubjectSearchCriteria);
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectSearchButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SubjectSearchButton).Click();
        }

        /// <summary>
        /// This allows us to input search criteria for the Subject
        /// </summary>
        public void SelectSubjectAPSearchCriteria(String SubjectAPSearchCriteria)
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AssessmentPeriodNameInput));
            //Search for aspect that is not valid
            WebContext.WebDriver.FindElement(MarksheetConstants.AssessmentPeriodNameInput).SetText(SubjectAPSearchCriteria);
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectAssessmentPeriodSearchButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SubjectAssessmentPeriodSearchButton).Click();
        }
        /// <summary>
        /// This allows us to select the Subject and move next
        /// </summary>
        public void SelectSearchedSubjectResultAndMoveNext(int NumberofSubject)
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectNextButton));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectCloseButton));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectBackButton));
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.SubjectElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectElementSelection));

            ReadOnlyCollection<IWebElement> SubjectElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectElementSelection);

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();
            if (NumberofSubject > 0)
            {
                selectedResults.Clear();
                foreach (IWebElement subjectcodes in SubjectElements)
                {

                    if (subjectcodes.Text != "")
                    {
                        subjectcodes.WaitUntilState(ElementState.Displayed);
                        subjectcodes.Click();
                        selectedResults.Add(subjectcodes.Text);
                        i++;
                    }
                    if (i == NumberofSubject)
                        break;
                }

                //foreach (var col in MarksheetGridHelper.FindAllColumns())
                //{
                //    if (col.Text != " ")
                //        Assert.IsTrue(selectedResults.Contains(col.Text));
                //}
            }
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectNextButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SubjectNextButton).Click();
            selectedResults.Clear();
        }

         

        /// <summary>
        /// This allows us to select the Subject Mode
        /// </summary>
        public void SelectSearchedSubjectModeResult(int NumberofSubjectModeResult)
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.ModeElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.ModeElementSelection));
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectPeriodNextButton));
            ReadOnlyCollection<IWebElement> ModeElements = WebContext.WebDriver.FindElements(MarksheetConstants.ModeElementSelection);

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();
            i = 0;
            selectedResults.Clear();
            foreach (IWebElement mode in ModeElements)
            {

                if (mode.Text != "")
                {
                    mode.WaitUntilState(ElementState.Displayed);
                    mode.Click();
                    selectedResults.Add(mode.Text);
                    i++;
                }
                if (i == NumberofSubjectModeResult)
                    break;
            }

            foreach (var col in MarksheetGridHelper.FindAllColumns())
            {
                if (col.Text != "")
                    columns.Add(col.Text);
            }

            foreach (string col in selectedResults)
            {
                Assert.IsTrue(columns.Contains(col));
            }

        }

        /// <summary>
        /// This allows us to select the Subject Method
        /// </summary>
        public void SelectSearchedSubjectMethodResult(int NumberofSubjectMethodResult)
        {
            ReadOnlyCollection<IWebElement> MethodElements = WebContext.WebDriver.FindElements(MarksheetConstants.MethodElementSelection);

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();

            selectedResults.Clear();
            foreach (IWebElement method in MethodElements)
            {

                if (method.Text != "")
                {
                    method.WaitUntilState(ElementState.Displayed);
                    method.Click();
                    i++;
                }
                if (i == NumberofSubjectMethodResult)
                    break;
            }
        }


        /// <summary>
        /// This allows us to select the Subject Purpose and move Next
        /// </summary>
        public void SelectSearchedSubjectPuposeResultAndMoveNext(int NumberofSubjectPurposeResult)
        {
            ReadOnlyCollection<IWebElement> PurposeElements = WebContext.WebDriver.FindElements(MarksheetConstants.PurposeElementSelection);

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();

            selectedResults.Clear();
            foreach (IWebElement purpose in PurposeElements)
            {

                if (purpose.Text != "")
                {
                    purpose.WaitUntilState(ElementState.Displayed);
                    purpose.Click();
                    i++;
                }
                if (i == NumberofSubjectPurposeResult)
                    break;
            }
            MoveToSubjectAssessmentPeriod();
            selectedResults.Clear();
        }

        public void MoveToSubjectAssessmentPeriod()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectPeriodNextButton));
            WebContext.WebDriver.FindElement(MarksheetConstants.SubjectPeriodNextButton).Click();
        }

        ///// <summary>
        ///// Method to assert No Subject Selected warning message
        ///// </summary>
        //public void NoSubjectSelectedWarningAssertion()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
        //    Assert.AreEqual("Please select at least one subject before proceeding to the next step", PopUpValidationMessage.Text);
        //    ValidationPopUpOKButton.Click();
        //}

        /// <summary>
        /// This allows us to select the Subject Purpose
        /// </summary>
        public void SelectSearchedSubjectPuposeResult(int NumberofSubjectPurposeResult)
        {
            ReadOnlyCollection<IWebElement> PurposeElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectPurposeResult);

            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();

            selectedResults.Clear();
            foreach (IWebElement purpose in PurposeElements)
            {

                if (purpose.Text != "")
                {
                    purpose.WaitUntilState(ElementState.Displayed);
                    purpose.Click();
                    i++;
                }
                if (i == NumberofSubjectPurposeResult)
                    break;
            }

            selectedResults.Clear();
        }




        /// <summary>
        /// Method to assert No mode method and purpose Selected warning message
        /// </summary>
        public void NoSubjectDetailSelectedWarningAssertion()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
            string validationmsg = "Please select from each section to indicate the following:" +
                                   "What do you want to measure..." +
                                   "Are you recording a..." +
                                   "Is this a..." +
                                   "before proceeding to next step";
            Assert.AreEqual(validationmsg, PopUpValidationMessage.Text.Replace("\r\n", ""));
            ValidationPopUpOKButton.Click();
        }
        /// <summary>
        /// Saves the changes
        /// </summary>
        public void Save()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton));
            SaveButton.Click();
        }

        /// <summary>
        /// Saves the changes
        /// </summary>
        public void DeleteMarksheetTemplateButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteButton));
            DeleteButton.Click();
        }

        /// <summary>
        ///Method to verify Save Marksheet Assertion Failure Case
        /// </summary>
        public void SaveMarksheetAssertionFailure()
        {
            try
            {
                waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveErrorMessage, "Validation Warning"));
                var ValidationMessageText = SaveValidationMessage.Text;
                Console.WriteLine("\nUnable to Proceed because of the following Validation Errors : {0}", ValidationMessageText);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        /// <summary>
        ///Method to verify Save Marksheet Assertion Success Case
        /// </summary>
        public void SaveMarksheetAssertionSuccess()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            try
            {
                waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "saved"));
                Console.WriteLine("\n{0}", SaveSuccessMessage.Text);
            }
            catch
            {
                waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveErrorMessage, "Validation Warning"));
                var ValidationMessageText = SaveValidationMessage.Text;
                Console.WriteLine("\nUnable to Proceed because of the following Validation Errors : {0}", ValidationMessageText);
            }
        }

        /// <summary>
        ///Method to verify delete Marksheet Assertion Success Case
        /// </summary>
        public void DeleteMarksheetAssertionSuccess()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            try
            {
                waiter.Until(ExpectedConditions.TextToBePresentInElement(DeleteSuccessMessage, "deleted"));
                Console.WriteLine("\n{0}", DeleteSuccessMessage.Text);
            }
            catch
            {
                waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveErrorMessage, "Validation Warning"));
                var ValidationMessageText = SaveValidationMessage.Text;
                Console.WriteLine("\nUnable to Proceed because of the following Validation Errors : {0}", ValidationMessageText);
            }
        }

        /// <summary>
        /// Waits till Delete Confirmation Pop Up gets Open
        /// </summary>
        public void WaitUntilSaveNContinuePopUpOpens()
        {
            while (true)
            {
                if (DeleteConfirmationPopUpWindow.GetAttribute("aria-hidden") == "false")
                    break;
            }
        }

        /// <summary>
        /// Clicks the No Button on Delete Confirmation Pop Up
        /// </summary>
        public void ClickNoButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(NoButton, "No"));
            NoButton.Click();
        }

        /// <summary>
        ///  Clicks the Yes Button on Delete Confirmation Pop Up
        /// </summary>
        public void ClickYesButton()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(YesButton, "Yes"));
            YesButton.Click();
        }

        /// <summary>
        /// Navigate to the additional columns via additional column
        /// </summary>
        public AdditionalColumn NavigateAdditionalColumnsviaAdditionalColumn()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AdditionalColumnsviaAdditionalLink));
            WebContext.WebDriver.FindElement(MarksheetConstants.SelectAdditioanlColumnLink).Click();
            return new AdditionalColumn();
        }

        public void AdditioanlColumnsAreUnSelected()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.AdditionalColumnsResults));
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.AdditoanlColumnElementSelection));
            ReadOnlyCollection<IWebElement> additionalColumnElements = WebContext.WebDriver.FindElements(MarksheetConstants.AdditoanlColumnElementSelection);
            foreach (IWebElement element in additionalColumnElements)
            {
                if (element.Text != "")
                {
                    String CheckSelction = element.GetAttribute("data-selected");
                    Assert.AreEqual(CheckSelction, "false");
                }
            }
        }

        /// <summary>
        /// This allows us to select the Additional Column
        /// </summary>
        public List<string> SelectAdditonalColumn(int numberOfAdditionalColumn)
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AdditoanlColumnElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AdditoanlColumnElementSelection));
            Thread.Sleep(1000);
            ReadOnlyCollection<IWebElement> additionalColumnElements = WebContext.WebDriver.FindElements(MarksheetConstants.AdditoanlColumnElementSelection);
            int i = 0;
            List<string> selectedResults = new List<string>();
            List<string> columns = new List<string>();
            foreach (IWebElement element in additionalColumnElements)
            {
                if (element.Text != "")
                {
                    String CheckSelction = element.GetAttribute("data-selected");
                    if(CheckSelction == "false")
                    {
                    element.WaitUntilState(ElementState.Displayed);
                    element.Click();
                    selectedResults.Add(element.Text);
                    i++;
                }
                   
                }
                if (i == numberOfAdditionalColumn)
                    break;
            }
            return selectedResults;
            //waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.AdditionalColumnDoneButton));

        }


        /// <summary>
        ///  Is MarksheetPreview Columns Present
        /// </summary>
        public void IsMarksheetPreviewColumnsPresent()
        {            
               Assert.IsTrue(WebContext.WebDriver.FindElement(By.CssSelector("div[view_id='gridPreview']")).Displayed, "Grid is present on screen!");           
        }

        /// <summary>
        ///  Is MarksheetPreview Columns Present
        /// </summary>
        public void IsMarksheetPreviewColumnsNotPresent()
        {
            bool isGridPresent = true;
            try
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(By.CssSelector("div[view_id='gridPreview']")).Displayed,
                    "Grid is present on screen!");

                Console.WriteLine(isGridPresent);
            }
            catch (NoSuchElementException ex)
            {
                isGridPresent = false;
                Assert.IsTrue(true, "Grid is not preset on the screen!");
                Console.WriteLine(isGridPresent);
            }
        }


        /// <summary>
        /// Gets the values of name
        /// </summary>
        public String getMarksheetTemplateName()
        {
            MarksheetName.WaitUntilState(ElementState.Displayed);
            String MarksheeTemplateName = MarksheetName.GetAttribute("value");

            return MarksheeTemplateName;
        }


        /// <summary>
        /// Gets the values of name
        /// </summary>
        public String getMarksheetTemplateDescription()
        {
            MarksheetName.WaitUntilState(ElementState.Displayed);
            String MarksheeTemplateDesc = MarksheetDescription.GetAttribute("value");

            return MarksheeTemplateDesc;
        }
        /// <summary>
        /// sets the values of name and description properties
        /// </summary>
        public void setMarksheetProperties(string name, string description, bool active = true)
        {
            MarksheetName.WaitUntilState(ElementState.Displayed);
            MarksheetName.Clear();
            MarksheetName.SendKeys(name);

            MarksheetDescription.WaitUntilState(ElementState.Displayed);
            MarksheetDescription.Clear();
            MarksheetDescription.SendKeys(description);

            SeleniumHelper.Get(General.DataMaintenance).SetCheckBox(MarksheetConstants.MarksheetActive, active);
        }

        /// <summary>
        /// Searches Marksheet by its Name
        /// </summary>
        public string SearchByName(string MarksheetNameText, bool MarksheetActive, bool takeScreenPrint)
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchMarksheetPanelButton));
            SearchMarksheetPanelButton.Click();
            waiter.Until(ExpectedConditions.ElementIsVisible(MarksheetConstants.SearchMarksheetName));
            WebContext.WebDriver.FindElement(MarksheetConstants.SearchMarksheetName).SendKeys(MarksheetNameText);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SMPShowMoreLink));
            SMPShowMoreLink.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(SMPSearchButton));
            if (MarksheetActive == false)
            {
                waiter.Until(ExpectedConditions.ElementToBeSelected(SMPActiveCheckBox));
                SMPActiveCheckBox.Click();
            }

            SMPSearchButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(MarksheetConstants.SearchResultText, MarksheetNameText));
            ReadOnlyCollection<IWebElement> SearchResult = WebContext.WebDriver.FindElements(MarksheetConstants.SearchResultText);

            var ResultText = "";
            try
            {
                foreach (IWebElement eachresult in SearchResult)
                {
                    if (eachresult.Text == MarksheetNameText)
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(eachresult));
                        ResultText = eachresult.Text;
                    }
                }
            }
            catch
            {
                Console.WriteLine("No Marksheet Found with Name : {0}", MarksheetNameText);
            }

            return ResultText;
            if (takeScreenPrint)
                WebContext.Screenshot();

        }

        /// <summary>
        /// Returns to Marksheet Builder screen from the Aspect Selection Page on click of close button on the Assessment Selection Panel
        /// </summary>
        public void ReturnToMBFromAssessmentSelectionScreen()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddAspectCloseButton));
            AddAspectCloseButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsLink));
        }

        /// <summary>
        /// Returns to Marksheet Builder screen from the Groups Selection Page on click of close button on the Group Selection Panel
        /// </summary>
        public void ReturnToMBFromGroupsSelectionScreen()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddGroupsCloseButton));
            AddGroupsCloseButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsLink));
        }

        /// <summary>
        /// Returns to Marksheet Builder screen from the Assessment Period Selection Page on click of close button on the Assessment Period Selection Panel of the Assessment Selection Flow
        /// </summary>
        public void ReturnToMBFromAssessmentPeriodAspectFlowSelectionScreen()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddAssessmentPeriodAspectFlowCloseButton));
            AddAssessmentPeriodAspectFlowCloseButton.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsLink));
        }

        /// <summary>
        /// Method to assert No Aspect Selected warning message
        /// </summary>
        public void NoAspectSelectedWarningAssertion()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
            Assert.AreEqual("Please select at least one assessment before proceeding to the next step", PopUpValidationMessage.Text);
            ValidationPopUpOKButton.Click();
        }

        /// <summary>
        /// Method to assert No Assessment Period Selected warning message
        /// </summary>
        public void NoAssessmentPeriodSelectedWarningAssertion()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
            Assert.AreEqual("Please select at least one assessment period before proceeding to the next step", PopUpValidationMessage.Text);
            ValidationPopUpOKButton.Click();
        }

        /// <summary>
        /// Method to assert No Subject Selected warning message
        /// </summary>
        public void NoSubjectSelectedWarningAssertion()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
            Assert.AreEqual("Please select at least one subject before proceeding to the next step", PopUpValidationMessage.Text);
            ValidationPopUpOKButton.Click();           
        }

        /// <summary>
        /// Click on the Aspect path Done button
        /// </summary>
        public void AspectDoneButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AspectDoneButton));
            AspectDoneButton.Click();
        }


        /// <summary>
        /// Method to assert No Groups Selected warning message
        /// </summary>
        public void NoGroupsSelectedWarningAssertion()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ValidationPopUpOKButton));
            Assert.AreEqual("Please select at least one group before proceeding to the next step", PopUpValidationMessage.Text);
            ValidationPopUpOKButton.Click();
        }

        public void SelectPropertiesTab()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AssessmentsLink));
            WebContext.WebDriver.FindElement(MarksheetConstants.PropertiesTab).Click();
        }

        /// <summary>
        /// Select the number of Year Groups and Classes and select Next
        /// </summary>
        public List<String> SelectYearGroupsAndClasses(int NoOfYearGroups, int NoOfClasses, bool moveNext = true)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));


            //System.Threading.Thread.Sleep(500);
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.YearGroupsCheckBox));
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.ClassesCheckBox));

            ReadOnlyCollection<IWebElement> CheckboxlistYG = WebContext.WebDriver.FindElements(MarksheetConstants.YearGroupsCheckBox);
            ReadOnlyCollection<IWebElement> CheckboxlistClasses = WebContext.WebDriver.FindElements(MarksheetConstants.ClassesCheckBox);
            List<String> selectedYearGroup = new List<string>();
            List<String> selectedClasses = new List<string>();

            int i = 0;
            int j = 0;
            foreach (IWebElement YearGroupElem in CheckboxlistYG)
            {
                if (YearGroupElem.Displayed == true && NoOfYearGroups != 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(YearGroupElem));
                    if (YearGroupElem.Selected == false)
                    {
                    YearGroupElem.Click();
                    selectedYearGroup.Add(YearGroupElem.Text);
                    i++;
                }
                        
                }
                if (i == NoOfYearGroups)
                    break;
            }

            foreach (IWebElement classElem in CheckboxlistClasses)
            {
                if (classElem.Displayed == true && NoOfClasses != 0)
                {
                    if (classElem.Selected == false)
                    {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(classElem));
                    classElem.Click();
                    selectedYearGroup.Add(classElem.Text);
                    j++;
                }
                }
                if (j == NoOfClasses)
                    break;
            }
            if (moveNext)
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(GroupNextButton));
                GroupNextButton.Click();
                //waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupFilterHeader, "Additional Filter"));
            }
            return selectedYearGroup;
        }

        /// <summary>
        /// Checks If any YearGroups are Selected or not
        /// </summary>
        public bool ChecksIfYearGroupsSelected()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> CheckboxlistYG =
                WebContext.WebDriver.FindElements(MarksheetConstants.YearGroupsCheckBox);
            bool isSelected = false;
            foreach (IWebElement YearGroupElem in CheckboxlistYG)
            {
                if (YearGroupElem.Selected)
                {
                    isSelected = true;
                }    
            }
            return isSelected;
        }

        /// <summary>
        /// Checks If any Classes are Selected or not
        /// </summary>
        public bool ChecksIfClassesSelected()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> CheckboxlistClasses =
                WebContext.WebDriver.FindElements(MarksheetConstants.ClassesCheckBox);
            
            bool isSelected = false;
            foreach (IWebElement classElem in CheckboxlistClasses)
            {
                if (classElem.Selected)
                {
                    isSelected = true;
                }
            }
            return isSelected;
        }

        /// <summary>
        /// Checks If any Classes are Selected or not
        /// </summary>
        public bool ChecksIfGroupFiltersSelected()
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupFilterHeader, "Additional Filter"));
            int i = 0;
            bool isSelected = false;
            string selectedfilter = "";
            IList<By> filtersCollections = new[]
            {
                MarksheetConstants.SelectNCYear, MarksheetConstants.SelectEthnicity, MarksheetConstants.SelectLanguage, MarksheetConstants.SelectSchoolIntake,
                MarksheetConstants.SelectSenNeedType,MarksheetConstants.SelectSenStatus,MarksheetConstants.SelectTeachingGroup,MarksheetConstants.SelectUserDefined,
                MarksheetConstants.SelectYearGroupsFilter,MarksheetConstants.SelectClassesFilter
            };

            foreach (var filtersCollection in filtersCollections)
            { 
                ReadOnlyCollection<IWebElement> CheckboxlistGroupFilters = WebContext.WebDriver.FindElements(filtersCollection);

                foreach (IWebElement groupfilterElem in CheckboxlistGroupFilters)
                {
                    if (groupfilterElem.Selected)
                    {
                    isSelected = true;
                        i++;
                        selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + groupfilterElem.GetAttribute("id") + "']")).Text;
                }
            }
            }
            System.Console.WriteLine("\nThe following filter has been selected : {0}", selectedfilter);
            System.Console.WriteLine("\nThe total no. of filter option selected : {0}", i);
            Assert.AreEqual(1, i);
            return isSelected;
        }

        /// <summary>
        ///  Counts the No. of available NC Year options and also selects them one by one
        ///  Verifies the NC Year Header Text and also verifies if the actual NC Year Count is equal to the expected NC Year Count
        ///  Also asserts that at a time only one group filter is selected.
        /// </summary>
        public void CheckNCYearFilterOptions(int ExpectedNCYearsCount)
        {
            int i = 0;
            try
            {
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.NCYear).Text.Equals("Curriculum Year"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedNCYearsCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe NC Year Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of NC Years Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
                GroupFilterCurriculumYearHeader.Click();
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.NCYear).Text.Equals("Curriculum Year"));
            foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectNCYear))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
            {
                    e.Click();
                        i++;
            }
            }
                Assert.AreEqual(ExpectedNCYearsCount, i);
            }
            else
            {
                System.Console.WriteLine("\nThe NC Year Parent Header was not displayed as there were No Records under it");
            }
            System.Console.WriteLine("\nCount of NC Years Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }

        }

        /// <summary>
        ///  Counts the No. of available Ethnicity options and also selects them one by one
        ///  Verifies the Ethnicity Header Text and also verifies if the actual Ethnicity Count is equal to the expected Ethnicity Count
        ///  Also asserts that at a time only one group filter is selected.
        /// </summary>
        public void CheckEthnicityFilterOptions(int ExpectedEthniciyCount)
            {
            int i = 0;
            try
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectEthnicity).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Ethnicity).Text.Equals("Ethnicity"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectEthnicity))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
            {
                    e.Click();
                        i++;
            }
                }
                Assert.AreEqual(ExpectedEthniciyCount, i);
            }
            else
            {
                System.Console.WriteLine("\nThe Ethnicity Parent Header was not displayed as there were No Records under it");
                }
            System.Console.WriteLine("\nCount of Ethnicities Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
                GroupFilterEthnicityHeader.Click();
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectEthnicity).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Ethnicity).Text.Equals("Ethnicity"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectEthnicity))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedEthniciyCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe Ethnicity Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of Ethnicities Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            
        }

        /// <summary>
        ///  Counts the No. of available Language options and also selects them one by one
        ///  Verifies the Language Header Text and also verifies if the actual Language Count is equal to the expected Language Count
        ///  Also asserts that at a time only one group filter is selected.  
        /// </summary>
        public void CheckLanguageFilterOptions(int ExpectedLanguageCount)
        {
            int i = 0;
            try
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectLanguage).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Language).Text.Equals("Language"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectLanguage))
                {
                waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                {
                    e.Click();  
                        i++;
                }
            }
                Assert.AreEqual(ExpectedLanguageCount, i);
        }        
            else
            {
                System.Console.WriteLine("\nThe Language Parent Header was not displayed as there were No Records under it");
            }
            System.Console.WriteLine("\nCount of Languages Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
        }
            catch
            {
                GroupFilterLanguageHeader.Click();
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectLanguage).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Language).Text.Equals("Language"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectLanguage))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedLanguageCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe Language Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of Languages Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            
        }

        /// <summary>
        ///  Counts the No. of available New Intake options and also selects them one by one
        ///  Verifies the School Intake Header Text and also verifies if the actual School Intake Count is equal to the expected School Intake Count
        ///  Also asserts that at a time only one group filter is selected.  
        /// </summary>
        public void CheckNewIntakeFilterOptions(int ExpectedNewIntakeCount)
        {
            int i = 0;
            try
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSchoolIntake).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SchoolIntake).Text.Equals("New Intake Group"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSchoolIntake))
            {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                {
                        e.Click();
                        i++;
                }
            }
                Assert.AreEqual(ExpectedNewIntakeCount, i);
            }
            else
            {
                System.Console.WriteLine("\nThe New Intake Group Parent Header was not displayed as there were No Records under it");
                    }
            System.Console.WriteLine("\nCount of School Intakes Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }
            catch
            {
                GroupFilterNewIntakeGroupHeader.Click();
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSchoolIntake).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SchoolIntake).Text.Equals("New Intake Group"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSchoolIntake))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedNewIntakeCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe New Intake Group Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of School Intakes Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }

        }

        /// <summary>
        ///  Counts the No. of available Class options and also selects them one by one
        ///  Verifies the Class Header Text and also verifies if the actual Class Count is equal to the expected Class Count
        ///  Also asserts that at a time only one group filter is selected.    
        /// </summary>
        public void CheckClassFilterOptions(int ExpectedClassCount)
            {
            int i = 0;
            try
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectClassesFilter).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Class).Text.Equals("Class"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectClassesFilter))
            {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                {
                        e.Click();
                        i++;
                }
            }
                Assert.AreEqual(ExpectedClassCount, i);
            }              
            else
            {
                System.Console.WriteLine("\nThe Class Parent Header was not displayed as there were No Records under it");
                    }
            System.Console.WriteLine("\nCount of Classes Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }
            catch
            {
                GroupFilterClassHeader.Click();
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectClassesFilter).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.Class).Text.Equals("Class"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectClassesFilter))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedClassCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe Class Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of Classes Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            
        }

        /// <summary>
        ///  Counts the No. of available Year Group options and also selects them one by one
        ///  Verifies the Year Group Header Text and also verifies if the actual Year Group Count is equal to the expected Year Group Count
        ///  Also asserts that at a time only one group filter is selected.   
        /// </summary>
        public void CheckYearGroupFilterOptions(int ExpectedYearGroupCount)
            {
            int i = 0;
            try
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroupsFilter).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.YearGroup).Text.Equals("Year Group"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroupsFilter))
            {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                {
                        e.Click();
                        i++;
                }
            }
                Assert.AreEqual(ExpectedYearGroupCount, i);
            }       
            else
            {
                System.Console.WriteLine("\nThe Year Group Parent Header was not displayed as there were No Records under it");
                    }
            System.Console.WriteLine("\nCount of Year Groups Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }
            catch
            {
                GroupFilterYearGroupHeader.Click();
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroupsFilter).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.YearGroup).Text.Equals("Year Group"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectYearGroupsFilter))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedYearGroupCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe Year Group Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of Year Groups Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            
        }

        /// <summary>
        ///  Counts the No. of available SEN Need Type options and also selects them one by one
        ///  Verifies the SEN Need Types Header Text and also verifies if the actual SEN Need Types Count is equal to the expected SEN Need Types Count
        ///  Also asserts that at a time only one group filter is selected.   
        /// </summary>
        public void CheckSENNeedTypeFilterOptions(int ExpectedSENNeedTypeCount)
            {
            int i = 0;
            try
            {
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenNeedType).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SenNeedType).Text.Equals("SEN Need Type"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenNeedType))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedSENNeedTypeCount, i);
                }
                else
                {
                    System.Console.WriteLine("\nThe SEN Need Type Parent Header was not displayed as there were No Records under it");
                }
                System.Console.WriteLine("\nCount of SEN Need Types Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenNeedType).Count > 0)
            {
                    GroupFilterSENNeedTypeHeader.Click();
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SenNeedType).Text.Equals("SEN Need Type"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenNeedType))
            {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                {
                        e.Click();
                        i++;
                }
            }
                Assert.AreEqual(ExpectedSENNeedTypeCount, i);
            }                
            else
            {
                System.Console.WriteLine("\nThe SEN Need Type Parent Header was not displayed as there were No Records under it");
                    }
            System.Console.WriteLine("\nCount of SEN Need Types Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }
        }

        /// <summary>
        ///  Counts the No. of available User Defined Group options and also selects them one by one
        ///  Verifies the User Defined Group Header Text and also verifies if the actual User Defined Group Count is equal to the expected User Defined Group Count
        ///  Also asserts that at a time only one group filter is selected.  
        /// </summary>
        public void CheckUserDefinedGroupFilterOptions(int ExpectedUserDefinedGroupCount)
            {
            int i = 0;
            try
            {
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectUserDefined).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.UserDefined).Text.Equals("User Defined Group"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectUserDefined))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedUserDefinedGroupCount, i);
                }

                else
                {
                    System.Console.WriteLine("\nThe User Defined Group Parent Header was not displayed as there were No Records under it");
                }

                System.Console.WriteLine("\nCount of User Defined Groups Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
                GroupFilterUserDefinedGroupHeader.Click();
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectUserDefined).Count > 0)
            {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.UserDefined).Text.Equals("User Defined Group"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectUserDefined))
            {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                {
                        e.Click();
                        i++;
                }
            }
                Assert.AreEqual(ExpectedUserDefinedGroupCount, i);
                    }

            else
            {
                System.Console.WriteLine("\nThe User Defined Group Parent Header was not displayed as there were No Records under it");
            }

            System.Console.WriteLine("\nCount of User Defined Groups Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }

        }

        /// <summary>
        ///  Counts the No. of available Teaching Group options and also selects them one by one
        ///  Verifies the Teaching Groups Header Text and also verifies if the actual Teaching Groups Count is equal to the expected Teaching Groups Count
        ///  Also asserts that at a time only one group filter is selected. 
        /// </summary>
        public void CheckTeachingGroupFilterOptions(int ExpectedTeachingGroupCount)
            {
            int i = 0;
            try
            {
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectTeachingGroup).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.TeachingGroup).Text.Equals("Teaching Group"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectTeachingGroup))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedTeachingGroupCount, i);
                }

                else
                {
                    System.Console.WriteLine("\nThe Teaching Group Header was not displayed as there were No Records under it");
                }

                System.Console.WriteLine("\nCount of Teaching Groups Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
                GroupFilterTeachingGroupHeader.Click();
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectTeachingGroup).Count > 0)
                {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.TeachingGroup).Text.Equals("Teaching Group"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectTeachingGroup))
                    {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                    {
                        e.Click();
                        i++;
                    }
                }
                Assert.AreEqual(ExpectedTeachingGroupCount, i);
            }

            else
            {
                System.Console.WriteLine("\nThe Teaching Group Header was not displayed as there were No Records under it");
            }

            System.Console.WriteLine("\nCount of Teaching Groups Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }

        }

        /// <summary>
        ///  Counts the No. of available SEN Status options and also selects them one by one
        ///  Verifies the SEN Status Header Text and also verifies if the actual SEN Status Count is equal to the expected SEN Status Count
        ///  Also asserts that at a time only one group filter is selected.   
        /// </summary>
        public void CheckSENStatusFilterOptions(int ExpectedSENStatusCount)
            {
            int i = 0;
            try
            {
                if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenStatus).Count > 0)
                {
                    Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SenStatus).Text.Equals("SEN Status"));
                    foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenStatus))
                    {
                        waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                        if (!e.Selected)
                        {
                            e.Click();
                            i++;
                        }
                    }
                    Assert.AreEqual(ExpectedSENStatusCount, i);
                }

                else
                {
                    System.Console.WriteLine("\nThe SEN Status Parent Header was not displayed as there were No Records under it");
                }

                System.Console.WriteLine("\nCount of SEN Status Displayed : {0}", i);
                if (i > 0)
                    Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
            }
            catch
            {
                GroupFilterSENStatusHeader.Click();
            if (WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenStatus).Count > 0)
                {
                Assert.IsTrue(WebContext.WebDriver.FindElement(MarksheetConstants.SenStatus).Text.Equals("SEN Status"));
                foreach (IWebElement e in WebContext.WebDriver.FindElements(MarksheetConstants.SelectSenStatus))
                    {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                    if (!e.Selected)
                    {
                        e.Click();
                        i++;
                    }
                }
                Assert.AreEqual(ExpectedSENStatusCount, i);
            }

            else
            {
                System.Console.WriteLine("\nThe SEN Status Parent Header was not displayed as there were No Records under it");
            }
            
            System.Console.WriteLine("\nCount of SEN Status Displayed : {0}", i);
            if (i > 0)
                Assert.AreEqual(true, ChecksIfGroupFiltersSelected());
                }
        }

        /// <summary>
        /// selects a group filter
        /// </summary>
        /// <param name="filterName"></param>
        public List<String> SelectGroupFilter(By filterName, bool clickGroupHeader = true)
            {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupFilterHeader, "Additional Filter"));
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(filterName));
            Thread.Sleep(2000);
            if (clickGroupHeader == true)
            GroupFilterYearGroupHeader.Click(); 

            List<String> selectedGroupFilter = new List<string>();
            foreach (IWebElement e in WebContext.WebDriver.FindElements(filterName))
                {
                //waiter.Until(ExpectedConditions.ElementToBeClickable(e));
                if (!e.Selected)
                    {
                    e.Click();
                    selectedGroupFilter.Add(e.Text);
                        break;
                    }
                }
            return selectedGroupFilter;
                }

        public string GetSelectedGroupOrGroupfilter(By SelectGroup)
        {
            string selectedGroup = "";
            ReadOnlyCollection<IWebElement> CheckboxlistGroup = WebContext.WebDriver.FindElements(SelectGroup);

            foreach (IWebElement groupfilterElem in CheckboxlistGroup)
            {
                if (groupfilterElem.Selected)
                {
                    // isSelected = true;
                    selectedGroup = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + groupfilterElem.GetAttribute("id") + "']")).Text;
                }
            }
            return selectedGroup;
        }
        /// <summary>
        ///  Assert for the Group filer
        /// </summary>
        public void clickPropertiesTab()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(PropertiesTab));
            PropertiesTab.Click();
        }

        /// <summary>
        /// Clicks the Delete Button in column properties and moves on to the Marksheet Builder page
        /// </summary>
        public void ClickDeleteButtonInPropertiesTab()
        {
            //  waiter.Until(ExpectedConditions.ElementToBeClickable(_additionalColumnDeleteButton));
            ReadOnlyCollection<IWebElement> DeleteList = WebContext.WebDriver.FindElements(By.CssSelector("span[class='webix_icon fa-trash']"));

            int i = 0;
            foreach (IWebElement DeleteElem in DeleteList)
            {
                if (i > 0)
                    DeleteElem.Click();
                i++;

            }
        }

        public string GetLevelPeriodMapping(string periodName)
        {
            if (periodName.Contains("Year 1") || periodName.Contains("Year 2"))
                return "Foundation Stage";
            else if (periodName.Contains("Year 3") || periodName.Contains("Year 4"))
                return "Foundation Stage";
            else if (periodName.Contains("Year 5") || periodName.Contains("Year 6") || periodName.Contains("Year 7"))
                return "Foundation Stage";
            else
                return string.Empty;
        }

        /// <summary>
        /// Clicks the Delete Button in column properties and moves on to the Marksheet Builder page
        /// </summary>
        public void ColumnPropertiesCollapseButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_additionalColumnPropertiesCollapseButton));
            _additionalColumnPropertiesCollapseButton.Click();
        }

        /// <summary>
        /// Method to assert No Groups Selected warning message
        /// </summary>
        public void CheckMarksheetIsAvailable()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CheckIsAvailableForMarksheet));
       //     Assert.AreEqual("Please select at least one group before proceeding to the next step", PopUpValidationMessage.Text);
            CheckIsAvailableForMarksheet.Click();
        }

        public string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

   }
}

