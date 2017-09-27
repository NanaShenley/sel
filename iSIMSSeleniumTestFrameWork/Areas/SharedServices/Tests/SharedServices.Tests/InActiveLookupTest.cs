using OpenQA.Selenium;
using SharedServices.Components;
using System;
using System.Collections.Generic;
using TestSettings;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace SharedServices.Tests
{
    public class InActiveLookupTest
    {
        /// <summary>
        /// This test is disabled as it is no more required
        /// </summary>
        [WebDriverTest(Groups = new[] { "InActiveLookup" }, Browsers = new[] { BrowserDefaults.Chrome }, Enabled = false)]
        public void InActiveLookup()
        {
            //Login
            InActiveLookup inActiveLookup = new InActiveLookup();

            //Find Pupil Record
            inActiveLookup.FindPupilRecordMenu();
            inActiveLookup.SearchPupilAction();
            inActiveLookup.NavigateToFirstPupil();

            //Select DropDown
            inActiveLookup.waitforElement("Gender.dropdownImitator");
            IWebElement outPutvalue = WebContext.WebDriver.FindElement(By.Name("Gender.dropdownImitator"));
            string currencyValue = outPutvalue.GetAttribute("value");

            //Check Style
            string color = outPutvalue.GetCssValue("color");
            String bgColor = outPutvalue.GetCssValue("background-color");

            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("Value from textbox is: " + currencyValue + "\n color is " + color + "\n BgColor is " + bgColor);

            //Find Pupil Contact
            inActiveLookup.FindPupilContactMenu();
            inActiveLookup.SearchPupilContactAction();
            inActiveLookup.NavigateToFirstPupil();

            //Select DropDown
            inActiveLookup.waitforElement("Gender.dropdownImitator");
            IWebElement outPutvalue1 = WebContext.WebDriver.FindElement(By.Name("Gender.dropdownImitator"));
            string currencyValue1 = outPutvalue1.GetAttribute("value");

            //Check Style
            string color1 = outPutvalue1.GetCssValue("color");
            String bgColor1 = outPutvalue1.GetCssValue("background-color");

            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("Value from textbox is: " + currencyValue1 + "\n color is " + color1 + "\n BgColor is " + bgColor1);

            //DropDown Option 1
            IList<IWebElement> option = inActiveLookup.SelectDropDownlist();
            IWebElement firstOption = option[1];

            Console.WriteLine("\r\n\r\n");
            Console.WriteLine("Value from " + firstOption.GetAttribute("value"));
            Console.WriteLine("color " + firstOption.GetCssValue("color"));
            Console.WriteLine("background-color " + firstOption.GetCssValue("background-color"));

        }
    }
}
