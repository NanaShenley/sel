using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staff.POM.Base;
using Staff.POM.Helper;
using System.Threading;
using SeSugar.Automation;
using Staff.POM.Components.Staff.Dialogs;
using WebDriverRunner.webdriver;

namespace Staff.POM.Components.Staff
{
    public class SalaryRangeHistoryPopup
    {
        private List<SalaryHistoryRecord> _salaryHistoryRecords;

        public SalaryRangeHistoryPopup()
        {
            Initialise();
        }

        private void Initialise()
        {
            PageFactory.InitElements(this, new ElementLocator(DialogIdentifier));

            _salaryHistoryRecords = new List<SalaryHistoryRecord>();

            IWebElement popover = WebContext.WebDriver.FindElement(DialogIdentifier);

            var salaryHistoryRecords = popover.FindElements(By.CssSelector(".contact-card")).Select(x => x.GetAttribute("data-row-id")).ToList();

            foreach (var salaryHistoryRecord in salaryHistoryRecords)
            {
                var panel = popover.FindElement(By.CssSelector("[data-row-id='" + salaryHistoryRecord + "']"));

                var row = new SalaryHistoryRecord();

                PageFactory.InitElements(row, new ElementLocator(panel));

                _salaryHistoryRecords.Add(row);
            }

        }

        public By DialogIdentifier
        {
            get { return SimsBy.AutomationId("popover-custom-id"); }
        }

        #region fields
        
        [FindsBy(How = How.CssSelector, Using = "[class='popover-title']")]
        private IWebElement _title;

      
        [FindsBy(How = How.CssSelector, Using = "[class='btn-close']")]
        private IWebElement _close;

        #endregion

        public string Title
        {
            get { return _title.GetText(); }
        }

        public void ClickClose()
        {
            _close.Click();
        }

        public List<SalaryHistoryRecord> SalaryHistoryRecords
        {
            get
            {
                return _salaryHistoryRecords;
            }
        }

        public class SalaryHistoryRecord
        {
            public SalaryHistoryRecord()
            {
            }

            [FindsBy(How = How.CssSelector, Using = "[data-list-item-name='date'] dd")]
            private IWebElement _date;

            [FindsBy(How = How.CssSelector, Using = "[data-list-item-name='name'] dd")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[data-list-item-name='salary'] dd")]
            private IWebElement _salary;

            public string Date
            {
                get
                {
                    return _date.GetText();
                }
            }

            public string Name
            {
                get
                {
                    return _name.GetText();
                }
            }

            public string Salary
            {
                get
                {
                    return _salary.GetText();
                }
            }
        }
    }
}