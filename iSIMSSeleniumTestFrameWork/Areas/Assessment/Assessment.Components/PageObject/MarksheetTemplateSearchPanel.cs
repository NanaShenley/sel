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
    public class MarksheetTemplateSearchPanel : BaseSeleniumComponents
    {

        public MarksheetTemplateSearchPanel()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        //SMP = Search Marksheet Panel

        [FindsBy(How = How.CssSelector, Using = "span[title='Marksheet Template Name']")]
        private IWebElement MarksheetTemplatePanelHeader;

        [FindsBy(How = How.CssSelector, Using = "button[data-toggle='show-left-panel']")]
        private IWebElement SearchMarksheetPanelButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='MarksheetName']")]
        private IWebElement SMPMarksheetName;

        [FindsBy(How = How.LinkText, Using = "Year Group")]
        private IWebElement YearGroupLink;

        [FindsBy(How = How.LinkText, Using = "Class")]
        private IWebElement ClassLink;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_advanced']")]
        private IWebElement SMPShowMoreLink;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='search_result_de87e34f_fddd_4f45_90a4_b060f6eff9de']")]
        private IWebElement MTDetailsLoading;

        [FindsBy(How = How.CssSelector, Using = "form[data-automation-id='search_criteria'] button[data-automation-id='search_criteria_submit']")]
        private IWebElement SMPSearchButton;

        [FindsBy(How = How.Id, Using = "tri_chkbox_Active")]
        private IWebElement SMPActiveCheckBox;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter'] > strong")]
        private IWebElement SMPTemplateResultCount;

        [FindsBy(How = How.Name, Using = "Owners.dropdownImitator")]
        private IWebElement OwnerDropdownInitiator;

        [FindsBy(How = How.Name, Using = "Subjects.dropdownImitator")]
        private IWebElement SubjectDropdownInitiator;        

        //Search Panel Filter Elements List

        private static By YearGroups = By.CssSelector("input[name='YearGroups.SelectedIds']");
        private static By Classes = By.CssSelector("input[name='Classes.SelectedIds']");
        private static By OwnerResultsList = By.CssSelector("div.select2-result-label");
        private static By MarksheetTemplateSearchResults = By.CssSelector("div[data-automation-id='search_results'] a[class='search-result h1-result']");

        //Hydration Logic web Elements

        [FindsBy(How = How.CssSelector, Using = "li[data-automation-id='quicklinks_top_level_pupil_submenu_pupilrecords']")]
        private IWebElement PupilRecordButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_pupil_button']")]
        private IWebElement AddNewPupilButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='task_menu']")]
        private IWebElement TaskMenu;

        [FindsBy(How = How.CssSelector, Using = "a[data-ajax-url*='CreateMarksheet/Details']")]
        private IWebElement ManageTemplatesLink;

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));


        /// <summary>
        /// Returns the result count for the search parameter and returns "No Marksheet Templates Found" in case of No Results are found for the search parameter.
        /// </summary>
        public string GetMarksheetTemplateCount()
        {
            try
            {
                return SMPTemplateResultCount.Text;
            }
            catch
            {
                return "No Marksheet Templates Found";
            }
        }

        /// <summary>
        /// Returns all the Marksheet Template Results as a part of List and returns "No Marksheet Templates Found" in case of No Result found.
        /// </summary>
        public List<string> TemplateResult()
        {
            List<string> MarksheetTemplateNameResults = new List<string>();
            ReadOnlyCollection<IWebElement> TemplateSearchResultList = WebContext.WebDriver.FindElements(MarksheetTemplateSearchResults);
            if (TemplateSearchResultList.Count == 0)
            {
                MarksheetTemplateNameResults.Add("No Marksheet Templates Found");
                return MarksheetTemplateNameResults;
            }

            else
            {
                foreach (IWebElement eachelement in TemplateSearchResultList)
                {
                    MarksheetTemplateNameResults.Add(eachelement.Text);
                }
                return MarksheetTemplateNameResults;
            }
        }

        /// <summary>
        /// Selects a marksheet template based on the specified name
        /// </summary>
        public MarksheetTemplateDetails SelectMarksheetTemplate(string MarksheetTemplateName)
        {
            ReadOnlyCollection<IWebElement> TemplateSearchResultList = WebContext.WebDriver.FindElements(MarksheetTemplateSearchResults);
            foreach (IWebElement eachelement in TemplateSearchResultList)
            {
                if (eachelement.Text == MarksheetTemplateName)
                {
                    eachelement.Click();
                    Thread.Sleep(2000);
                    break;
                }
            }
            return new MarksheetTemplateDetails();
        }


        /// <summary>
        /// Opens up the Search Marksheet Panel and waits until Marksheet Template Header Panel Name is displayed
        /// </summary>
        public MarksheetTemplateSearchPanel OpenSearchMarksheetPanel()
        {

            SearchMarksheetPanelButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(MarksheetTemplatePanelHeader, "Marksheet Template Name"));
            return new MarksheetTemplateSearchPanel();
        }

        /// <summary>
        /// Expands the Year Groups list options
        /// </summary>
        public void ExpandYearGroupList()
        {
            YearGroupLink.Click();
        }

        /// <summary>
        /// Expands the Classes list options
        /// </summary>
        public void ExpandClassList()
        {
            ClassLink.Click();
        }

        /// <summary>
        /// Checks the Active Checkbox in case true or unchecks it in case false
        /// </summary>
        public void IsActive(bool CheckBoxState)
        {
            try
            {
                if (CheckBoxState == true)
                {
                    if (SMPActiveCheckBox.GetAttribute("checked") == "checked")
                        return;
                }
                else if (CheckBoxState == false)
                {
                    if (SMPActiveCheckBox.GetAttribute("checked") == "checked")
                        SMPActiveCheckBox.Click();
                }
            }
            catch (Exception)
            {
                if (CheckBoxState == true)
                    SMPActiveCheckBox.Click();
                else if (CheckBoxState == false)
                    return;
            }
        }

        /// <summary>
        /// Selects a particular Year Group (Returns True in case the Year Group Exists or else will return false.
        /// </summary>
        public bool SelectYearGroup(string YearGroup)
        {
            ReadOnlyCollection<IWebElement> YearGroupList = WebContext.WebDriver.FindElements(YearGroups);
            int counter = 0;
            foreach (IWebElement eachelement in YearGroupList)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == YearGroup)
                {
                    eachelement.Click();
                    counter++;
                    break;
                }
            }
            if (counter == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Opens up the Advance Search Options
        /// </summary>
        public void OpenAdvanceSearchOptions()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SMPShowMoreLink));
            SMPShowMoreLink.Click();
        }

        /// <summary>
        /// Searches the Marksheet Template based on the available search criteria
        /// </summary>
        public MarksheetTemplateSearchPanel ClickOnSearch()
        {
            SMPSearchButton.Click();
            // This method allows user to wait until the results are getting displayed after click of serach button
            while (true)
            {
                if (SMPSearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new MarksheetTemplateSearchPanel();
        }


        /// <summary>
        /// Searches and Selects a particular class (Returns True in case the class Exists or else will return false.
        /// </summary>
        public bool SelectClass(string Class)
        {
            ReadOnlyCollection<IWebElement> ClassesList = WebContext.WebDriver.FindElements(Classes);
            int counter = 0;
            foreach (IWebElement eachelement in ClassesList)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == Class)
                {
                    Actions actions = new Actions(WebContext.WebDriver);
                    actions.MoveToElement(eachelement).Click().Build().Perform();
                    counter++;
                    break;
                }
            }
            if (counter == 0)
                return false;
            return true;
        }


        /// <summary>
        /// Searches Marksheet by its Owner
        /// </summary>
        public bool SelectOwner(string Owner)
        {
            OwnerDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(OwnerResultsList);
           
            int counter = 0;
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == Owner)
                {
                    eachelement.Click();
                    counter++;
                    break;
                }
            }

            if (counter == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Check whether the dropdown contains the passed String
        /// </summary>
        public String VerifyIteminDropdown(string Owner)
        {
            OwnerDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(OwnerResultsList);

           
            String templateOwner = "";
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == Owner)
                {
                    templateOwner = eachelement.Text;
                    eachelement.Click();
                   break;
                }
            }


            return templateOwner;
        }

        /// <summary>
        /// Searches Marksheet by its Subject
        /// </summary>
        public bool SelectSubject(string Subject)
        {
            SubjectDropdownInitiator.Click();
            ReadOnlyCollection<IWebElement> List = WebContext.WebDriver.FindElements(OwnerResultsList);
            int counter = 0;
            foreach (IWebElement eachelement in List)
            {
                if (eachelement.Text == Subject)
                {
                    eachelement.Click();
                    counter++;
                    break;
                }
            }

            if (counter == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Enter Marksheet Template Name in the Marksheet Template Name Search Text Box
        /// </summary>
        public void EnterMarksheetTemplateName(string Name)
        {
            SMPMarksheetName.Clear();
            SMPMarksheetName.SendKeys(Name);
        }

        /// <summary>
        /// Hydration Logic - Method to Navigate to Pupil Record Screen and then Opens back the Marksheet Template Search Panel 
        /// </summary>
        public MarksheetTemplateSearchPanel HydrationLogic()
        {
            WebContext.WebDriver.FindElement(By.CssSelector("button[title='Hide Search']")).Click();
            PupilRecordButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(AddNewPupilButton, "Add New Pupil"));
            TaskMenu.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(ManageTemplatesLink)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchMarksheetPanelButton)).Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(MarksheetTemplatePanelHeader, "Marksheet Template Name"));
            return new MarksheetTemplateSearchPanel();
        }
    }
}
