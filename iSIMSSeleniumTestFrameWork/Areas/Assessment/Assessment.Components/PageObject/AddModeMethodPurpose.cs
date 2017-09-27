using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class AddModeMethodPurpose : BaseSeleniumComponents 
    {

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next-type-periods']")]
        public readonly IWebElement _btnSubjectNext = null;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='mode-back']")]
        private readonly IWebElement _btnModeBack = null;

        //[FindsBy(How = How.CssSelector, Using = "i[data-automation-id='modeclosebutton']")]
        //private readonly IWebElement _btnModeClose = null;

        public AddModeMethodPurpose()
        {
           PageFactory.InitElements(WebContext.WebDriver, this);
        }

        /// <summary>
        /// This returns the list of selected Subject modes
        /// </summary>                
        public List<String> modeAssessmentPeriodSelection(int NumberofAssessmentPeriod)
        {
            ReadOnlyCollection<IWebElement> subjectAssessmentPeriods = modeSubjectList();
            List<String> selectedResults = SelectModeMethodPurpose(subjectAssessmentPeriods, NumberofAssessmentPeriod);
            return selectedResults;
        }
        
        /// <summary>
        /// This returns the list of selected Subject method
        /// </summary>
        public List<String> methodAssessmentPeriodSelection(int NumberofAssessmentPeriod)
        {
            ReadOnlyCollection<IWebElement> subjectAssessmentPeriods = methodSubjectList();
            List<String> selectedResults = SelectModeMethodPurpose(subjectAssessmentPeriods, NumberofAssessmentPeriod);
            return selectedResults;
        }

        /// <summary>
        /// This returns the list of selected Subject purpose
        /// </summary>
        public List<String> purposeAssessmentPeriodSelection(int NumberofPurposetobeselected)
        {
            ReadOnlyCollection<IWebElement> subjectAssessmentPeriods = purposeSubjectList();
            List<String> selectedResults = SelectModeMethodPurpose(subjectAssessmentPeriods, NumberofPurposetobeselected);
            return selectedResults;
        }


        /// <summary>
        /// Selection of purpose
        /// </summary>
        public void purposeAssessmentPeriodSelectionByName(String PurposeName)
        {
            ReadOnlyCollection<IWebElement> subjectAssessmentPeriods = purposeSubjectList();
            SelectModeMethodPurposebyName(subjectAssessmentPeriods, PurposeName);
            
        }

        /// <summary>
        /// This returns the list of Subject modes
        /// </summary>                
        public ReadOnlyCollection<IWebElement> modeSubjectList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.ModeElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.ModeElementSelection));
            ReadOnlyCollection<IWebElement> modeSubjectList = WebContext.WebDriver.FindElements(MarksheetConstants.ModeElementSelection);
            return modeSubjectList;
        }

        /// <summary>
        /// This returns the list of Subject modes
        /// </summary>                
        public List<String> modeSelctedList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.ModeElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.ModeElementSelection));
            List<String> selectedMode = getSelectedItemsfromCollection(WebContext.WebDriver.FindElements(MarksheetConstants.ModeElementSelection));
            return selectedMode;
        }

        /// <summary>
        /// This returns the list of Subject modes
        /// </summary>                
        public List<String> methodSelctedList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MethodElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.MethodElementSelection));
            List<String> selectedMethod = getSelectedItemsfromCollection(WebContext.WebDriver.FindElements(MarksheetConstants.MethodElementSelection));
            return selectedMethod;
        }

        /// <summary>
        /// This returns the list of Subject method
        /// </summary>
        public ReadOnlyCollection<IWebElement> methodSubjectList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.MethodElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.MethodElementSelection));
            ReadOnlyCollection<IWebElement> methodSubjectList = WebContext.WebDriver.FindElements(MarksheetConstants.MethodElementSelection);
            return methodSubjectList;
        }

        /// <summary>
        /// This returns the list of Subject purpose
        /// </summary>
        public ReadOnlyCollection<IWebElement> purposeSubjectList()
        {
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.PurposeElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.PurposeElementSelection));
            ReadOnlyCollection<IWebElement> purposeSubjectList = WebContext.WebDriver.FindElements(MarksheetConstants.PurposeElementSelection);
            return purposeSubjectList;
        }

        /// <summary>
        /// This allows us to select the number of Subject Mode, Method and purpose as specified by user
        /// </summary>
        public List<String> SelectModeMethodPurpose(ReadOnlyCollection<IWebElement> ModeMethodPurposelistSelection, int NumberofSubjectModeResult)
        {
            ReadOnlyCollection<IWebElement> ModeElements = ModeMethodPurposelistSelection;

            int i = 0;
            String selection = "";
            List<String> selectedResults = new List<String>();
            foreach (IWebElement mode in ModeElements)
            {

                selection = mode.GetAttribute("data-selected");
                if (selection == "true")
                    i++;
                if (mode.Text != "" && selection == "false")
                {
                    mode.WaitUntilState(ElementState.Displayed);
                    waiter.Until(ExpectedConditions.ElementToBeClickable(mode));
                    mode.Click();
                    selectedResults.Add(mode.Text);
                    i++;
                }
                if (i == NumberofSubjectModeResult)
                    break;
            }
            return selectedResults;
        }

        /// <summary>
        /// This allows us to select the Subject Mode, Method and purpose as specified by user
        /// </summary>
        public void SelectModeMethodPurposebyName(ReadOnlyCollection<IWebElement> ModeMethodPurposelistSelection, String Modemethodpurposebyname)
        {
            ReadOnlyCollection<IWebElement> ModeElements = ModeMethodPurposelistSelection;

            String selection = "";
            List<String> selectedResults = new List<String>();
            foreach (IWebElement mode in ModeElements)
            {
                selection = mode.GetAttribute("data-selected");
                if (selection == "true")
                    break;
                if (mode.Text == Modemethodpurposebyname)
                {
                    mode.WaitUntilState(ElementState.Displayed);
                    waiter.Until(ExpectedConditions.ElementToBeClickable(mode));
                    mode.Click();
                }             
            }
        }

        /// <summary>
        /// This allows us to select the Subject Mode, Method and purpose as specified by user
        /// </summary>
        public List<String> getSelectedItemsfromCollection(ReadOnlyCollection<IWebElement> ModeMethodPurposelistSelection)
        {
            ReadOnlyCollection<IWebElement> ModeElements = ModeMethodPurposelistSelection;

            String selection = "";
            List<String> selectedResults = new List<String>();
            foreach (IWebElement mode in ModeElements)
            {
                selection = mode.GetAttribute("data-selected");
                if (selection == "true")
                    selectedResults.Add(mode.Text);                
            }
            return selectedResults;
        }
        /// <summary>
        /// For moving over to Assessment period through Mode Method and purpose
        /// </summary>
        public AddAssessmentPeriod modeMethodPurposeNextButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectNext));
            _btnSubjectNext.Click();
            while (true)
            {
                if (_btnSubjectNext.GetAttribute("disabled") != "true")
                    break;
            }
            return new AddAssessmentPeriod();
        }

        /// <summary>
        /// For moving over to Assessment period through Mode Method and purpose
        /// </summary>
        public AddSubjects modeMethodPurposeBackButton()
        {
           // waiter.Until(ExpectedConditions.ElementToBeClickable(_btnModeBack));
            _btnModeBack.Click();
            waiter.Until(ExpectedConditions.ElementExists(MarksheetConstants.SubjectElementSelection));
            waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectNextButton));
            //waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.SubjectAssessmentPeriodElementSelection));
            return new AddSubjects();
        }

        //public MarksheetBuilder modeMethodPurposeCloseButton()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(_btnModeClose));
        //    _btnModeClose.Click();
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AssessmentsLink1));
        //    return new MarksheetBuilder();
        //}


        /// <summary>
        /// Checks if all the modes are unselected
        /// </summary>
        public String checkModeIsNotSelcted()
        {
            ReadOnlyCollection<IWebElement> modeList = modeSubjectList();
            String checkMode = checkSubjectPropertyIsNotSelcted(modeList);
            return checkMode;
        }

        /// <summary>
        /// Checks if all the methods are unselected
        /// </summary>        
        public String checkMethodIsNotSelcted()
        {
            ReadOnlyCollection<IWebElement> methodList = methodSubjectList();
            String checkMethod = checkSubjectPropertyIsNotSelcted(methodList);
            return checkMethod;
        }

        /// <summary>
        /// Checks if all the purposes are unselected
        /// </summary>        
        public String checkPurposeIsNotSelcted()
        {
            ReadOnlyCollection<IWebElement> purposeList = purposeSubjectList();
            String checkPurpose = checkSubjectPropertyIsNotSelcted(purposeList);
            return checkPurpose;
        }

        /// <summary>
        /// Checks if subject properties which is passed into this method are unselected
        /// </summary>        
        public String checkSubjectPropertyIsNotSelcted(ReadOnlyCollection<IWebElement> subjectPropertiesList)
        {
            String CheckSelction = "";
            foreach (IWebElement aspectElem in subjectPropertiesList)
            {
                if (aspectElem.Text != "")
                {
                    CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                        break;
                }
            }
            return CheckSelction;
        }
    }
}
