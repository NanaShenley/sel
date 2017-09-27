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
    public class AddNewTemplate
    {
        #region Story 10638 Navigate to the Manage Templates screen
        //Can I navigate to the Manage Templates screen from the Task Menu?
        //Can I see Create button, but cant access save and cancel buttons. 
        [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CanNavigateToManageTemplateScreen()
        {
            //TemplateSearchScreen ScreenObject = TemplateScreenNavigation.NavigateToTemplateMenuPage();
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObject = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            bool assert1 = ScreenObject.IsCreateNewTemplateDisplayed();
            bool assert2 = !(ScreenObject.IsCancelTemplateDisplayed());
            bool assert3 = !(ScreenObject.IsSaveTemplateDisplayed());

            Assert.True(assert1 && assert2 && assert3);
        }
        #endregion  

        #region Story 10640 Create New General Message Template
        /*
            Can I enter a Template Name?
            Is the Template Name mandatory?
            Can I select the Type of template?
            Is the Type of template mandatory?
            Can I define whether the template is visible? 
        */
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ValCreateNewGeneralMsgTemplate()
        {
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Can i enter Template Name?
            bool assert1 = ScreenObj.IsTemplateNameAccessible();

            //Is the template name mandatory
            bool assert2 = ScreenObj.IsTemplateNameMandatory();

            //is the type of template mandatory.
            //bool assert3 = ScreenObj.IsTemplateTypeMandatory();

            //Can i select the type of template
            bool assert4 = ScreenObj.IsTemplateTypeSelectable("Parental Reporting");

            //Can i check whether the template is visible. 
            bool assert5 = ScreenObj.IsTemplateMarkedActive();
            
            Assert.True(assert1 && assert2 &&  assert4 && assert5);
        }
        #endregion

        #region Story 14732 Is the combination of Template Name and Template Purpose unique? (Same name and Same Purpose)
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome})]
        public void ValUniqueTemplateNameAndPurpose()
        {
            //TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPage();
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Create a new template first.
            string tName = "ValU" + TemplateSearchScreen.RandomString(5);
            ScreenObj.CreateMessageTemplate(tName, "General");

            //Try and make new template with same name.
            TemplateScreenNavigation.CloseManageMessageTemplateTab();

            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);
            bool assertVal = ScreenObj.CreateMessageTemplate(tName, "General");

            POM.Helper.SeleniumHelper.ClickByJS(ScreenObj.cancelTemplate);
            BaseSeleniumComponents.WaitUntilDisplayed(By.CssSelector(POM.Helper.SeleniumHelper.AutomationId("ignore_commit_dialog")));
            System.Threading.Thread.Sleep(2000);
            POM.Helper.SeleniumHelper.ClickByJS(ScreenObj.cancelTemplateDontSave);
            ScreenObj.DeleteTemplate(tName);
            Assert.False(assertVal);

        }
        #endregion  

        #region Story 14732 Is the combination of Template Name and Template Purpose unique? (Same name and different Purpose)
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValSameTemplateNameAndUniquePurpose()
        {
            //TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPage();
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Create a new template first.
            string tName = "ValS" + TemplateSearchScreen.RandomString(5);
            ScreenObj.CreateMessageTemplate(tName, "General");

            //Try and make new template with same name.
            TemplateScreenNavigation.CloseManageMessageTemplateTab();

            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);
            bool assertVal = ScreenObj.CreateMessageTemplate(tName, "Parental Reporting");

            //ScreenObj.DeleteTemplate(tName);
            
            Assert.True(assertVal);
        }
        #endregion  

        #region Story 14732 Check for template marked as invisible in search results. It should not appear in results when "Include inactive template" checkbox is unchecked
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValInvisibleTemplateInSearchResultsExcludeInactive()
        {
            //TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPage();
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);

            //Create a new template with invisible property. 
            string tName = "ValI" + TemplateSearchScreen.RandomString(5);
            ScreenObj.CreateMessageTemplate(tName, "General", false);

            //Search for newly created template.
            TemplateScreenNavigation.CloseManageMessageTemplateTab();
            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);

            bool assertVal = ScreenObj.CheckResultsInTemplateSearch(tName) == "No Records Found";
            ScreenObj.DeleteTemplate(tName,true);

            Assert.True(assertVal);             

        }
        #endregion

        #region Story 14732 Check for template marked as invisible in search results. It should appear in results when "Include inactive template" checkbox is checked
          [NotDone]
        [WebDriverTest(Enabled = true, Groups = new[] { "Templates" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void ValInvisibleTemplateInSearchResultsIncludeInactive()
        {
            //TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPage();
            String[] featureList = { "SendMessage", "ManageTemplate" };
            TemplateSearchScreen ScreenObj = TemplateScreenNavigation.NavigateToTemplateMenuPageFeatureBee(featureList);


            //Create a new template with invisible property. 
            string tName = "ValInvi" + TemplateSearchScreen.RandomString(5);
            ScreenObj.CreateMessageTemplate(tName, "General", false);

            //Search for newly created template.
            TemplateScreenNavigation.CloseManageMessageTemplateTab();
            TemplateScreenNavigation.NavigateToTemplateMenuPage(false);

            bool assertVal = ScreenObj.CheckResultsInTemplateSearch(tName,true) == "Record Found";
            
            //ScreenObj.DeleteTemplate(tName, true);
            Assert.True(assertVal);             

        }
        #endregion

    }
}
