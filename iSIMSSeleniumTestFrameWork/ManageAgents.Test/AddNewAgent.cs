using NUnit.Framework;
using SharedComponents.Helpers;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;
using SharedComponents.HomePages;
using Agents.Components;
using TestSettings;
using OpenQA.Selenium;
using Agents.Components.Utils;
using System;
using SharedComponents.BaseFolder;
using Selene.Support.Attributes;

namespace ManageAgents.Test
{
    public class AddNewAgent
    {

        #region Story 3257- Validations on keeping Forename-Surname empty, surname empty, forename empty, max character input for forename and surname.
        [WebDriverTest(Enabled = true, Groups = new[] { "Agents","All","P2" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void CheckValidationOnKeepingSurNameForeNameEmpty()
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();           
            BaseSeleniumComponents.WaitUntilDisplayed(AgentElements.MainScreen.AgentForename);

            Assert.True(ScreenObject.CheckValidation("","")    &&  // for empty forename and surname.
                        ScreenObject.CheckValidation("p", "")  &&  // for empty surname. 
                        ScreenObject.CheckValidation("", "P")  &&  // for empty forename  
                        Int32.Parse(dialogObj.AgentForeName.GetAttribute("maxlength")) == 100 &&   // For forename maxlength
                        Int32.Parse(dialogObj.AgentSurName.GetAttribute("maxlength")) == 100);     // For surname maxlength
        }
        #endregion

        #region Story 3257- Dialog disappear on Cancel Click
        [WebDriverTest(Enabled = true, Groups = new[] { "Agents" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DialogAgentDisappearsOnCancel()
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            Assert.True(ScreenObject.DialogDisappeared());
        }
        #endregion


        //Screen for service selection is removed as per new implementation. - 26/10/16 - Shridhar. 
        #region Story 3257- Does the name of Agent appears in the Second dialog (after continue?) and On pressing back keys does the mandatory fields retain their values?
        [WebDriverTest(Enabled = false, Groups = new[] { "Agents" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void AgentNameAppendAndBackkeyFunctionality()
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();
            string forename = AddNewAgentDialog.RandomString(10);
            string surname = AddNewAgentDialog.RandomString(10);
            
            bool assert1 = dialogObj.IsNameAppended(forename, surname);
            bool assert2 = dialogObj.BackKeyFunctioningProperly(forename, surname);

            Assert.True(assert1 && assert2);
        }
        #endregion
        

        //Test case has removed as Service provided is not a mandatory field as per new implementation. - 26/10/16 - Shridhar. 
        #region Story 3257-Does we get validation on not checking any services?
        [WebDriverTest(Enabled = false, Groups = new[] { "Agents" }, Browsers = new[] { BrowserDefaults.Chrome })]
        public void DoesValidationMessageAppearsWithNoService()
        {
            AgentSearchScreen ScreenObject = AgentScreenNavigation.NavigateToAgentMenuPage();
            AddNewAgentDialog dialogObj = ScreenObject.ClickAddNewAgent();
            AgentServicesProvided obj = dialogObj.EnterNames(AddNewAgentDialog.RandomString(10), AddNewAgentDialog.RandomString(10));
            obj.CreateButtonClick();
            Assert.True(obj.IsAlertMessageAppearingOnKeepingServiceEmpty());
        }
        #endregion

    }
}
