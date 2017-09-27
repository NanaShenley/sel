using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.Generic;
using System.Threading;
using TestSettings;
using WebDriverRunner.webdriver;
using Assessment.Components;

namespace POM.Components.HomePages
{
    public class AssessmentQuickLinks : BaseSeleniumComponents
    {
        public void CheckIfAssessmentQuickLinkExists(SeleniumHelper.iSIMSUserType userType)
        {
            By loc = null;
            WebDriverWait wait = null;
            switch (userType)
            {
                case SeleniumHelper.iSIMSUserType.AssessmentCoordinator:
                    loc = By.CssSelector("a[data-ajax-url$='/Assessment/AssessmentMarksheet/LoadBusinessServicesShellQuickLinks']");
                    wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
                    wait.Until(driver => driver.FindElement(loc));
                    Assert.IsNotNull(loc, "Quick Link not found for assessment coordinator.");
                    break;

                case SeleniumHelper.iSIMSUserType.ClassTeacher:
                    loc = By.CssSelector("a[data-ajax-url$='/Assessment/AssessmentMarksheet/LoadTeacherShellQuickLinks']");
                    wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
                    wait.Until(driver => driver.FindElement(loc));
                    Assert.IsNotNull(loc, "Quick Link not found for class teacher.");
                    break;
            }
        }

        public void OpenAssessmentQuickLinksDropdown(SeleniumHelper.iSIMSUserType userType)
        {
            string selector = string.Empty;
            By eleToClick = null;
            switch (userType)
            {
                case SeleniumHelper.iSIMSUserType.AssessmentCoordinator:
                    selector = "a[data-ajax-url$='/Assessment/AssessmentMarksheet/LoadBusinessServicesShellQuickLinks']";
                    eleToClick = By.CssSelector(selector);
                    break;

                case SeleniumHelper.iSIMSUserType.ClassTeacher:
                    selector = "a[data-ajax-url$='/Assessment/AssessmentMarksheet/LoadTeacherShellQuickLinks']";
                    eleToClick = By.CssSelector(selector);
                    break;
            }
            WaitUntilDisplayed(BrowserDefaults.WaitLoading, By.CssSelector(selector));
            Thread.Sleep(10000);
            WaitUntillAjaxRequestCompleted();
            SeleniumHelper.Get(eleToClick).Click();
            WaitUntillAjaxRequestCompleted();
        }

        public void ClickAndVerifyManageTemplatesLink(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-manage-create-marksheet']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find manage templates link");
        }

        public void ClickAndVerifySchemeCreateNew(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Create-new-Scheme']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find Create New Scheme link");
        }


        public void ClickAndVerifySchemeNewFromExisting(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Create-From-Existing-Scheme']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find create from existing Scheme link");
        }

        public void ClickAndVerifySchemeModify(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Modify-Scheme']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find create from existing Scheme link");
        }

        public void ClickAndVerifyTopic(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Topic']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Topic_open_button']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find Topic link");
        }


        public void ClickAndVerifyTopicNewFromExisting(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Create-From-Existing-Topic']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find create from existing Topic link");
        }

        public void ClickAndVerifyTopicModify(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-Modify-Topic']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Create']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find create from existing Topic link");
        }


        public void ClickAndVerifyMyMarksheetsLink(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-my-marksheets']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Marksheets_open_button']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find my marksheets link");
        }

        public void ClickAndVerifyPOSMarksheetLink(SeleniumHelper.iSIMSUserType userType)
        {
            By eleToClick = By.CssSelector("a[data-automation-id$='quick-link-pos-marksheet']");
            By eleToWaitFor = By.CssSelector("a[data-automation-id$='tab_Programme_of_Study_Tracking_close_button']");
            SeleniumHelper.ClickAndWaitFor(eleToClick, eleToWaitFor);
            WaitUntillAjaxRequestCompleted();
            Assert.IsNotNull(eleToWaitFor, "Unable to find POS Marksheet link");
        }

        /// <summary>
        /// Selects the first class available from the Class Picker in toolbar
        /// </summary>
        /// <returns></returns>
        public string SelectFirstClassFromQuickLink()
        {
            var quickLinkClassPicker = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='QuickLinkClassPicker']"));
            quickLinkClassPicker.Click();
            string selectedClass = "";
            var list = WebContext.WebDriver.FindElement(By.CssSelector(".mega-menu-list"));

            IList<IWebElement> classLabels = list.FindElements(By.CssSelector(".mega-menu-list-item"));
            if(classLabels.Count > 0)
            {
                selectedClass = classLabels[0].GetText();
                classLabels[0].Click();
                ElementAccessor.WaitForAjaxReady(By.CssSelector(".locking-mask-loading")); 
            }
            return selectedClass;
        }
    }
}