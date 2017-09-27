using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using SharedComponents.BaseFolder;
using OpenQA.Selenium;
using SeSugar;
using OpenQA.Selenium.Support.UI;
//using SharedComponents.Helpers;
using System.Threading;
using PageObjectModel.Helper;
using PageObjectModel;

namespace Admissions.Component
{
    public class EnquiryDetails : BaseSeleniumComponents
    {

        public EnquiryDetails()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1200));

        }
        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(1200));

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='enquiries_header_title']")]
        public IWebElement EnquiriesTitle;
        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_enquiry_button']")]
        public IWebElement AddEnquiryButton;

        [FindsBy(How = How.CssSelector, Using = "input[name='EnquirerTitle.dropdownImitator']")]
        public IWebElement EnquirerTitle;

        [FindsBy(How = How.CssSelector, Using = "input[name='Gender.dropdownImitator']")]
        public IWebElement EnquirerGender;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='child_dialog'] input[name='Gender.dropdownImitator']")]
        public IWebElement ChildGender;

        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='child_dialog'] input[name*='RelationshipType.dropdownImitator']")]
        public IWebElement ChildRelationship;

        [FindsBy(How = How.CssSelector, Using = "input[name='Occupation.dropdownImitator']")]
        public IWebElement EnquirerOccupation;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_enquirer_button']")]
        public IWebElement AddEnquiryContact;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='add_new_enquirer_button']")]
        public IWebElement AddNewEnquiryContact;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_new_child_button']")]
        public IWebElement AddChild;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='ok_button']")]
        public IWebElement OkButton;
        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_submit']")]
        public IWebElement SearchButton;
        [FindsBy(How = How.CssSelector, Using = "form[data-section-id='dialog-searchCriteria'] button[data-automation-id='search_criteria_submit']")]
        public IWebElement EnquirerSearchButton;
        [FindsBy(How = How.CssSelector, Using = "div[data-automation-id='search_result']")]
        public IWebElement SearchResult;
        [FindsBy(How = How.CssSelector, Using = "div[data-section-id='dialog-searchResults'] div[data-automation-id='search_result']")]
        public IWebElement EnquirerSearchResult;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_email_address_button']")]
        public IWebElement AddEmailButton;
        [FindsBy(How = How.CssSelector, Using = "input[name*='EmailLocationType.dropdownImitator']")]
        public IWebElement EmailLocationType;
        [FindsBy(How = How.CssSelector, Using = "input[name*='PhoneLocationType.dropdownImitator']")]
        public IWebElement PhoneLocationType;

        [FindsBy(How = How.CssSelector, Using = "input[name='IsHighCommitment']")]
        public IWebElement HighCommitment;

        [FindsBy(How = How.CssSelector, Using = "input[name='IsFollowUpRequired']")]
        public IWebElement Followuprequired;

        [FindsBy(How = How.CssSelector, Using = "input[name='HighCommitment']")]
        public IWebElement HighCommitmentSearch;

        [FindsBy(How = How.CssSelector, Using = "input[name='FollowUpRequired']")]
        public IWebElement FollowuprequiredSearch;


        [FindsBy(How = How.CssSelector, Using = "input[name='ShowInactive']")]
        public IWebElement ShowInactive;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='search_criteria_advanced']")]
        public IWebElement ClickMoreFilters;

        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='Extended_Dropdown']")]
        public IWebElement ClickActiveDropdown;

        [FindsBy(How = How.CssSelector, Using = "li[data-automation-id='Inactive']")]
        public IWebElement ClickInactive;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='add_telephone_number_button']")]
        public IWebElement AddPhoneButton;



        [FindsBy(How = How.CssSelector, Using = "a[data-automation-id='well_know_action_save']")]
        public IWebElement SaveButton;

        //public static By EnquiriesTitle = By.CssSelector("span[dataautomatioid='enquiries_header_title']");

        public static By Notes = By.CssSelector("textarea[name='Notes']");
        public static By DateOfEnquiry = By.CssSelector("input[name='DateOfEnquiry']");
        public static By AddNewContact = By.CssSelector("button[data-automation-id='add_new_contact_button']");

        public static By EnquiryContactNameSearch = By.CssSelector("input[name='Name']");
        public static By EnquirerContactNameSearch = By.CssSelector("form[data-section-id='dialog-searchCriteria'] input[name='Name']");

        public static By EnquiryContactSurname = By.CssSelector("input[name='Surname']");
        public static By EnquiryContactForename = By.CssSelector("input[name='Forename']");
        public static By NotesSection = By.CssSelector("div[data-section-id='NotesLogSection'] span[class='read-less-content']");
        public static By EnquiryContactNotes = By.CssSelector("textarea[name='Notes']");
        public static By EnquiryChildForename = By.CssSelector("div[data-automation-id='child_dialog'] input[name='Forename']");
        public static By EnquiryChildSurname = By.CssSelector("div[data-automation-id='child_dialog']  input[name='Surname']");
        public static By EnquiryChildDOB = By.CssSelector("div[data-automation-id='child_dialog']  input[name='DateOfBirth']");
        public static By EnterEmailAddress = By.CssSelector("input[name*='.EmailAddress']");
        public static By EnterPhone = By.CssSelector("input[name*='.TelephoneNumber']");

        public const string StatusSuccess = "status_success";

        public String GetEnquiryitle()
        {
            waiter.Until(ExpectedConditions.ElementExists(By.CssSelector("span[data-automation-id='enquiries_header_title']")));
            String Enquiryitle = WebContext.WebDriver.FindElement(By.CssSelector("span[data-automation-id='enquiries_header_title']")).Text;
            return Enquiryitle;
        }
        public void AddButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddEnquiryButton)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddEnquiryContact));

        }

        public void AddEnquiryContactButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddEnquiryContact)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(OkButton));
        }

        public void AddNewEnquirerContactButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddNewEnquiryContact)).Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(OkButton));
        }


        public void AddChildButtonClick()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddChild));
            AddChild.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(OkButton));

        }

        public String EnquirerBasicDetails()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryContactForename));
            String Forename = Utilities.GenerateRandomString(10);
            WebContext.WebDriver.FindElement(EnquiryContactForename).SendKeys(Forename);
            waiter.Until(ExpectedConditions.ElementExists(EnquiryContactSurname));
            String Surname = Utilities.GenerateRandomString(10);
            WebContext.WebDriver.FindElement(EnquiryContactSurname).SendKeys(Surname);
            ClickOkButton();
            return Surname;
        }

        public void IsHighCommitment()
        {
            HighCommitment.Click();
            HighCommitment.GetValue();
        }
        public void IsFollowuprequired()
        {
            Followuprequired.Click();
            Followuprequired.GetValue();
        }

        public string SetEnquirerForename(string forename="")
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryContactForename));
            WebContext.WebDriver.FindElement(EnquiryContactForename).ClearText();
            if (forename == string.Empty)
            {
                forename = Utilities.GenerateRandomString(10);
            }
            WebContext.WebDriver.FindElement(EnquiryContactForename).SendKeys(forename);
            return forename;
        }

        public void SetEnquirerTitle(String Title)
        {
            SeleniumHelper.ChooseSelectorOption(EnquirerTitle, Title);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetEnquirerGender(String Gender)
        {
            SeleniumHelper.ChooseSelectorOption(EnquirerGender, Gender);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetEnquirerOccupation(String Occupation)
        {
            SeleniumHelper.ChooseSelectorOption(EnquirerOccupation, Occupation);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetEmailType(String EmailType)
        {
            SeleniumHelper.ChooseSelectorOption(EmailLocationType, EmailType);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetPhoneLocationType(String PhoneType)
        {
            SeleniumHelper.ChooseSelectorOption(PhoneLocationType, PhoneType);
            WaitUntillAjaxRequestCompleted();
        }

        public String SetEnquirerSurname(string surname = "")
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryContactSurname));
            WebContext.WebDriver.FindElement(EnquiryContactSurname).ClearText();
            if (surname == string.Empty)
            {
                surname = Utilities.GenerateRandomString(10);
            }
            WebContext.WebDriver.FindElement(EnquiryContactSurname).SendKeys(surname);
            return surname;
        }
        public String SetEnquirerNotes()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryContactNotes));
            String Notes = "Test Notes" + Utilities.GenerateRandomString(10);
            WebContext.WebDriver.FindElement(EnquiryContactNotes).SendKeys(Notes);
            return Notes;
        }
        public String getEnquirerNotes()
        {
            String Notes = waiter.Until(ExpectedConditions.ElementIsVisible(NotesSection)).GetText();

            return Notes;
        }

        public void SetEmailAddress()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnterEmailAddress));
            WebContext.WebDriver.FindElement(EnterEmailAddress).SendKeys("abc@xyz.com");
        }
        public void SetPhone()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnterPhone));
            WebContext.WebDriver.FindElement(EnterPhone).SendKeys("231324132123");
        }

        public EnquiryDetails ClickOkButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(OkButton));
            OkButton.Click();
            while (true)
            {
                if (OkButton.GetAttribute("disabled") != "true")
                    break;
            }
            Wait.WaitForAjaxReady();
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddEnquiryContact));
            return new EnquiryDetails();
        }

        public EnquiryDetails ClickSearchButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(SearchButton));
            SearchButton.Click();
            while (true)
            {
                if (SearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            WaitUntillAjaxRequestCompleted();
            return new EnquiryDetails();
        }

        public EnquiryDetails ClickEnquirerSearchButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(EnquirerSearchButton));
            EnquirerSearchButton.Click();
            while (true)
            {
                if (EnquirerSearchButton.GetAttribute("disabled") != "true")
                    break;
            }
            WaitUntillAjaxRequestCompleted();
            return new EnquiryDetails();
        }

        public EnquiryDetails SearchByName(String Surname)
        {
            WebContext.WebDriver.FindElement(EnquiryContactNameSearch).SendKeys(Surname);
            ClickSearchButton();
            SearchResult.Click();
            WaitUntillAjaxRequestCompleted();
            SearchResult.Text.Contains(Surname);
            return new EnquiryDetails();
        }

        public EnquiryDetails SearchEnquirerByName(String Surname)
        {
            WebContext.WebDriver.FindElement(EnquirerContactNameSearch).SendKeys(Surname);
            ClickEnquirerSearchButton();
            EnquirerSearchResult.Click();
            WaitUntillAjaxRequestCompleted();
            EnquirerSearchResult.Text.Contains(Surname);
            return new EnquiryDetails();
        }

        public EnquiryDetails SearchContainsName(String Surname)
        {
            ClickSearchButton();
            SearchResult.Click();
            WaitUntillAjaxRequestCompleted();
            SearchResult.Text.Contains(Surname);
            return new EnquiryDetails();
        }


        public void ClickSaveButton()
        {
            Thread.Sleep(3000);
            waiter.Until(ExpectedConditions.ElementToBeClickable(SaveButton)).Click();
        }

        public void WaitForStatus()
        {
            Thread.Sleep(2000);
            BaseSeleniumComponents.WaitUntilDisplayed(new TimeSpan(0, 0, 0, 5), By.CssSelector(SeleniumHelper.AutomationId(StatusSuccess)));
        }

        public void ClickEmailButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddEmailButton)).Click();
        }
        public void ClickPhoneButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(AddPhoneButton)).Click();

        }
        public void SetChildGender(String Gender)
        {
            SeleniumHelper.ChooseSelectorOption(ChildGender, Gender);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetChildRelationship(String relation)
        {
            SeleniumHelper.ChooseSelectorOption(ChildRelationship, relation);
            WaitUntillAjaxRequestCompleted();
        }
        public void SetChildDOB(String DOB)
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryChildDOB));
            //String DateOfBirth = "04/04/2010";
            WebContext.WebDriver.FindElement(EnquiryChildDOB).SendKeys(DOB);
        }
        public void SetChildSurname()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryChildSurname));
            String Surname = Utilities.GenerateRandomString(10);
            WebContext.WebDriver.FindElement(EnquiryChildSurname).SendKeys(Surname);
        }

        public void SetChildForename()
        {
            waiter.Until(ExpectedConditions.ElementExists(EnquiryChildForename));
            String forename = Utilities.GenerateRandomString(10);
            WebContext.WebDriver.FindElement(EnquiryChildForename).SendKeys(forename);
        }

        public void MakeEnquiryInactive()
        {
            ClickActiveDropdown.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(ClickInactive));
            ClickInactive.Click();
        }
        public void MoreFilters()
        {
            ClickMoreFilters.Click();
            waiter.Until(ExpectedConditions.ElementToBeClickable(HighCommitmentSearch));
        }

        public void SelectAdvancedfilter(String AdvanceFilter, String EnquirerSurname)
        {
            switch (AdvanceFilter)
            {
                case "HighCommitment":
                    MoreFilters();
                    HighCommitmentSearch.Click();
                    SearchContainsName(EnquirerSurname);
                    break;

                case "FollowupRequired":

                    MoreFilters();
                    FollowuprequiredSearch.Click();
                    SearchContainsName(EnquirerSurname);
                    break;

                case "Inactive":
                    MakeEnquiryInactive();
                    ShowInactive.Click();
                    SearchContainsName(EnquirerSurname);
                    break;

            }

        }

    }
}
