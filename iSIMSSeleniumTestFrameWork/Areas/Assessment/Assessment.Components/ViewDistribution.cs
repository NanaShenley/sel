using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebDriverRunner.webdriver;

namespace Assessment.Components
{
    public class ViewDistribution : BaseSeleniumComponents
    {
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Export')]")]
        private IWebElement _exportTable;

        [FindsBy(How = How.XPath, Using = "//a[contains(.,'To CSV')]")]
        private IWebElement _exportTableToCSV;

        [FindsBy(How = How.XPath, Using = "//a[contains(.,'To Excel (xlsx)')]")]
        private IWebElement _exportTableToExcel;


        public ViewDistribution()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }        

        public ViewDistribution ClickExportDistributionTable()
        {
            _exportTable.Click();
            return this;
        }

        public void ClickExportDistributionImage()
        {
            //return true;
        }

        public ViewDistribution ClickExportDistributionTableAsCSV()
        {
            _exportTableToCSV.Click();
            return this;
        }

        public ViewDistribution ClickExportDistributionTableAsExcel()
        {
            _exportTableToExcel.Click();
            return this; 
        }
    }
}
