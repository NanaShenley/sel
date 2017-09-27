using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class AddGroups : BaseSeleniumComponents
    {

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));

        public AddGroups()
        {
            WebContext.WebDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] div.slider-header-title")]
        private IWebElement GroupHeader;

        [FindsBy(How = How.CssSelector, Using = "button[data-automation-id='next-group']")]
        private IWebElement GroupDoneButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] [data-marksheet-groups-back]")]
        private IWebElement GroupBackButton;

        [FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='home'] div.slider-header-title")]
        private IWebElement MarksheetBuilderHeader;

        //[FindsBy(How = How.CssSelector, Using = "div[data-slide-panel-id='groups'] [data-automation-id='groupclosebutton']")]
        //private IWebElement GroupCloseButton;

        //Group Selection Panel Elements List

        private static By YearGroupsCheckBox = By.CssSelector("input[name='YearGroups.SelectedIds']");
        private static By ClassesCheckBox = By.CssSelector("input[name='Classes.SelectedIds']");

        /// <summary>
        /// Selects Year Group based on the Year Group Count
        /// </summary>
        public void SelectNoOfYearGroups(int NoOfYearGroups)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            waiter.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(YearGroupsCheckBox));
            ReadOnlyCollection<IWebElement> YearGroupCheckboxlistYG = WebContext.WebDriver.FindElements(YearGroupsCheckBox);

            int i = 0;
            foreach (IWebElement YearGroupElem in YearGroupCheckboxlistYG)
            {
                if (YearGroupElem.Displayed == true && NoOfYearGroups != 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(YearGroupElem));
                    if (YearGroupElem.Selected == false)
                    {
                        YearGroupElem.Click();
                        i++;
                    }
                }
                if (i == NoOfYearGroups)
                    break;
            }

        }

        /// <summary>
        /// Selects a specific Year Group
        /// </summary>
        public void SelectYearGroup(string YearGroup)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> YearGroupCheckboxlistYG = WebContext.WebDriver.FindElements(YearGroupsCheckBox);
            foreach (IWebElement eachelement in YearGroupCheckboxlistYG)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == YearGroup)
                {
                    eachelement.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// Selects Classes based on the Class Count
        /// </summary>
        public void SelectNoOfClasses(int NoOfClasses)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> CheckboxlistClasses = WebContext.WebDriver.FindElements(ClassesCheckBox);

            int j = 0;

            foreach (IWebElement classElem in CheckboxlistClasses)
            {
                if (classElem.Displayed && NoOfClasses != 0)
                {
                    waiter.Until(ExpectedConditions.ElementToBeClickable(classElem));
                    classElem.Click();

                    j++;
                }
                if (j == NoOfClasses)
                    break;
            }

        }

        /// <summary>
        /// Selects a specific Class
        /// </summary>
        public void SelectClass(string Class)
        {
            waiter.Until(ExpectedConditions.TextToBePresentInElement(GroupHeader, "School Groups"));
            ReadOnlyCollection<IWebElement> ClassesList = WebContext.WebDriver.FindElements(ClassesCheckBox);
            foreach (IWebElement eachelement in ClassesList)
            {
                if (WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + eachelement.GetAttribute("id") + "']")).Text == Class)
                {
                    eachelement.Click();
                    break;
                }
            }
        }

        /// <summary>
        /// returns the list of selected groups 
        /// </summary>
        /// <param name="SelectGroup"></param>
        /// <returns></returns>
        public List<string> GetSelectedGroupOrGroupfilter(By SelectGroup)
        {
            List<string> selectedGroup = new List<string>();
            ReadOnlyCollection<IWebElement> CheckboxlistGroup = WebContext.WebDriver.FindElements(SelectGroup);

            foreach (IWebElement groupfilterElem in CheckboxlistGroup)
            {
                if (groupfilterElem.Selected)
                {
                    // isSelected = true;
                    selectedGroup.Add(WebContext.WebDriver.FindElement(By.CssSelector("label[for='" + groupfilterElem.GetAttribute("id") + "']")).Text);
                }
            }
            return selectedGroup;
        }

        /// <summary>
        /// Clicks the Next Button and moves on to the next page
        /// </summary>
        public MarksheetBuilder ClickDoneButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GroupDoneButton));
            GroupDoneButton.Click();
            while (true)
            {
                if (GroupDoneButton.GetAttribute("disabled") != "true")
                    break;
            }
            return new MarksheetBuilder();
        }

        /// <summary>
        /// Clicks the Back Button and moves on to the Marksheet Builder page
        /// </summary>
        public MarksheetBuilder ClickBackButton()
        {
            waiter.Until(ExpectedConditions.ElementToBeClickable(GroupBackButton));
            Thread.Sleep(2000);
            GroupBackButton.Click();
            waiter.Until(ExpectedConditions.TextToBePresentInElement(MarksheetBuilderHeader, "Marksheet Builder"));
            return new MarksheetBuilder();
        }

        ///// <summary>
        ///// Clicks the Close Button and moves on to the Marksheet Builder page
        ///// </summary>
        //public MarksheetBuilder ClickCloseButton()
        //{
        //    waiter.Until(ExpectedConditions.ElementToBeClickable(GroupCloseButton));
        //    GroupCloseButton.Click();
        //    return new MarksheetBuilder();
        //}
    }
}
