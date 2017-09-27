using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using WebDriverRunner.webdriver;

namespace Assessment.Components.PageObject
{
    public class MarksheetType: BaseSeleniumComponents
    {
        public MarksheetType()
        {
           PageFactory.InitElements(WebContext.WebDriver, this);
        }
    }
}
