using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using TestSettings;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class ColumnDetails : BaseSeleniumComponents
    {
        public const string ColumndetailsPopoverSelector = "[data-show-column-details-popover='']";
        [FindsBy(How = How.CssSelector, Using = ColumndetailsPopoverSelector)]
        public IWebElement _columnDetailsActionElement;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ColumnDetailsPopover']")]
        public IWebElement _columnDetailsPopover;

        public const string ColumndetailsPopoverCloseSelector = "[data-close-column-details-popover='']";
        [FindsBy(How = How.CssSelector, Using = ColumndetailsPopoverCloseSelector)]
        public IWebElement _ColumnDetailsPopoverCloseElement;

        public const string ColumnDetailsPopoverElementItemTitle =  "[class='popover-card-list-title']";
        public const string ColumnDetailsPopoverElementItem = "[class='popover-card-list-item']";
        public const string ColumnDetailsPopoverButtonDescMore = "[class='read-more-expand']";
        public const string ColumnDetailsPopoverButtonDesLess = "[class='read-more-collapse']";
        public const string ColumnDetailsPopoverElementDescMore = "[class='read-more-content']"; 
        

        public ColumnDetails(SeleniumHelper.iSIMSUserType userType = SeleniumHelper.iSIMSUserType.AssessmentCoordinator)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);

            SeleniumHelper.Login(userType);
            //SeleniumHelper.NavigateMenu("Tasks", "Assessment", "Marksheets");
            CommonFunctions.GotToMarksheetMenu();
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public ColumnDetails OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            return this;
        }

        public bool OpenHeaderMenu(string columnHeader)
        {
            string openHeaderId = "header_menu_" + columnHeader;
            if (
                WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id ='" + openHeaderId + "']"))
                    .Displayed)
            {
                WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id ='" + openHeaderId + "']"));
                return true;
            }
            else
                return false;
        }

        public string GetHearderMenuId(string columnHeader)
        {
            string openHeaderId = "header_menu_" + columnHeader;
            return "[data-automation-id ='" + openHeaderId + "']";
        }

        public void OpenColumnDetailsPopover(string columnHeader)
        {
            if (OpenHeaderMenu(columnHeader))
            {
                WaitForElement(By.CssSelector(ColumndetailsPopoverSelector));
                if (WebContext.WebDriver.FindElements(By.CssSelector(ColumndetailsPopoverSelector))
                    .ToList()
                    .Any(ele => ele.Displayed))
                    WebContext.WebDriver.FindElements(By.CssSelector(ColumndetailsPopoverSelector))
                        .ToList()
                        .FirstOrDefault(ele => ele.Displayed)
                        .Click();
            }
        }

        public void ClosePopover()
        {
            WaitForElement(By.CssSelector(ColumndetailsPopoverCloseSelector));
            WebContext.WebDriver.FindElements(By.CssSelector(ColumndetailsPopoverCloseSelector)).FirstOrDefault(col=>col.Displayed).Click();
        }

        public bool IsPopoverOpen(string columnHeader)
        {
            WaitForElement(By.CssSelector("[class='popover-title']"));
            IWebElement popoverTitle = WebContext.WebDriver.FindElements(By.CssSelector("[class='popover-title']")).FirstOrDefault(pop=>pop.Displayed);
            return popoverTitle.Text == columnHeader;
        }

        public string PopoverShortDescription(string columnHeader)
        {
            WaitForElement(By.CssSelector("[class='popover-title']"));
            ReadOnlyCollection<IWebElement> popoverTitleTitles = WebContext.WebDriver.FindElements(By.CssSelector(ColumnDetailsPopoverElementItemTitle));
            ReadOnlyCollection<IWebElement> popoverTitleItems = WebContext.WebDriver.FindElements(By.CssSelector(ColumnDetailsPopoverElementItem));
            for (int i = 0; i < popoverTitleTitles.Count; i++ )
            {
                if (popoverTitleTitles[i].Text == "Description")
                {
                    return popoverTitleItems[i].Text;
                }
            }
            return string.Empty;
        }

        public string PopoverLongDescription(string columnHeader)
        {
            WaitForElement(By.CssSelector("[class='popover-title']"));
            ReadOnlyCollection<IWebElement> popoverTitleTitles = WebContext.WebDriver.FindElements(By.CssSelector(ColumnDetailsPopoverElementItemTitle));
            ReadOnlyCollection<IWebElement> popoverTitleItems = WebContext.WebDriver.FindElements(By.CssSelector(ColumnDetailsPopoverElementItem));
            int i = 0;
            string moreContent = string.Empty;
            IWebElement lessButton = null;
            for (i = 0; i < popoverTitleTitles.Count; i++)
            {
                if (popoverTitleTitles[i].Text == "Description")
                {
                    IWebElement moreButton = popoverTitleItems[i].FindElement(By.CssSelector(ColumnDetailsPopoverButtonDescMore));
                    moreButton.Click();
                    //Thread.Sleep(2000);
                    IWebElement moreContentElement = popoverTitleItems[i].FindElement(By.CssSelector(ColumnDetailsPopoverElementDescMore));
                    moreContent = moreContentElement.Text;
                    lessButton = popoverTitleItems[i].FindElement(By.CssSelector(ColumnDetailsPopoverButtonDesLess));
                    lessButton.Click();                   
                }
            }
            //popoverTitleItems = WebContext.WebDriver.FindElements(By.CssSelector(ColumnDetailsPopoverElementItem));
            //lessButton = popoverTitleItems[i].FindElement(By.CssSelector(ColumnDetailsPopoverButtonDesLess));
            //moreButton.Click();
            return moreContent;
        }
    }
}
