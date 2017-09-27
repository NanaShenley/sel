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

namespace Assessment.Components.PageObject
{
    public class GroupFilters
    {
        public GroupFilters()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] div.slider-header-title")]
        private IWebElement GroupsHeader;

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
        private IWebElement GroupFilterYearGroupHeader;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-group']")]
        private IWebElement DoneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='group-filter'] [data-marksheet-groupfilter-back]")]
        private IWebElement BackButton;

        //[FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='group-filter'] [data-automation-id='groupfilterclosebutton']")]
        //private IWebElement CloseButton;

        //Expand/Collapse Control Element
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Ethnicity']")]
        private IWebElement EthnicityCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Curriculum Year']")]
        private IWebElement NCYearCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Language']")]
        private IWebElement LanguageCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_New_Intake Group']")]
        private IWebElement SchoolIntakeCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='marksheets-groupfilter-searchResults'] a[data-automation-id='section_menu_Class']")]
        private IWebElement ClassCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='marksheets-groupfilter-searchResults'] a[data-automation-id='section_menu_Year Group']")]
        private IWebElement YearGroupsCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_SEN Need Type']")]
        private IWebElement SenNeedTypeCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_User Defined Group']")]
        private IWebElement UserDefinedGroupCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_Teaching Group']")]
        private IWebElement TeachingGroupCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='section_menu_SEN Status']")]
        private IWebElement SENStatusCollapseButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='done-group'] div.builder-step-title")]
        private IWebElement GroupFilterDoneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='home'] div.slider-header-title")]
        private IWebElement MarksheetBuilderHeader;

        private static By SelectNCYear = By.CssSelector("input[name='NCYear.SelectedIds']");
        private static By SelectEthnicity = By.CssSelector("input[name='Ethnicity.SelectedIds']");
        private static By SelectLanguage = By.CssSelector("input[name='Language.SelectedIds']");
        private static By SelectSchoolIntake = By.CssSelector("input[name='SchoolIntake.SelectedIds']");
        private static By SelectClassesFilter = By.CssSelector("input[name='ClassesFilter.SelectedIds']");
        private static By SelectYearGroupsFilter = By.CssSelector("input[name='YearGroupsFilter.SelectedIds']");
        private static By SelectSenNeedType = By.CssSelector("input[name='SenNeedType.SelectedIds']");
        private static By SelectUserDefinedGroup = By.CssSelector("input[name='UserDefined.SelectedIds']");
        private static By SelectTeachingGroup = By.CssSelector("input[name='TeachingGroup.SelectedIds']");
        private static By SelectSENStatus = By.CssSelector("input[name='SenStatus.SelectedIds']");
        //private static By AllFilterValues = By.CssSelector("div[data-automation-id='marksheets-groupfilter-searchResultsinput'] [name*='.SelectedIds']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        /// <summary>
        /// Clicks the Done Button and moves on to the Marksheet Builder page
        /// </summary>
        public MarksheetBuilder ClickDoneButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DoneButton));
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupFilterHeader, "Additional Filter"));
            Thread.Sleep(2000);
            DoneButton.Click();
            while (true)
            {
                if (DoneButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new MarksheetBuilder();
        }

        /// <summary>
        /// Clicks the Back Button and moves on to the Marksheet Builder page
        /// </summary>
        public MarksheetBuilder ClickBackButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(BackButton));
            BackButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(MarksheetBuilderHeader, "Marksheet Builder"));
            return new MarksheetBuilder();
        }

        ///// <summary>
        ///// Clicks the Close Button and moves on to the Marksheet Builder page
        ///// </summary>
        //public MarksheetBuilder ClickCloseButton()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(CloseButton));
        //    CloseButton.Click();
        //    return new MarksheetBuilder();
        //}

        /// <summary>
        /// Returns a New Page Object for the Group Filters Page
        /// </summary>
        public GroupFilters NewGroupFiltersPageObject()
        {
            return new GroupFilters();
        }


        //NC YEAR GROUP FILTER FUNCTIONS


        /// <summary>
        ///  Counts the No. of available NC Year options
        /// </summary>
        public int GetNCYearFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> NCYearList = WebContext.WebDriver.FindElements(SelectNCYear);
                if (NCYearList.Count > 0)
                {
                    foreach (IWebElement eachelement in NCYearList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of NC Year Filter Options
        /// </summary>
        public List<string> GetNCYearFilterNameList()
        {
            List<string> NCYearFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> NCYearList = WebContext.WebDriver.FindElements(SelectNCYear);
                foreach (IWebElement eachelement in NCYearList)
                {
                    NCYearFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return NCYearFilterOptionsNameList;
            }
            catch
            {
                return NCYearFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for NC Year Filter Options
        /// </summary>
        public string GetSelctedNCYearFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> NCYearList = WebContext.WebDriver.FindElements(SelectNCYear);
                string text = NCYearCollapseButton.GetAttribute("aria-expanded");
                Console.WriteLine(text);
                if (NCYearCollapseButton.GetAttribute("aria-expanded") != "true")
                    NCYearCollapseButton.Click();
                while (true)
                {
                    if (NCYearCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (NCYearList.Count > 0)
                {
                    foreach (IWebElement eachelement in NCYearList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a NC Year Filter Option
        /// </summary>
        public void SelectNCYearFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> NCYearList = WebContext.WebDriver.FindElements(SelectNCYear);
                if (NCYearCollapseButton.GetAttribute("aria-expanded") != "true")
                    NCYearCollapseButton.Click();
                while (true)
                {
                    if (NCYearCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (NCYearList.Count > 0)
                {
                    foreach (IWebElement eachelement in NCYearList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available", FilterName);
            }
        }



        //ETHNICITY GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available Ethnicity options
        /// </summary>
        public int GetEthnicityFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> EthnicityList = WebContext.WebDriver.FindElements(SelectEthnicity);
                if (EthnicityList.Count > 0)
                {
                    foreach (IWebElement eachelement in EthnicityList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of Ethnicity Filter Options
        /// </summary>
        public List<string> GetEthnicityFilterNameList()
        {
            List<string> EthnicityFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> EthnicityList = WebContext.WebDriver.FindElements(SelectEthnicity);
                foreach (IWebElement eachelement in EthnicityList)
                {
                    EthnicityFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return EthnicityFilterOptionsNameList;
            }
            catch
            {
                return EthnicityFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for Ethnicity Filter Options
        /// </summary>
        public string GetSelctedEthnicityFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> EthnicityList = WebContext.WebDriver.FindElements(SelectEthnicity);
                if (EthnicityCollapseButton.GetAttribute("aria-expanded") != "true")
                    EthnicityCollapseButton.Click();
                while (true)
                {
                    if (EthnicityCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (EthnicityList.Count > 0)
                {
                    foreach (IWebElement eachelement in EthnicityList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a Ethnicity Filter Option
        /// </summary>
        public void SelectEthnicityFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> EthnicityList = WebContext.WebDriver.FindElements(SelectEthnicity);
                if (EthnicityCollapseButton.GetAttribute("aria-expanded") != "true")
                    EthnicityCollapseButton.Click();
                while (true)
                {
                    if (EthnicityCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (EthnicityList.Count > 0)
                {
                    foreach (IWebElement eachelement in EthnicityList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available", FilterName);
            }
        }



        //LANGUAGE GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available Language options
        /// </summary>
        public int GetLanguageFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> LanguageList = WebContext.WebDriver.FindElements(SelectLanguage);
                if (LanguageList.Count > 0)
                {
                    foreach (IWebElement eachelement in LanguageList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of Language Filter Options
        /// </summary>
        public List<string> GetLanguageFilterNameList()
        {
            List<string> LanguageFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> LanguageList = WebContext.WebDriver.FindElements(SelectLanguage);
                foreach (IWebElement eachelement in LanguageList)
                {
                    LanguageFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return LanguageFilterOptionsNameList;
            }
            catch
            {
                return LanguageFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for Language Filter Options
        /// </summary>
        public string GetSelctedLanguageFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> LanguageList = WebContext.WebDriver.FindElements(SelectLanguage);
                if (LanguageCollapseButton.GetAttribute("aria-expanded") != "true")
                    LanguageCollapseButton.Click();
                while (true)
                {
                    if (LanguageCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (LanguageList.Count > 0)
                {
                    foreach (IWebElement eachelement in LanguageList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a Language Filter Option
        /// </summary>
        public void SelectLanguageFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> LanguageList = WebContext.WebDriver.FindElements(SelectLanguage);
                if (LanguageCollapseButton.GetAttribute("aria-expanded") != "true")
                    LanguageCollapseButton.Click();
                while (true)
                {
                    if (LanguageCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (LanguageList.Count > 0)
                {
                    foreach (IWebElement eachelement in LanguageList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }



        //SCHOOL INTAKE GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available School Intake options
        /// </summary>
        public int GetSchoolIntakeFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> SchoolIntakeList = WebContext.WebDriver.FindElements(SelectSchoolIntake);
                if (SchoolIntakeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SchoolIntakeList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of School Intake Filter Options
        /// </summary>
        public List<string> GetSchoolIntakeFilterNameList()
        {
            List<string> SchoolIntakeFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> SchoolIntakeList = WebContext.WebDriver.FindElements(SelectSchoolIntake);
                foreach (IWebElement eachelement in SchoolIntakeList)
                {
                    SchoolIntakeFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return SchoolIntakeFilterOptionsNameList;
            }
            catch
            {
                return SchoolIntakeFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for School Intake Filter Options
        /// </summary>
        public string GetSelctedSchoolIntakeFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> SchoolIntakeList = WebContext.WebDriver.FindElements(SelectSchoolIntake);
                if (SchoolIntakeCollapseButton.GetAttribute("aria-expanded") != "true")
                    SchoolIntakeCollapseButton.Click();
                while (true)
                {
                    if (SchoolIntakeCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SchoolIntakeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SchoolIntakeList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a School Intake Filter Option
        /// </summary>
        public void SelectSchoolIntakeFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> SchoolIntakeList = WebContext.WebDriver.FindElements(SelectSchoolIntake);
                if (SchoolIntakeCollapseButton.GetAttribute("aria-expanded") != "true")
                    SchoolIntakeCollapseButton.Click();
                while (true)
                {
                    if (SchoolIntakeCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SchoolIntakeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SchoolIntakeList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }



        //CLASS GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available Class options
        /// </summary>
        public int GetClassFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> ClassList = WebContext.WebDriver.FindElements(SelectClassesFilter);
                if (ClassList.Count > 0)
                {
                    foreach (IWebElement eachelement in ClassList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of Class Filter Options
        /// </summary>
        public List<string> GetClassFilterNameList()
        {
            List<string> ClassFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> ClassList = WebContext.WebDriver.FindElements(SelectClassesFilter);
                foreach (IWebElement eachelement in ClassList)
                {
                    ClassFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return ClassFilterOptionsNameList;
            }
            catch
            {
                return ClassFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for Class Filter Options
        /// </summary>
        public string GetSelctedClassFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> ClassList = WebContext.WebDriver.FindElements(SelectClassesFilter);
                if (ClassCollapseButton.GetAttribute("aria-expanded") != "true")
                    ClassCollapseButton.Click();
                while (true)
                {
                    if (ClassCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (ClassList.Count > 0)
                {
                    foreach (IWebElement eachelement in ClassList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a Class Filter Option
        /// </summary>
        public void SelectClassFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> ClassList = WebContext.WebDriver.FindElements(SelectClassesFilter);
                if (ClassCollapseButton.GetAttribute("aria-expanded") != "true")
                    ClassCollapseButton.Click();
                while (true)
                {
                    if (ClassCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (ClassList.Count > 0)
                {
                    foreach (IWebElement eachelement in ClassList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("{0} is not available");
            }
        }



        //YEAR GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available Year Groups options
        /// </summary>
        public int GetYearGroupsFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> YearGroupsList = WebContext.WebDriver.FindElements(SelectYearGroupsFilter);
                if (YearGroupsList.Count > 0)
                {
                    foreach (IWebElement eachelement in YearGroupsList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of Year Groups Filter Options
        /// </summary>
        public List<string> GetYearGroupsFilterNameList()
        {
            List<string> YearGroupsFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> YearGroupsList = WebContext.WebDriver.FindElements(SelectYearGroupsFilter);
                foreach (IWebElement eachelement in YearGroupsList)
                {
                    YearGroupsFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return YearGroupsFilterOptionsNameList;
            }
            catch
            {
                return YearGroupsFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for Year Groups Filter Options
        /// </summary>
        public string GetSelctedYearGroupsFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> YearGroupsList = WebContext.WebDriver.FindElements(SelectYearGroupsFilter);
                if (YearGroupsCollapseButton.GetAttribute("aria-expanded") != "true")
                    YearGroupsCollapseButton.Click();
                while (true)
                {
                    if (YearGroupsCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (YearGroupsList.Count > 0)
                {
                    foreach (IWebElement eachelement in YearGroupsList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a Year Groups Filter Option
        /// </summary>
        public void SelectYearGroupsFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> YearGroupsList = WebContext.WebDriver.FindElements(SelectYearGroupsFilter);
                if (YearGroupsCollapseButton.GetAttribute("aria-expanded") != "true")
                    YearGroupsCollapseButton.Click();
                while (true)
                {
                    if (YearGroupsCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (YearGroupsList.Count > 0)
                {
                    foreach (IWebElement eachelement in YearGroupsList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }



        //SEN NEED TYPE GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available SenNeedType options
        /// </summary>
        public int GetSenNeedTypeFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> SenNeedTypeList = WebContext.WebDriver.FindElements(SelectSenNeedType);
                if (SenNeedTypeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SenNeedTypeList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of SenNeedType Filter Options
        /// </summary>
        public List<string> GetSenNeedTypeFilterNameList()
        {
            List<string> SenNeedTypeFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> SenNeedTypeList = WebContext.WebDriver.FindElements(SelectSenNeedType);
                foreach (IWebElement eachelement in SenNeedTypeList)
                {
                    SenNeedTypeFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return SenNeedTypeFilterOptionsNameList;
            }
            catch
            {
                return SenNeedTypeFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for SenNeedType Filter Options
        /// </summary>
        public string GetSelctedSenNeedTypeFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> SenNeedTypeList = WebContext.WebDriver.FindElements(SelectSenNeedType);
                if (SenNeedTypeCollapseButton.GetAttribute("aria-expanded") != "true")
                    SenNeedTypeCollapseButton.Click();
                while (true)
                {
                    if (SenNeedTypeCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SenNeedTypeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SenNeedTypeList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a SenNeedType Filter Option
        /// </summary>
        public void SelectSenNeedTypeFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> SenNeedTypeList = WebContext.WebDriver.FindElements(SelectSenNeedType);
                if (SenNeedTypeCollapseButton.GetAttribute("aria-expanded") != "true")
                    SenNeedTypeCollapseButton.Click();
                while (true)
                {
                    if (SenNeedTypeCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SenNeedTypeList.Count > 0)
                {
                    foreach (IWebElement eachelement in SenNeedTypeList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }



        //USER DEFINED GROUP FILTER FUNCTIONS



        /// <summary>
        ///  Counts the No. of available UserDefinedGroup options
        /// </summary>
        public int GetUserDefinedGroupFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> UserDefinedGroupList = WebContext.WebDriver.FindElements(SelectUserDefinedGroup);
                if (UserDefinedGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in UserDefinedGroupList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of UserDefinedGroup Filter Options
        /// </summary>
        public List<string> GetUserDefinedGroupFilterNameList()
        {
            List<string> UserDefinedGroupFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> UserDefinedGroupList = WebContext.WebDriver.FindElements(SelectUserDefinedGroup);
                foreach (IWebElement eachelement in UserDefinedGroupList)
                {
                    UserDefinedGroupFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return UserDefinedGroupFilterOptionsNameList;
            }
            catch
            {
                return UserDefinedGroupFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for UserDefinedGroup Filter Options
        /// </summary>
        public string GetSelctedUserDefinedGroupFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> UserDefinedGroupList = WebContext.WebDriver.FindElements(SelectUserDefinedGroup);
                if (UserDefinedGroupCollapseButton.GetAttribute("aria-expanded") != "true")
                    UserDefinedGroupCollapseButton.Click();
                while (true)
                {
                    if (UserDefinedGroupCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (UserDefinedGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in UserDefinedGroupList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a UserDefinedGroup Filter Option
        /// </summary>
        public void SelectUserDefinedGroupFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> UserDefinedGroupList = WebContext.WebDriver.FindElements(SelectUserDefinedGroup);
                if (UserDefinedGroupCollapseButton.GetAttribute("aria-expanded") != "true")
                    UserDefinedGroupCollapseButton.Click();
                while (true)
                {
                    if (UserDefinedGroupCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (UserDefinedGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in UserDefinedGroupList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }







        /// <summary>
        ///  Counts the No. of available TeachingGroup options
        /// </summary>
        public int GetTeachingGroupFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> TeachingGroupList = WebContext.WebDriver.FindElements(SelectTeachingGroup);
                if (TeachingGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in TeachingGroupList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of TeachingGroup Filter Options
        /// </summary>
        public List<string> GetTeachingGroupFilterNameList()
        {
            List<string> TeachingGroupFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> TeachingGroupList = WebContext.WebDriver.FindElements(SelectTeachingGroup);
                foreach (IWebElement eachelement in TeachingGroupList)
                {
                    TeachingGroupFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return TeachingGroupFilterOptionsNameList;
            }
            catch
            {
                return TeachingGroupFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for TeachingGroup Filter Options
        /// </summary>
        public string GetSelctedTeachingGroupFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> TeachingGroupList = WebContext.WebDriver.FindElements(SelectTeachingGroup);
                if (TeachingGroupCollapseButton.GetAttribute("aria-expanded") != "true")
                    TeachingGroupCollapseButton.Click();
                while (true)
                {
                    if (TeachingGroupCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (TeachingGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in TeachingGroupList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a TeachingGroup Filter Option
        /// </summary>
        public void SelectTeachingGroupFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> TeachingGroupList = WebContext.WebDriver.FindElements(SelectTeachingGroup);
                if (TeachingGroupCollapseButton.GetAttribute("aria-expanded") != "true")
                    TeachingGroupCollapseButton.Click();
                while (true)
                {
                    if (TeachingGroupCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (TeachingGroupList.Count > 0)
                {
                    foreach (IWebElement eachelement in TeachingGroupList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }







        /// <summary>
        ///  Counts the No. of available SENStatus options
        /// </summary>
        public int GetSENStatusFilterOptionsCount()
        {
            int i = 0;
            try
            {
                ReadOnlyCollection<IWebElement> SENStatusList = WebContext.WebDriver.FindElements(SelectSENStatus);
                if (SENStatusList.Count > 0)
                {
                    foreach (IWebElement eachelement in SENStatusList)
                    {
                        i++;
                    }
                }
                return i;
            }
            catch
            {
                return i;
            }
        }

        /// <summary>
        ///  Gets the list of SENStatus Filter Options
        /// </summary>
        public List<string> GetSENStatusFilterNameList()
        {
            List<string> SENStatusFilterOptionsNameList = new List<string>();
            try
            {
                IReadOnlyCollection<IWebElement> SENStatusList = WebContext.WebDriver.FindElements(SelectSENStatus);
                foreach (IWebElement eachelement in SENStatusList)
                {
                    SENStatusFilterOptionsNameList.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text);
                }
                return SENStatusFilterOptionsNameList;
            }
            catch
            {
                return SENStatusFilterOptionsNameList;
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name for SENStatus Filter Options
        /// </summary>
        public string GetSelctedSENStatusFilterName()
        {
            string selectedfilter = "";
            try
            {
                ReadOnlyCollection<IWebElement> SENStatusList = WebContext.WebDriver.FindElements(SelectSENStatus);
                if (SENStatusCollapseButton.GetAttribute("aria-expanded") == "false" || SENStatusCollapseButton.GetAttribute("aria-expanded") != null)
                    SENStatusCollapseButton.Click();
                while (true)
                {
                    if (SENStatusCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SENStatusList.Count > 0)
                {
                    foreach (IWebElement eachelement in SENStatusList)
                    {
                        if (eachelement.Selected)
                        {
                            selectedfilter = WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text;
                        }
                    }
                }
                return selectedfilter;
            }
            catch
            {
                return selectedfilter;
            }
        }

        /// <summary>
        ///  Selects a SENStatus Filter Option
        /// </summary>
        public void SelectSENStatusFilterName(string FilterName)
        {
            try
            {
                ReadOnlyCollection<IWebElement> SENStatusList = WebContext.WebDriver.FindElements(SelectSENStatus);
                if (SENStatusCollapseButton.GetAttribute("aria-expanded") == "false" || SENStatusCollapseButton.GetAttribute("aria-expanded") != null)
                    SENStatusCollapseButton.Click();
                while (true)
                {
                    if (SENStatusCollapseButton.GetAttribute("aria-expanded") == "true")
                        break;
                }
                if (SENStatusList.Count > 0)
                {
                    foreach (IWebElement eachelement in SENStatusList)
                    {
                        if (!eachelement.Selected && WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == FilterName)
                        {
                            WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Click();
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("{0} is not available");
            }
        }

        /// <summary>
        ///  Gets the selected Group Filter Name from all the filter options
        /// </summary>
        public string GetSelctedFilterName()
        {
            
            List<string> FunctionNames = new List<string>();
            FunctionNames.Add(GetSelctedNCYearFilterName());
            FunctionNames.Add(GetSelctedEthnicityFilterName());
            FunctionNames.Add(GetSelctedLanguageFilterName());
            FunctionNames.Add(GetSelctedSchoolIntakeFilterName());
            FunctionNames.Add(GetSelctedYearGroupsFilterName());
            FunctionNames.Add(GetSelctedClassFilterName());
            FunctionNames.Add(GetSelctedSenNeedTypeFilterName());
            FunctionNames.Add(GetSelctedUserDefinedGroupFilterName());
            FunctionNames.Add(GetSelctedTeachingGroupFilterName());
            FunctionNames.Add(GetSelctedSENStatusFilterName());
            string selectedfilter = "";
            foreach (string eachstring in FunctionNames)
            {
                if (eachstring != "")
                {
                    selectedfilter = eachstring;
                    break;
                }
            }
            return selectedfilter;
        }

        /// <summary>
        ///  Gets the Text Present on the Group Filter Done Button
        /// </summary>
        public string GetGroupFilterDoneButtonText()
        {
            string GroupFilterDoneButtonText = "";
            GroupFilterDoneButtonText = GroupFilterDoneButton.Text;
            return GroupFilterDoneButtonText;
        }
    }
}
