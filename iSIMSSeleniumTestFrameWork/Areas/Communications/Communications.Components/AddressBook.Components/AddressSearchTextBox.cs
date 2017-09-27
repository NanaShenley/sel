using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;
using System.Windows;
using System.Windows.Forms;
using NUnit.Framework;
using System.Windows.Controls;
using SharedComponents.Helpers;
using OpenQA.Selenium.Interactions;
using AddressBook.Components.Pages;
using SharedComponents.HomePages;
using AddressBook.Test;
namespace AddressBook.Components
{
    public class AddressSearchTextBox
    {
        private const string cssforTextSearch = "shell_global_search_input";
        [FindsBy(How = How.Id, Using = cssforTextSearch)]
        public IWebElement textSearch;

        public AddressSearchTextBox()
        {
            // Initiate elements POM
            PageFactory.InitElements(WebContext.WebDriver, this);
        }
    }
}
