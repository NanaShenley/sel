using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using System.Windows;
using System.Windows.Forms;
using NUnit.Framework;
using System.Windows.Controls;
using System.Threading;
using SharedComponents.Helpers;
using AddressBook.Components;
using AddressBook.Components.Pages;
using TestSettings;


namespace AddressBook.Components
{
    public class AddressBookPopup
    {
        // Basic Details
        public const string cssforNamePopUp = "detail_displayname";
        public const string cssforGenderPopUp = "detail_gender";
        public const string cssforDOBPopUp = "detail_dateofbirth";
        private const string cssForLinks = "quick_link_btn0";
        private const string cssForContacts = "contact_sheet_additional_title";

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(AddressBookElements.Timeout));

        public AddressBookPopup()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp))); //Parul: Make changes in this as it wont be pupil always
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public bool GetPupilBasicDetails()
        {
            var displayNamePopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp)));
            var pupilNamePopup = displayNamePopup[0].Text; /* Popup Pupil name*/

            var displayGenderPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforGenderPopUp)));
            var pupilGenderPopup = displayGenderPopup[0].Text;

            var displayDOBPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforDOBPopUp)));
            var pupilDOBPopup = displayDOBPopup[0].Text;

            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));  // Elements in result tile
            String Name = elements[0].Text;
            int index1 = Name.IndexOf(",");
            String PupilName1 = (Name.ToString().Substring((index1 + 2)));
            String PupilName2 = (Name.ToString().Substring(0, (index1)));
            String pupilName = PupilName1 + " " + PupilName2;
            StringAssert.AreEqualIgnoringCase(pupilName, pupilNamePopup, "Failed");   //Verify if name displayed in the new popup is same as name in the results fetched

            TestResultReporter.Log("<b>Pupil Name - </b> " + pupilNamePopup);
            TestResultReporter.Log("<b> Gender -  </b>" + pupilGenderPopup);

            int index = pupilDOBPopup.IndexOf(':');
            TestResultReporter.Log("<b> DOB -  </b>" + pupilDOBPopup.ToString().Substring(index + 1));
            return ((pupilNamePopup != null) && (pupilGenderPopup != null) && (pupilDOBPopup != null));// All are mandatory fields
        }
        //Method to check for Name and DOB details for Pupil. 
        public bool GetPupilBasicDetailsNameDOB()
        {
            var displayNamePopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp)));
            var pupilNamePopup = displayNamePopup[0].Text;

            var displayDOBPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforDOBPopUp)));
            var pupilDOBPopup = displayDOBPopup[0].Text;

            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("PreferredListName_Learner")));

            String Name = elements[0].Text;
            String fName = Name.ToString().Substring((Name.IndexOf(",") + 2));
            String lName = Name.ToString().Substring(0, Name.IndexOf(","));

            String pupilName = fName + lName;
            StringAssert.AreNotEqualIgnoringCase(pupilName, pupilNamePopup, "Failed");

            TestResultReporter.Log("<b>Pupil Name - </b> " + pupilNamePopup);

            int index = pupilDOBPopup.IndexOf(':');
            TestResultReporter.Log("<b> DOB -  </b>" + pupilDOBPopup.ToString().Substring(index + 1));
            return ((pupilNamePopup != null) && (pupilDOBPopup != null));// All are mandatory fields

        }

        //Method to check for gender details for Pupil - Returns true if Gender details are blank.
        public bool CheckPupilGenderDetailsForBlank()
        {
            var displayGenderPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforGenderPopUp)));
            var pupilGenderPopup = displayGenderPopup[0].Text;

            TestResultReporter.Log("<b> Gender -  </b>" + pupilGenderPopup);

            return (pupilGenderPopup == "");

        }

        public bool GetPupilContactBasicDetails()
        {
            var displayNamePopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp)));
            var pupilContactNamePopup = displayNamePopup[0].Text; /* Popup Pupil name*/

            var displayGenderPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforGenderPopUp)));
            var pupilContactGenderPopup = displayGenderPopup[0].Text;

            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_LearnerContact")));  // Elements in result tile
            String Name = elements[0].Text;
            int index1 = Name.IndexOf(",");
            String PupilContactName1 = (Name.ToString().Substring((index1 + 2)));
            String PupilContactName2 = (Name.ToString().Substring(0, (index1)));
            String pupilContactName = PupilContactName1 + " " + PupilContactName2;
            //   StringAssert.AreEqualIgnoringCase(pupilContactName, pupilContactNamePopup, "Failed");   //Verify if name displayed in the new popup is same as name in the results fetched
            //Parul: Check for Title in names
            TestResultReporter.Log("<b>Pupil contact Name - </b> " + pupilContactNamePopup);
            TestResultReporter.Log("<b> Gender -  </b>" + pupilContactGenderPopup);

            return ((pupilContactNamePopup != null) && (pupilContactGenderPopup != null));// All are mandatory fields
        }


        public bool GetStaffBasicDetails()
        {
            var displayNamePopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp)));
            var staffNamePopup = displayNamePopup[0].Text; /* Popup Pupil name*/

            var displayGenderPopup = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssforGenderPopUp)));
            var staffGenderPopup = displayGenderPopup[0].Text;

            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("search_result_tile_Staff")));  // Elements in result tile
            String Name = elements[0].Text;
            int index1 = Name.IndexOf(",");
            String StaffName1 = (Name.ToString().Substring((index1 + 2)));
            String StaffName2 = (Name.ToString().Substring(0, (index1)));
            String staffName = StaffName1 + " " + StaffName2;
            //   StringAssert.AreEqualIgnoringCase(pupilContactName, pupilContactNamePopup, "Failed");   //Verify if name displayed in the new popup is same as name in the results fetched
            //Parul: Check for Title in names
            TestResultReporter.Log("<b>Staff Name - </b> " + staffNamePopup);
            TestResultReporter.Log("<b> Gender -  </b>" + staffGenderPopup);

            return ((staffNamePopup != null) && (staffGenderPopup != null));// All are mandatory fields
        }
        public bool IsPupilTelephoneDisplayed()
        {
            try
            {
                WebContext.WebDriver.FindElement(AddressBookElements.TelephoneElement);
                IWebElement PupilTelephone = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("communication_telephone_basic")));
                TestResultReporter.Log("<b>Telephone Number -</b>   " + PupilTelephone.Text);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public bool IsPupilTelephoneIconDisplayed()
        {
            IWebElement IconTelephone = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("communication_telephone_icon_basic")));
            return IconTelephone.Displayed;
        }


        public bool IsPupilEmailIconDisplayed()
        {
            IWebElement IconEmail = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("communication_email_icon_basic")));
            return IconEmail.Displayed;
        }

        public bool IsPupilAddressIconDisplayed()
        {
            IWebElement IconAddress = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("meaning_location_icon_basic")));
            return IconAddress.Displayed;
        }


        public bool IsEmailDisplayed()
        {
            try
            {
               
                WebContext.WebDriver.FindElement(AddressBookElements.EmailElement);
                IWebElement Email = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("communication_email_basic")));
                TestResultReporter.Log("<b>Email Address -</b>   " + Email.Text);
                return true;
            }
            catch(NoSuchElementException e)
            {
                return false;
            }
        }

        public bool IsAddressDisplayed()
        {

            try
            {
                WebContext.WebDriver.FindElement(AddressBookElements.AddressElement);
                IWebElement Address = WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("meaning_home_icon_basic")));
                TestResultReporter.Log("<b> Address -</b>   " + Address.Text);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }

        }

        public bool IsLinkDisplayed()
        {
            try
            {
                WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId("quick_link_btn0")));
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public int HaveAssociatedPupil()
        {
            var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("contact_card_container")));  // Elements in result tile
            return elements.Count;
        }

        public void GetPupilDetails()
        {
            if (HaveAssociatedPupil() >= 1)
            {
                for (int i = 1; i < HaveAssociatedPupil(); i++)
                {
                    var elements = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("contact_card_container")));  // Elements in result tile
                }
            }
            else
            {
                TestResultReporter.Log("No associated Pupils here");
            }

        }

        public void WaitForDialogToAppear()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(SeleniumHelper.AutomationId(cssforNamePopUp)));
        }

        public void WaitForConfirmationDialogToAppear()
        {
            BaseSeleniumComponents.WaitUntilDisplayed(SeleniumHelper.SelectByDataAutomationID("save_continue_commit_dialog"));
        }

        public void ClickPupilDetailsLink()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks))));
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;

            if (linksPopUpCount >= 1)
            {
                linksPopUp[0].Click();
                POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                //            IWebElement quickLink = SeleniumHelper.Get(By.LinkText("Pupil Details")); //Pupil Details Aut-Id quick_link_btn0
                //           quickLink.Click();

            }
        }

        public void ClickCommunicationLogLink()
        {
            //quick_link_btn1
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(SeleniumHelper.AutomationId("quick_link_btn1"))));
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId("quick_link_btn1")));
            var linksPopUpCount = linksPopUp.Count;

            if (linksPopUpCount >= 1)
            {
                linksPopUp[0].Click();
                POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

            }
        }

        public void ClickPupilContactsDetailsLink()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks))));
            
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;

            if (linksPopUpCount >= 1)
            {
                linksPopUp[0].Click();
                POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                //    IWebElement quickLink = SeleniumHelper.Get(By.LinkText("Pupil Contact Details")); //Pupil Details Aut-Id quick_link_btn0
                //   quickLink.Click();

            }
        }

        public void ClickStaffDetailsLink()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks))));
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;
            if (linksPopUpCount >= 1)
            {
                linksPopUp[0].Click();
              //  POM.Helper.Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                //      IWebElement quickLink = SeleniumHelper.Get(By.LinkText("Staff Details")); //Pupil Details Aut-Id quick_link_btn0
                //     quickLink.Click();
            }
        }


        public void ClickPupiRecordAndLogLink()
        {
            WebDriverWait wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            wait.Until(ExpectedConditions.ElementExists(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks))));
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;

            if (linksPopUpCount >= 1)
            {
                linksPopUp[1].Click();
          //      IWebElement quickLink = SeleniumHelper.Get(By.LinkText("Pupil Details")); //Pupil Details Aut-Id quick_link_btn0
          //      quickLink.Click();

            }
        }

        public void ClickPupilLogLink()
        {
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;

            if (linksPopUpCount >= 1)
            {
                linksPopUp[0].Click();
                IWebElement quickLink = SeleniumHelper.Get(By.LinkText("Pupil Log")); //Pupil Details Aut-Id quick_link_btn0
                quickLink.Click();

            }
        }


        public bool TestPupilLogLink_QuickLinkPresent()
        {
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));
            var linksPopUpCount = linksPopUp.Count;


            linksPopUp[0].Click();
            try
            {
                WebContext.WebDriver.FindElement(By.LinkText("Pupil Log")); //Pupil Details Aut-Id quick_link_btn0
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }

        }

        //Method to validate presence of quick link on searched pupil record. Return true of link is not present. 
        public bool TestPupilLogLink_QuickLinkNotPresent()
        {
            var linksPopUp = WebContext.WebDriver.FindElements(By.CssSelector(SeleniumHelper.AutomationId(cssForLinks)));

            if (linksPopUp.Count == 0)
                return true;
            else
                return false;
        }

        //Method to check presene of associated contacts for Pupil
        //Returns true if field is listed else false
        public bool IsPupilAssociactedContactDisplayed()
        {
            try
            {
                WebContext.WebDriver.FindElement(By.CssSelector(SeleniumHelper.AutomationId(cssForContacts)));
                return true;
            }

            catch (NoSuchElementException e)
            {
                return false;
            }


        }
        public bool IsPupilImageDisplayed()
        {
            try
            {
                IWebElement ele = WebContext.WebDriver.FindElement(AddressBookElements.DisplayPopup);
                ele.FindElement(By.TagName("img"));
                return true;
            }

            catch (NoSuchElementException e)
            {
                return false;
            }

        }
    }






}
