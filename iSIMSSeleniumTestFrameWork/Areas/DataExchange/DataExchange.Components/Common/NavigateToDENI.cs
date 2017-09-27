using System;
using System.Threading;
using OpenQA.Selenium;
using SharedComponents.CRUD;
using SharedComponents.Helpers;
using TestSettings;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using SharedComponents.BaseFolder;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataExchange.Components.Common
{
    /// <summary>
    /// This class is responsible for Creating DENI and Searching DENI in results.
    /// </summary>
    public class NavigateToDENI
    {
        public void NavigateToDENIScreen()
        {
            //Login to the application
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.ReturnsManager,false);

            SeleniumHelper.NavigateMenu("Tasks", "Statutory Return", "Manage Statutory Returns");       
        }           

        /// <summary>
        /// validate if search results exist
        /// </summary>
        /// <returns></returns>
        public bool HasResults()
        {            
            return WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='search_results_counter']")).Text != "No Matches";
        }

        /// <summary>
        /// Create DENI 
        /// </summary>
        public void CreateDENIReturn()
        {
            SearchDENI();
            if (SearchResults.HasResults() && SearchResults.GetSearchResults().Count >0)
            {
                SearchResults.SelectSearchResult(0);
                return;
            }
            BaseSeleniumComponents.WaitUntilDisplayed(DataExchangeElement.CreateButton);
            SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElement.CreateButton);

            IWebElement dialogSelector = SeleniumHelper.Get(DataExchangeElement.DialogSelector);
            BaseSeleniumComponents.WaitUntilDisplayed(DataExchangeElement.VersionSelector);
            dialogSelector.ChooseSelectorOption(DataExchangeElement.VersionSelector, DataExchangeElement.DENIVersion);
            SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElement.CreateReturnOkButton);
            Thread.Sleep(1000);
            SearchDENI();
        }

        /// <summary>
        /// Search DENI
        /// </summary>
       public void SearchDENI()
       {
           SearchCriteria.SetCriteria(DataExchangeElement.SearchCriteriaStatutoryVersion, DataExchangeElement.DENIVersion);
           SearchCriteria.Search();
           Thread.Sleep(500);
           //SearchResults.WaitForResults();
       }

        /// <summary>
        /// Navigate to Detail Reports
        /// </summary>
       public void NavigateToDetailReportsDeni()
       {
           BaseSeleniumComponents.WaitUntilDisplayed(BrowserDefaults.TimeOut, DataExchangeElement.DetailReportsButton);
           SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElement.DetailReportsButton);

           BaseSeleniumComponents.WaitUntilDisplayed(BrowserDefaults.TimeOut, DataExchangeElement.IssuesAndQueriesReport);
           SeleniumHelper.WaitForElementClickableThenClick(DataExchangeElement.IssuesAndQueriesReport);
           WebContext.Screenshot();
       }

       /// <summary>
       /// Read the notification message header text
       /// </summary>
       /// <returns></returns>
       private string readAndReturnNotificationMessageText()
       {
           string message = BaseSeleniumComponents.WaitForAndGet(DataExchangeElement.NotificationAlert).FindChild(By.CssSelector("span[class=\"search-result h1-result\"]")).Text;
           WebContext.Screenshot();
           return message;
       }

       /// <summary>
       /// Au: Logigear
       /// Des: Scroll Right the specific cell in table grid until it is visible
       /// Only use with custom table (Webx DataTable) in Capita
       /// </summary>
       /// <returns></returns>
       public static void ScrollRightUntilExist(IWebElement tableElement, By element, bool containMultipleWebix = false)
       {
           bool isElementPresent = IsElementExists(element);
           int currentPosition = 0;
           int stepScroll = 100;
          
           string scripCommand;
           IWebElement scrollElement = null;
           if (containMultipleWebix)
           {
               scripCommand = "$$(arguments[0]).scrollTo({0},{1})";
               scrollElement = tableElement.FindElement(By.CssSelector(".webix_ss_hscroll.webix_vscroll_x"));
           }
           else
           {
               scripCommand = "$$(document.getElementsByClassName(\"webix_ss_hscroll webix_vscroll_x\")[0]).scrollTo({0},{1})";
           }

           while (isElementPresent == false)
           {
               if (containMultipleWebix)
               {
                   ((IJavaScriptExecutor)WebContext.WebDriver).ExecuteScript(String.Format(scripCommand, stepScroll, currentPosition), scrollElement);
               }
               else
               {
                   ExecuteJavascript(String.Format(scripCommand, stepScroll, currentPosition));
               }
               //currentPosition += 100;
               stepScroll += 100;

               isElementPresent = IsElementExists(element);
           }
       }

       public static Boolean IsElementExists(By element)
       {
           try
           {
               if (WebContext.WebDriver.FindElement(element) != null)
                   return true;
               else
                   return false;
           }
           catch (Exception)
           {
               return false;
           }
       }

       public static string ExecuteJavascript(string commandScript)
       {
           try
           {
               IWebDriver webDriver = WebContext.WebDriver;
               IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

               var result = js.ExecuteScript(commandScript);
               if (result != null)
                   return result.ToString();
               return "";
           }
           catch (Exception)
           {
               return "";
           }
       }

    }
}
