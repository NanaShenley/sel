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
using System.Collections.ObjectModel;
using Selene.Support.Attributes;

namespace ManageTemplate.Test
{
    class DeleteTemplate
    {
        #region story 11812 Delete Message Template
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DeleteMessageTemplate()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObject = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Create a new template with invisible property. 
            string tName = "Delete" + TemplateSearchScreen.RandomString(5);
            ScreenObject.CreateMessageTemplate(tName, "General", true);

            TemplateScreenNavigation.CloseManageMessageTemplateTab();
            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);

            Assert.True(ScreenObject.DeleteTemplate(tName) == "No Records Found");

        }

        #endregion  

    }
}
