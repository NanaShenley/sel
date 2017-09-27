using System;
using System.Collections.ObjectModel;
using System.Linq;
using OpenQA.Selenium;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;
using System.Threading;

namespace SharedComponents
{
    public class DatePicker :BaseSeleniumComponents
    {
        public DatePicker()
        {
            WaitForElement(By.CssSelector("span[class='input-group-addon datepicker-picker']"));
        }

        public IWebElement SelectToday
        {
            get
            {
                return WebContext.WebDriver.FindElement(By.CssSelector("[class='day active today']")); 
                
            }
        }

        private IWebElement SelectDay(string day)
        {
            
            ReadOnlyCollection<IWebElement> daypickers = WebContext.WebDriver.FindElements(By.CssSelector("[class='day']"));
            return daypickers.First(daypicker => daypicker.Text == day);
        }

        private IWebElement SelectMonth(string month)
        {
            ReadOnlyCollection<IWebElement> monthpickers = WebContext.WebDriver.FindElements(By.CssSelector("[class='month']"));
            return monthpickers.First(monthpicker => monthpicker.Text == month);
        }

        public void SelectDate(string day, string month, string year)
        {
            char[] myChar = { '0'};
            day = day.TrimStart(myChar);
            SelectYear(year).Click();
            SelectMonth(month).Click();
            SelectDay(day).Click();
            Thread.Sleep(2000);
        }

        private IWebElement DatePickerButton
        {
            get
            {
                return WebContext.WebDriver.FindElement(By.CssSelector("span[class='input-group-addon datepicker-picker']"));
            }
        }


        public DatePicker OpenDatePicker()
        {
            DatePickerButton.Click();
            return new DatePicker();
        }
        

        private IWebElement SelectYear(string yearToFind)
        {
            string currentYear = DateTime.Now.ToString("yyyy");
            string currentMonthSelector = string.Format("{0} {1}", DateTime.Now.ToString("MMMM"), currentYear);
            const string classPickerSwitch = "[class='picker-switch']";
            ReadOnlyCollection<IWebElement> pickerswitches = WebContext.WebDriver.FindElements(By.CssSelector(classPickerSwitch));
            pickerswitches.First(pickerswitch => pickerswitch.Text == currentMonthSelector).Click();
            ReadOnlyCollection<IWebElement> pickerswitches2 = WebContext.WebDriver.FindElements(By.CssSelector(classPickerSwitch));
            pickerswitches2.First(pickerswitch => pickerswitch.Text == currentYear).Click();
            string cssSelectorToFind = yearToFind == currentYear ? "[class='year active']" : "[class='year']";
            ReadOnlyCollection<IWebElement> years = WebContext.WebDriver.FindElements(By.CssSelector(cssSelectorToFind));
            return years.First(year => year.Text == yearToFind);
            //return WebContext.WebDriver.FindElement(By.XPath("//span[contains(.,'" + year + "')]"));
        }
    }
}