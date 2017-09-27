using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;
using System.Collections.ObjectModel;
using System.Threading;

namespace Assessment.Components.PageObject
{
    public class AddAssessmentPeriod : BaseSeleniumComponents
    {

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] input[data-automation-id='Input-Aspect-Assessment-periods']")]
        private readonly IWebElement _assessmentPeriod = null;

        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] button[data-automation-id='search_criteria_submit']")]
        private readonly IWebElement _btnSearch = null;

        //[FindsBy(How = How.CssSelector, Using = "i[data-automation-id='aspectassessmentperiodclosebutton']")]
        //private readonly IWebElement _btnAspectClose = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-aspect-periods']")]
        public readonly IWebElement _btnAspectAssessmentPeriodDone = null;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='aspect-assessment-period-back']")]
        private readonly IWebElement _btnAspectAssessmentPeriodBack = null;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-subject-periods']")]
        public readonly IWebElement _btnSubjectAssessmentPeriodDone = null;

        [FindsBy(How = How.CssSelector, Using = "i[data-automation-id='subject-assessment-period-back']")]
        private readonly IWebElement _btnSubjectAssessmentPeriodBack = null;

        //[FindsBy(How = How.CssSelector, Using = "i[data-automation-id='subjectassessmentperiodclosebutton']")]
        //private readonly IWebElement _btnSubjectClose = null;

        //[FindsBy(How = How.CssSelector, Using = "div[data-section-id='marksheets-aspect-assessmentperiod-searchResults'] input[id='AssessmentPeriodName']")]
        //private readonly IWebElement AssessmentPeriodName = null;

        public AddAssessmentPeriod()
        {
           PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void EnterSearchName(string Apname)
        {
            WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] input[data-automation-id='Input-Aspect-Assessment-periods']"));
            _assessmentPeriod.Clear();
           _assessmentPeriod.SendKeys(Apname);
        }

        public void ClickSearch()
        {
            WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("form[data-section-id='marksheets-aspect-assessmentperiod-searchCriteria'] button[data-automation-id='search_criteria_submit']"));
            _btnSearch.Click();
        }

        //public MarksheetBuilder ClickClose()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(_btnAspectClose));
        //  //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
        //    _btnAspectClose.Click();
        //    return new MarksheetBuilder();
        //}
         
        public MarksheetBuilder ClickAspectAssessmentPeriodDone()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnAspectAssessmentPeriodDone));
          //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
            Thread.Sleep(2000);
            _btnAspectAssessmentPeriodDone.Click();
            return new MarksheetBuilder();
        }

        public AddAspects ClickAspectAssessmentPeriodBack()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnAspectAssessmentPeriodBack));
          //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
            _btnAspectAssessmentPeriodBack.Click();
            Thread.Sleep(2000);
            return new AddAspects();
        }

        public MarksheetBuilder ClickSubjectAssessmentPeriodDone()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectAssessmentPeriodDone));
            //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
            _btnSubjectAssessmentPeriodDone.Click();
            return new MarksheetBuilder();
        }

        public AddModeMethodPurpose ClickSubjectAssessmentPeriodBack()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectAssessmentPeriodBack));
            //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
            _btnSubjectAssessmentPeriodBack.Click();
            return new AddModeMethodPurpose();
        }

        //public MarksheetBuilder ClickSubjectAssessmentPeriodClose()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(_btnSubjectClose));
        //    //  WaitForElement(new TimeSpan(0, 0, 0, 15), By.CssSelector("i[data-automation-id='aspectassessmentperiodclosebutton']"));
        //    _btnSubjectClose.Click();
        //    return new MarksheetBuilder();
        //}

        //Returns a list of Assessment periods through Aspect flow
        public ReadOnlyCollection<IWebElement> AspectAssessmentPeriodList()
        {
                ReadOnlyCollection<IWebElement> PeriodElements = null;
                waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(MarksheetConstants.AspectAssessmentPeriodElementSelection));                    
                waiter.Until(ExpectedConditions.ElementToBeClickable(MarksheetConstants.AspectAssessmentPeriodElementSelection));   
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.AspectAssessmentPeriodElementSelection);
                return PeriodElements;
        }


        //Returns a list of Assessment periods through Subject flow
        public ReadOnlyCollection<IWebElement> SubjectAssessmentPeriodList()
        {
                ReadOnlyCollection<IWebElement> PeriodElements = null;
                PeriodElements = WebContext.WebDriver.FindElements(MarksheetConstants.SubjectAssessmentPeriodElementSelection);
                return PeriodElements;
        }

        // Selects Assessment period through Subject path with the number of AP passed
        public List<String> subjectAssessmentPeriodSelection(int NumberofAssessmentPeriod)
        {
            ReadOnlyCollection<IWebElement> subjectAssessmentPeriods = SubjectAssessmentPeriodList();
            List<String> selectedResults = SelectAssessmentPeriod(subjectAssessmentPeriods, NumberofAssessmentPeriod);
            return selectedResults;
        }

        // Selects Assessment period through Aspect path with the number of AP passed
        public List<String> AspectAssessmentPeriodSelection(int NumberofAssessmentPeriod)
        {
            ReadOnlyCollection<IWebElement> aspectAssessmentPeriods = AspectAssessmentPeriodList();
            List<String> selectedResults = SelectAssessmentPeriod(aspectAssessmentPeriods, NumberofAssessmentPeriod);
            return selectedResults;
        }

        //Ticks the checkbox of Assessment period through the selected path and number of selections passed as parameters
        public List<String> SelectAssessmentPeriod(ReadOnlyCollection<IWebElement> AssessmentPeriodList, int NumberofAssessmentPeriod)
        {
            int i = 0;
             ReadOnlyCollection<IWebElement> PeriodElements = AssessmentPeriodList;
            List<String> selectedResults = new List<string>();
             foreach (IWebElement periodcodes in PeriodElements)
            {
                String CheckSelction = periodcodes.GetAttribute("data-selected");
                if (periodcodes.Text != "" && NumberofAssessmentPeriod > 0)
                {
                    if (CheckSelction == "false")
                    {
                        periodcodes.WaitUntilState(ElementState.Displayed);
                        periodcodes.Click();
                        selectedResults.Add(periodcodes.Text);
                        i++;
                    }
                }
                if (i == NumberofAssessmentPeriod)                
                    break;                            
             }
             return selectedResults;
        }

    
        public String checkAssessmentPeriodIsNotSelcted(ReadOnlyCollection<IWebElement> AssessmentPeriodList)
        {
            ReadOnlyCollection<IWebElement> AspectList = AssessmentPeriodList;
            String CheckSelction = "";
            foreach (IWebElement aspectElem in AspectList)
            {
                if (aspectElem.Text != "")
                {
                    CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                    {
                        return CheckSelction;
                        break;
                    }
                }
            }
            return CheckSelction;
        }

        public List<String> getAspectAssessmentPeriodSelectedItems()
        {
            ReadOnlyCollection<IWebElement> aspectAPList = AspectAssessmentPeriodList();
            List<String> selectedAspectAPList = getAssessmentPeriodSelectedItems(aspectAPList);
            return selectedAspectAPList;
        }

        public List<String> getSubjectAssessmentPeriodSelectedItems()
        {
            ReadOnlyCollection<IWebElement> subjectAPList = SubjectAssessmentPeriodList();
            List<String> selectedSubjectAPList = getAssessmentPeriodSelectedItems(subjectAPList);
            return selectedSubjectAPList;
        }

        public List<String> getAssessmentPeriodSelectedItems(ReadOnlyCollection<IWebElement> AssessmentPeriodList)
        {

            ReadOnlyCollection<IWebElement> AspectList = AssessmentPeriodList;

            List<string> checkAPselected = new List<string>();
            String CheckSelction = "";
            foreach (IWebElement aspectElem in AspectList)
            {
                if (aspectElem.Text != "")
                {
                    CheckSelction = aspectElem.GetAttribute("data-selected");
                    if (CheckSelction == "true")
                        checkAPselected.Add(aspectElem.Text);
                }
            }
            return checkAPselected;
        }
    
        
        //public ReadOnlyCollection<IWebElement> SubjectAssessmentPeriodList(int NumberofAssessmentPeriod)
        //{
        //   int i = 0;
        //    ReadOnlyCollection<IWebElement> SubjectAssessmentPeriodList = SubjectAssessmentPeriodList();
        //    foreach (IWebElement periodcodes in SubjectAssessmentPeriodList)
        //    {
        //        String CheckSelction = periodcodes.GetAttribute("data-selected");
        //        if (periodcodes.Text != "")
        //        {
        //            if (CheckSelction == "false")
        //            {
        //                periodcodes.WaitUntilState(ElementState.Displayed);
        //                periodcodes.Click();
        //                selectedResults.Add(periodcodes.Text);
        //                i++;
        //            }
        //        }
        //        if (i == NumberofAssessmentPeriod)
        //            break;
        //    }
        //    return selectedResults;
        //}
        
    }

    public class PageObjectBridge<T>
    {
        public ReadOnlyCollection<IWebElement> selectedElements { get; set; }
        public T pageObject { get; set; }
    }



      
}
