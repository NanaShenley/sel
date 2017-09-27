using NUnit.Framework;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using TestSettings;
using OpenQA.Selenium;
using System;
using SharedComponents.BaseFolder;
using Template.Components;
using Selene.Support.Attributes;

namespace ManageTemplate.Test
{
    public class CloneTemplate
    {
        #region Story 11811- Clone Template
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AbleToCloneTemplate()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Create a new template with invisible property. 
            string tName = "Able" + TemplateSearchScreen.RandomString(5);
            ScreenObj.CreateMessageTemplate(tName,"General", false);

            //Search for newly created template.
            TemplateScreenNavigation.CloseManageMessageTemplateTab();
            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);
          
            ScreenObj.CloneTemplateFromToolbar(tName, "General");
            ScreenObj.CheckForClonedTemplate("Copy of " + tName, "General");          
          
         
        }
        #endregion
    }
}
