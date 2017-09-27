using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedComponents;
using SharedComponents.BaseFolder;
using OpenQA.Selenium.Support.PageObjects;
using WebDriverRunner.webdriver;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeSugar;
using PageObjectModel.Helper;
using System.Threading;

namespace Admissions.Component
{
    public class CriteriaDetails : BaseSeleniumComponents
    {    
    

        public CriteriaDetails()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1200));

        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(1200));

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='manage_criteria_header_title']")]
        public IWebElement CriteriaTitle;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='well_know_action_save']")]
        public IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        public IWebElement DeleteButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='confirm_delete_dialog'] button[data-automation-id='continue_with_delete_button']")]
        public IWebElement DeleteContinueButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_criteria_button']")]
        public IWebElement AddCriteriaButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        public IWebElement SearchButton;
        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='search_result']")]
        public IWebElement SearchResult;

        public static By CriteriaName = By.CssSelector("form[id='editableData'] input[name='Name']");
         public static By CriteriaDescription = By.CssSelector("textarea[name='Description']");
       public static By CriteriaSearchByName = By.CssSelector("form[data-automation-id='search_criteria'] input[name='Name']");

        public const string StatusSuccess = "status_success";

        public String getCriteriaTitle()
        {

            waiter.Until(ExpectedConditions.ElementExists(By.CssSelector("span[data-automation-id='manage_criteria_header_title']")));
            String CriteriaTitle = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id='manage_criteria_header_title']")).Text;
            return CriteriaTitle;
        }

        public String setCriteriaName()
        {

            waiter.Until(ExpectedConditions.ElementExists(CriteriaName));
            String CriteriaNameText = Utilities.GenerateRandomString(10, "Criteria");
            WebContext.WebDriver.FindElement(CriteriaName).SendKeys(CriteriaNameText);
            return CriteriaNameText;
        }

        public String getCriteriaName()
        {

            String criteriaName = waiter.Until(ExpectedConditions.ElementExists(CriteriaName)).GetText();
            return criteriaName;
        }

        public String getCriteriaDescription()
        {

            String criteriaDescription = waiter.Until(ExpectedConditions.ElementExists(CriteriaDescription)).GetText();
            return criteriaDescription;
        }

        public String setCriteriaDescription()
        {

            waiter.Until(ExpectedConditions.ElementExists(CriteriaName));
            String criteriaDescription = Utilities.GenerateRandomString(20, "Criteria Description ");
            WebContext.WebDriver.FindElement(CriteriaDescription).SendKeys(criteriaDescription);
            return criteriaDescription;
        }

        public void ClickAddCriteriaButton()
        {
            Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddCriteriaButton)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton));
        }

        public void ClickSaveButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton)).Click();
        }

        public void ClickDeleteButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteButton)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteContinueButton)).Click();

        }


        public void WaitForStatus()
        {
            Thread.Sleep(2000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.CssSelector(SeleniumHelper.AutomationId(StatusSuccess)));
        }

        public CriteriaDetails ClickSearchButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
            SearchButton.Click();
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            WaitUntillAjaxRequestCompleted();
            return new CriteriaDetails();
        }


        public CriteriaDetails SearchByName(String Surname)
        {
            WebContext.WebDriver.FindElement(CriteriaSearchByName).SendKeys(Surname);
            //ClickSearchButton();
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchResult));
            SearchResult.Click();
            WaitUntillAjaxRequestCompleted();
            SearchResult.Text.Contains(Surname);
            return new CriteriaDetails();
        }

    }
}
