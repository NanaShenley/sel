using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Components
{
    public class TemplateElements
    {
        public struct SearchPanel
        {
            public static readonly By SearchButton = By.CssSelector("[data-automation-id='search_criteria_submit']");
            //public static readonly By templateName = By.CssSelector("[data-automation-id='search_criteria_submit']");
            //public static readonly By templateType = By.CssSelector("[data-automation-id='search_criteria_submit']");
        }

        public struct MainScreen
        {
            public static readonly By createButton = By.CssSelector("[data-automation-id='add_button']");
            public static readonly By saveButton = By.CssSelector("[data-automation-id='well_know_action_save']");
            public static readonly By cancelButton = By.CssSelector("[data-automation-id='well_know_action_Cancel']");
            public static readonly By TemplateName = By.CssSelector("input[name='NameOfTemplate']"); 
        }
    }
}