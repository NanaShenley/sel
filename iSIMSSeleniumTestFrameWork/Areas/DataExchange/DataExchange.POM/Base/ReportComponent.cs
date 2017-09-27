using DataExchange.POM.Helper;
using OpenQA.Selenium;
using WebDriverRunner.webdriver;

namespace DataExchange.POM.Base
{
    public class ReportComponent
    {
        public bool CheckReportExists(string reportId)
        {
            bool isExists = false;

            IWebElement element = WebContext.WebDriver.FindElementSafe(By.CssSelector(reportId));
            if (element.IsElementExists())
            {
                isExists = true;
            }

            return isExists;
        }

        public void ClickReport(string reportId)
        {
            IWebElement element = WebContext.WebDriver.FindElementSafe(By.Id(reportId));
            if (element.IsElementDisplayed())
            {
                element.Click();
            }
        }
    }
}
