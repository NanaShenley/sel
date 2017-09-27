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
    public class PolicyDetails : BaseSeleniumComponents
    {


        public PolicyDetails()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1200));

        }

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(1200));

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='admission_policies_header_title']")]
        public IWebElement PolicyTitle;
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_admission_policy_button']")]
        public IWebElement AddPolicyButton;

        //[FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_criteria_button']")]
        //public IWebElement AddCriteriaButton;

        [FindsBy(How = How.CssSelector, Using = "div[id='screen-viewer'] a[data-automation-id='add_new_criteria_button']")]
        public IWebElement AddNewCriteriaButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='well_know_action_save']")]
        public IWebElement SaveButton;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='delete_button']")]
        public IWebElement DeleteButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='confirm_delete_dialog'] button[data-automation-id='continue_with_delete_button']")]
        public IWebElement DeleteContinueButton;


        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        public IWebElement SearchButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='search_result']")]
        public IWebElement SearchResult;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='confirmation_required_dialog'] button[data-automation-id='ok_button']")]
        public IWebElement NoCriteriaDialog;

        public static By PolicyName = By.CssSelector("form[id='editableData'] input[name='Name']");
        public static By PolicyDescription = By.CssSelector("textarea[name='Description']");

        //public static By CriteriaName = By.CssSelector("form[id='dialog-editableData'] input[name='Name']");
        //public static By CriteriaDescription = By.CssSelector("form[id='dialog-editableData'] textarea[name='Description']");

        public static By PolicySearchByName = By.CssSelector("form[data-automation-id='search_criteria'] input[name='Name']");

        public const string StatusSuccess = "status_success";

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_criteria_button']")]
        public IWebElement AddCriteriaDialog;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='admission_criteria_maintenance'] a[data-automation-id='add_new_criteria_button']")]
        public IWebElement AddCriteriaButton;

        public static By CriteriaName = By.CssSelector("div[data-automation-id='admission_criteria_maintenance'] input[name='Name']");
        public static By CriteriaDescription = By.CssSelector("div[data-automation-id='admission_criteria_maintenance'] textarea[name='Description']");

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='admission_criteria_maintenance'] input[name='AdmissionsCriteriaType.dropdownImitator']")]
        public IWebElement CriteriaType;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='admission_criteria_maintenance'] a[data-automation-id='well_know_action_save']")]
        public IWebElement SaveCriteriaButton;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        public IWebElement SelectCriteriaButton;
                
        public static By CriteriaPriority = By.CssSelector("input[name$='Priority']");

        public String getPolicyTitle()
        {

            waiter.Until(ExpectedConditions.ElementExists(By.CssSelector("span[data-automation-id='admission_policies_header_title']")));
            String PolicyTitle = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id='admission_policies_header_title']")).Text;
            return PolicyTitle;
        }

        public String setPolicyName()
        {

            waiter.Until(ExpectedConditions.ElementExists(PolicyName));
            String PolicyNameText = Utilities.GenerateRandomString(10, "Policy");
            WebContext.WebDriver.FindElement(PolicyName).SendKeys(PolicyNameText);
            return PolicyNameText;
        }

        public String getPolicyName()
        {

            String policyName = WebContext.WebDriver.FindElement(PolicyName).Text;
            return policyName;
        }

        public String getPolicyDescription()
        {

            String policyDescription = waiter.Until(ExpectedConditions.ElementExists(PolicyDescription)).GetText();
            return policyDescription;
        }

        public String setPolicyDescription()
        {

            waiter.Until(ExpectedConditions.ElementExists(PolicyName));
            String policyDescription = Utilities.GenerateRandomString(20, "Policy Description ");
            WebContext.WebDriver.FindElement(PolicyDescription).SendKeys(policyDescription);
            return policyDescription;
        }
        public void ClickAddPolicyButton()
        {
         //   Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddPolicyButton)).Click();
        }
        //public void ClickAddCriteriaButton()
        //{
        //    Thread.Sleep(3000);
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(AddCriteriaButton)).Click();
        //}
        public void ClickAddNewCriteriaButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddNewCriteriaButton)).Click();
        }


        public void ClickSaveButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton)).Click();
        }
        public void ClickDeleteButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteButton)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(DeleteContinueButton)).Click();

        }
        public void ClickNoCriteriaDialogButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(NoCriteriaDialog)).Click();
        }

        public void WaitForStatus()
        {
         //   Thread.Sleep(2000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.CssSelector(SeleniumHelper.AutomationId(StatusSuccess)));
        }

        public PolicyDetails ClickSearchButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
            SearchButton.Click();
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            WaitUntillAjaxRequestCompleted();
            return new PolicyDetails();
        }


        public PolicyDetails SearchByName(String policyname)
        {
            WebContext.WebDriver.FindElement(PolicySearchByName).SendKeys(policyname);
     //     ClickSearchButton();
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchResult));
            SearchResult.Click();
            WaitUntillAjaxRequestCompleted();
            SearchResult.Text.Contains(policyname);
            return new PolicyDetails();
        }

        public void ClickAddCriteriaDialog()
        {
         //   Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddCriteriaDialog)).Click();
        }

            public void ClickAddCriteriaButton()
        {
         //   Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddCriteriaButton)).Click();
        }

        public String setCriteriaName()
        {

            waiter.Until(ExpectedConditions.ElementExists(CriteriaName));
            String CriteriaNameText = Utilities.GenerateRandomString(10, "Criteria");
            WebContext.WebDriver.FindElement(CriteriaName).SendKeys(CriteriaNameText);
            return CriteriaNameText;
        }

        public String setCriteriaDescription()
        {

            waiter.Until(ExpectedConditions.ElementExists(CriteriaName));
            String criteriaDescription = Utilities.GenerateRandomString(20, "Criteria Description ");
            WebContext.WebDriver.FindElement(CriteriaDescription).SendKeys(criteriaDescription);
            return criteriaDescription;
        }

        public void SetCriteriaType(String Type)
        {
            SeleniumHelper.ChooseSelectorOption(CriteriaType, Type);
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickSaveCriteriaButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveCriteriaButton)).Click();
        }

        public void ClickSelectCriteriaButton()
        {
        //    Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SelectCriteriaButton)).Click();
        }

        public String setCriteriaPriority()
        {

            waiter.Until(ExpectedConditions.ElementExists(CriteriaPriority));
            String CriteriaNameText = Utilities.GenerateRandomString(10, "Criteria");
            WebContext.WebDriver.FindElement(CriteriaPriority).SendKeys("1");
            Thread.Sleep(500);
            return CriteriaNameText;
        }
    }
}
