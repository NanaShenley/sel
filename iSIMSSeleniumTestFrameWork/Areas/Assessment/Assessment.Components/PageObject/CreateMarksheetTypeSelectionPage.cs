using System;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class CreateMarksheetTypeSelectionPage : BaseSeleniumComponents
    {
        public CreateMarksheetTypeSelectionPage()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        // ReSharper disable UnassignedField.Compiler
        [FindsBy(How = How.LinkText, Using = "Create Marksheet Template")]
        private IWebElement _menuType;

        public MarksheetType MarksheetTypeSelection(string marksheetType)
        {
            //_menuType.Click();
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            switch (marksheetType)
            {
                case "Clone Marksheet":
                    WebContext.WebDriver.FindElement(MarksheetConstants.NewFromExistingPalette).Click();
                    break;
                case "New Template(Blank)":
                    WaitForElement(new TimeSpan(0, 0, 0, 15), MarksheetConstants.MarksheetWithLevels);
                    WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevels).Click();
                    MarksheetBuilder marksheetBuilder = new MarksheetBuilder();
                    return marksheetBuilder;
                case "Modify Existing Template":
                    WaitForElement(new TimeSpan(0, 0, 0, 15), MarksheetConstants.ModifyMarksheet);
                    WebContext.WebDriver.FindElement(MarksheetConstants.ModifyMarksheet).Click();
                    MarksheetBuilder _marksheetBuilder = new MarksheetBuilder();
                    return _marksheetBuilder;
                case "New from Existing Template":
                    WaitForElement(new TimeSpan(0, 0, 0, 15), MarksheetConstants.CopyTemplate);
                    WebContext.WebDriver.FindElement(MarksheetConstants.CopyTemplate).Click();
                    MarksheetBuilder _copyMarksheetBuilder = new MarksheetBuilder();
                    return _copyMarksheetBuilder;
                case "New Template":
                    WaitForElement(new TimeSpan(0, 0, 0, 15), MarksheetConstants.MarksheetWithLevelsNew);
                    WebContext.WebDriver.FindElement(MarksheetConstants.MarksheetWithLevelsNew).Click();
                    CreateMarksheet createMarksheet = new CreateMarksheet();
                    return createMarksheet;
                case "Create Marksheet(s)":
                    WaitForElement(new TimeSpan(0, 0, 0, 15), MarksheetConstants.AssignTemplateGroupAndFilter);
                    WebContext.WebDriver.FindElement(MarksheetConstants.AssignTemplateGroupAndFilter).Click();
                    AssignTemplateGroupAndFilter assignTemplateGroupAndFilter = new AssignTemplateGroupAndFilter();
                    return assignTemplateGroupAndFilter;
                case "Programme of Study Tracking":
                    WebContext.WebDriver.FindElement(MarksheetConstants.ProgrammeOfStudyTracking).Click();
                    break;
                case "Tracking Grid":
                    WebContext.WebDriver.FindElement(MarksheetConstants.TrackingGrid).Click();
                    break;
            }
            return null;
        }

        public void TakePrint(bool takeScreenPrint)
        {
            if (takeScreenPrint)
                WebContext.Screenshot();
        }

        //public MarksheetPage SelectNext()
        //{
        //    _next.Click();
        //    return new SchoolSelectionPage();
        //}
    }
}
