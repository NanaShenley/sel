using Facilities.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Facilities.Components.Facilities_Pages
{
    public class AttendanceKPIPages : BaseFacilitiesPage
    {
        #pragma warning disable 0649
        // [FindsBy(How = How.CssSelector, Using = ".layout-col .main .pane")] // TODO need to remove
        [FindsBy(How = How.Id, Using = "editableData")]
        private readonly IWebElement _main;

        [FindsBy(How = How.CssSelector, Using = ("[data-automation-id='search_criteria']"))]
        private readonly IWebElement _search;

        #pragma warning restore 0649
        
        //TODO : to be replaced with data-automation-id once it has been added
        //Feedback story id - 8720
        private IWebElement _validationContainerCssSelector
        {
            get
            {
                return WaitForAndGet(By.CssSelector("[class='validation-summary-errors']"));
            }
        }

        private IWebElement _validationContainer
        {
            get
            {
                return WaitForAndGet(By.CssSelector("[data-automation-id='status_error']"));
            }
        }
        
        private IWebElement _successContainer
        {
            get
            {
                return WaitForAndGet(By.CssSelector("[data-automation-id='status_success']"));
            }
        }

        private IWebElement SearchButton
        {
            get
            {
                return _search.FindElementSafe(By.CssSelector("[data-automation-id='search_criteria_submit']"));
            }
        }

        private IWebElement AcademicYearDropDownSelector
        {
            get
            {
                return _search.FindElementSafe(By.Name("AcademicSelector.dropdownImitator"));
            }
        }


        private IWebElement AcademicYearDropDown
        {
            get
            {
                return _search.FindElementSafe(By.Name("AcademicSelector.Binding"));
            }
        }

        private IList<IWebElement> AcademicYearItems
        {
            get
            {
                return WebContext.WebDriver.FindElements(By.Id("select2-results-1"));
                //  return _main.FindElementsSafe(By.ClassName("select2-result-label"));                 
                // return _main.FindElementsSafe(By.Id("select2-results"));
            }
        }

        private IWebElement Targetvalue
        {
            get
            {
                return _main.FindElementSafe(By.Name("Threshold.TargetValue"));  // TargetValue //Threshold.TargetValue
            }
        }

        private IWebElement Benchmark
        {
            get
            {
                return _main.FindElementSafe(By.Name("Benchmark.TargetValue"));
            }
        }

        private IWebElement Thresholdminimum
        {
            get
            {
                return _main.FindElement(By.Name("Threshold.MinThreshold"));
            }
        }
        private IWebElement Thresholdmaximum
        {
            get
            {
                return _main.FindElementSafe(By.Name("Threshold.MaxThreshold"));
            }
        }

        public AttendanceKPIPages()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        //***************************************************Operation for Modify Attendance Key Performance Indicator***************************************************

        public void ModifyTargetValue(string targetvalue)
        {
            Targetvalue.Clear();
            Targetvalue.SendKeys(targetvalue);
        }

        public void ModifyBenchMark(string benchmark)
        {
            Benchmark.Clear();
            Benchmark.SendKeys(benchmark);
        }
        public void ModifyThresholdMinimum(string thresholdminimum)
        {
            Thresholdminimum.Clear();
            Thresholdminimum.SendKeys(thresholdminimum);
        }

        public void ModifyThresholdMaximum(string thresholdmaximum)
        {
            Thresholdmaximum.Clear();
            Thresholdmaximum.SendKeys(thresholdmaximum);
        }
        public void ClearTargetValue()
        {
            Targetvalue.Clear();
        }
        public void ClearBenchMark()
        {
            Benchmark.Clear();
        }

        public void SelectSearchDropdown()
        {
            AcademicYearDropDownSelector.Click();
        }

        public void SelectSearchItem()
        {
            AcademicYearItems[0].Click();
        }


        public string[] GetValidationWarning()
        {
            return _validationContainer.Text.Split(new[] { "\r\n" }, StringSplitOptions.None);            
        }

        public string[] GetValidationWarningByCssSelector()
        {
            return _validationContainerCssSelector.Text.Split(new[] { "\r\n" }, StringSplitOptions.None);            
        }

        public bool HasConfirmedSave()
        {
            return _successContainer.Displayed;            
        }

        public void Search()
        {
            SearchButton.Click();
        }

        public bool IsCurrentAcademicYearSelected()
        {
            var mySelect = new SelectElement(AcademicYearDropDown);
            var selectedOption = mySelect.SelectedOption;
            var optionText = selectedOption.GetAttribute("innerText");
            var currentAcademicYear = "Academic Year 2016/2017";   //ToDo : Retrieve current Academic year from DB 
            return optionText.CompareTo(currentAcademicYear) == 0;
        }

        public static void Navigate()
        {
            //Login to SIMS8 Application
            SeleniumHelper.Login();
            // Navigate to Key Performance Indicators Menu
            SeleniumHelper.NavigateMenu("Tasks", "School Management", "Key Performance Indicators");
        }
    }
}