using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Facilities.Components.Common
{
    class FacilitiesCommonElements
    {
        public static readonly By Schemename = By.Name("SchemeName");
        public static readonly By Createbutton = By.CssSelector("[data-automation-id='create_button']");
    }
}
