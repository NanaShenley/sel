using System;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Interactions;
using System.Threading;
using SharedComponents.BaseFolder;
using System.Collections.Generic;

namespace Assessment.Components.PageObject
{
    public class AspectDetails : BaseSeleniumComponents
    {
        public AspectDetails()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='aspect__details_header_title']")]
        private IWebElement AspectDetailsTitle;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        private IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_advanced']")]
        private IWebElement MoreDetailsButton;

        //[FindsBy(How = How.CssSelector, Using = "input[name='AspectName']")]
        //private IWebElement AspectSeacrhText;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter']")]
        private IWebElement NumberOfResults;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_aspect']")]
        private IWebElement CreateAspectButton;

        //[FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_gradesettype_aspect']")]
        //private IWebElement CreateAspectWithGradesetType;

        //[FindsBy(How = How.CssSelector, Using = "a[data-automation-id='create_commenttype_aspect']")]
        //private IWebElement CreateAspectWithCommentType;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='choose_gradeset_button']")]
        private IWebElement GradesetButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        private IWebElement ConfirmationDialogOKButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-target='[data-crud-screen]']")]
        private IWebElement SearchPanelShowHide;

        [FindsBy(How = How.CssSelector, Using = "input[name='Subject.dropdownImitator']")]
        public IWebElement SearchBySubjectDropdown;

        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentGradesetType.dropdownImitator']")]
        public IWebElement SearchByGradesetTypeDropdown;

        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentGradeset.dropdownImitator']")]
        public IWebElement SearchByGradesetDropdown;

        [FindsBy(How = How.CssSelector, Using = "input[name='ResourceProvider.dropdownImitator']")]
        public IWebElement SearchByResourceProviderDropdown;

        [FindsBy(How = How.CssSelector, Using = "input[name='IsStatutory']")]
        public IWebElement IsStatutory;

        //Aspect Details
        public static By AspectNameInput = By.CssSelector("input[name='Name']");
        public static By GradesetButtonBy = By.CssSelector("button[data-automation-id='choose_gradeset_button']");

        [FindsBy(How = How.CssSelector, Using = "textarea[name='Description']")]
        private IWebElement AspectDescription;
        [FindsBy(How = How.CssSelector, Using = "input[name='Name']")]
        private IWebElement AspectName;


        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentMode.dropdownImitator']")]
        public IWebElement AspectRecordingAssessment;
        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentMethod.dropdownImitator']")]
        public IWebElement AspectTeacherAssessment;
        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentPurpose.dropdownImitator']")]
        public IWebElement AspectMeasuring;
        [FindsBy(How = How.CssSelector, Using = "input[name='AssessmentGradeType.dropdownImitator']")]
        public IWebElement AspectType;
        [FindsBy(How = How.CssSelector, Using = "input[name='PhaseSelector.dropdownImitator']")]
        public IWebElement PhaseSelector;
        [FindsBy(How = How.CssSelector, Using = "input[name='SubjectLevel.dropdownImitator']")]
        public IWebElement Subject;        

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='well_know_action_save']")]
        public IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        private IWebElement DeleteButton;

        //[FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_error']")]
        //private IWebElement SaveErrorMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement SaveSuccessMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='status_success']")]
        private IWebElement DeleteSuccessMessage;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='confirm_delete_dialog']")]
        private readonly IWebElement DeleteConfirmationPopUpWindow = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='cancel_button']")]
        private readonly IWebElement CancelButton = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='continue_with_delete_button']")]
        private readonly IWebElement ContinueButton = null;

        //[FindsBy(How = How.CssSelector, Using = "div.validation-summary-errors")]
        //private IWebElement SaveValidationMessage;

        public static By GradesetDialogTitle = By.CssSelector("h4[data-automation-id='select_gradeset_-_popup_header_title']");
        private static By AspectSeacrhText = By.CssSelector("input[name='AspectName']");
        private static By ConfirmationDialog = By.CssSelector("div[data-automation-id = 'confirmation_required_dialog']");


        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks on the Search Button
        /// </summary>
        public AspectDetails Search()
        {
            Thread.Sleep(4000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton)).Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            Thread.Sleep(4000);
            return new AspectDetails();
        }

        /// <summary>
        /// Clicks on the More details Button
        /// </summary>
        public AspectDetails MoreDetailsClick()
        {
            Thread.Sleep(4000);
            //   waiter.Until(ExpectedConditions.TextToBePresentInElement(AspectDetailsTitle, "Aspect Details"));
            MoreDetailsButton.Click();
            Thread.Sleep(2000);
            return new AspectDetails();
        }

        /// <summary>
        /// Clicks on the Create Aspect Drodwon Button
        /// </summary>
        public void CreateAspect()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(CreateAspectButton));
            CreateAspectButton.Click();
        }

        ///// <summary>
        ///// Clicks on the Create CreateAspectWithGradesetType
        ///// </summary>
        //public void CreateAspectGradesetType()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(CreateAspectWithGradesetType));
        //    CreateAspectWithGradesetType.Click();
        //    waiter.Until(ExpectedConditions.ElementIsVisible(AspectNameInput));

        //}

        ///// <summary>
        ///// Clicks on the Create Aspect Drodwon Button
        ///// </summary>
        //public void CreateAspectCommentType()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(CreateAspectWithCommentType));
        //    CreateAspectWithCommentType.Click();
        //    waiter.Until(ExpectedConditions.ElementIsVisible(AspectNameInput));

        //}

        /// <summary>
        /// Clicks on the Create Aspect Drodwon Button
        /// </summary>
        public void SearchPanelShowHideClick()
        {
            SearchPanelShowHide.Click();

        }

        /// <summary>
        /// Clicks on the Create CreateAspectWithGradesetType
        /// </summary>
        public GradesetSearchPanel GradesetButtonClick()
        {
            WaitForElement(GradesetButtonBy);
            GradesetButton.Click();

            Thread.Sleep(2000);
            //waiter.Until(ExpectedConditions.TextToBePresentInElementLocated(By.CssSelector("h4[data-automation-id='select_gradeset_-_popup_header_title']"), "Select Gradeset - "));

            //Thread.Sleep(1000);
            return new GradesetSearchPanel();
        }


        /// <summary>
        /// Clicks on the Save
        /// </summary>
        public void SaveButtonClick()
        {
            SaveButton.Click();
            Thread.Sleep(2000);
            if (IsElementPresent((ConfirmationDialog)))
            {
                waiter.Until(ExpectedConditions.ElementToBeClickable(ConfirmationDialogOKButton));
                ConfirmationDialogOKButton.Click();
            }

        }

        /// <summary>
        /// checks if the element is present on the screen or not
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool IsElementPresent(By element)
        {
            try
            {
                WebContext.WebDriver.FindElement(element);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /// <summary>
        /// Clicks on the Delete 
        /// </summary>
        public void DeleteButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteButton));
            DeleteButton.Click();
        }

        /// <summary>
        /// Clicks on the Delete 
        /// </summary>
        public void ContinueButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(ContinueButton));
            ContinueButton.Click();

        }

        /// <summary>
        /// New Page Object for Assessment Period Lookup Search Panel
        /// </summary>
        public AspectDetails NewAssessmentPeriodLookupSearchPanelPageObject()
        {
            return new AspectDetails();
        }

        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public void SearchByAspectName(string SearchAspectName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AspectSeacrhText));
            WebContext.WebDriver.FindElement(AspectSeacrhText).Clear();
            Thread.Sleep(2000);
            WebContext.WebDriver.FindElement(AspectSeacrhText).SendKeys(SearchAspectName);
        }

        /// <summary>
        /// It sets the given Aspect name in the Name text field
        /// </summary>
        public String NumberofSearchResults()
        {
            String ResultCounter = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id='search_results_counter']")).Text;

            return ResultCounter;
        }

        /// <summary>
        /// It sets the given gradeset name in the Name text field
        /// </summary>
        public String getAspectNameInSearchresults()
        {
            String ResultCounter = WebContext.WebDriver.FindElement(By.CssSelector("a[class='search-result h1-result'] span[class='search-result-detail']")).Text;

            return ResultCounter;
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setAspectDescription(string SearchAspectDescription)
        {
            AspectDescription.Clear();
            AspectDescription.SendKeys(SearchAspectDescription);
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setAspectName(string SearchAspectName)
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(AspectNameInput));
            AspectName.Clear();
            AspectName.SendKeys(SearchAspectName);
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setSubjectforSearch(string mode)
        {

            //  AspectRecordingAssessment.Click();
            SearchBySubjectDropdown.Clear();
            SearchBySubjectDropdown.SendKeys(mode);
            Actions action = new Actions(WebContext.WebDriver);
            action.KeyDown(Keys.Enter).Perform();

            //AspectRecordingAssessment.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setAspectRecordingAssessment(string mode)
        {

            //  AspectRecordingAssessment.Click();
            AspectRecordingAssessment.Clear();
            AspectRecordingAssessment.SendKeys(mode);
            Actions action = new Actions(WebContext.WebDriver);
            action.KeyDown(Keys.Enter).Perform();

            //AspectRecordingAssessment.SendKeys(Keys.Enter);
        }
        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setAspectTeacherAssessment(string Method)
        {
            AspectTeacherAssessment.Clear();
            AspectTeacherAssessment.SendKeys(Method);
            AspectRecordingAssessment.SendKeys(Keys.Enter);
        }
        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setAspectMeasuring(string Purpose)
        {
            AspectMeasuring.Clear();
            AspectMeasuring.SendKeys(Purpose);
            AspectRecordingAssessment.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// It sets the given Aspect Description in the Description text field
        /// </summary>
        public void setLearningProject(string LearningProjecttext)
        {
            PhaseSelector.Clear();
            PhaseSelector.SendKeys(LearningProjecttext);
            AspectRecordingAssessment.SendKeys(Keys.Enter);
        }

        /// <summary>
        /// Returns the Success Message displayed on screen on save success
        /// </summary>
        public string GetSaveSuccessMessage()
        {
            Thread.Sleep(2000);
            return SaveSuccessMessage.Text;
        }

        /// <summary>
        ///Method to verify Save Marksheet Assertion Success Case
        /// </summary>
        public void SaveMarksheetAssertionSuccess()
        {
            waiter.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("div[data-automation-id='well_know_action_save']")));
            waiter.Until(ExpectedConditions.TextToBePresentInElement(SaveSuccessMessage, "Aspect record saved"));
            Console.WriteLine("\n{0}", SaveSuccessMessage.Text);
        }

        public void SetAssessmentDateFlag()
        {
            List<IWebElement> RadButtonList = new List<IWebElement>(WebContext.WebDriver.FindElements(By.Name("IsAssessmentDateRequired")));
            RadButtonList[0].Click();
        }
    }
}
