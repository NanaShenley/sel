using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;
using POM.Components.Pupil;
using POM.Helper;
using Pupil.Components.Common;
using Selene.Support.Attributes;
using SeSugar.Automation;
using TestSettings;
using WebDriverRunner.webdriver;


namespace Pupil.BehaviourEventTest.Tests
{
    public class BehaviourEventTest
    {
        private const string BehaviourEventsFeature = "Behaviour Events";

        [WebDriverTest(Browsers = new[] { BrowserDefaults.Chrome },
        Groups = new[] { PupilTestGroups.BehaviourEvent.Page, PupilTestGroups.BehaviourEvent.AddNewBehaviourEvents, PupilTestGroups.Priority.Priority1 })]
        public void PupilConduct_AddNewConduct()
        {
            //Arrange
            //Arrange
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.TestUser, enabledFeatures: BehaviourEventsFeature);
            Wait.WaitForDocumentReady();

            // Navigate to Pupil Record
            AutomationSugar.NavigateMenu("Tasks", "Pupil Conduct", "Behaviour Events");
            Thread.Sleep(5);
            AutomationSugar.WaitFor("add_button");
            AutomationSugar.ClickOn("add_button");
            AutomationSugar.WaitForAjaxCompletion();

            // Verify data is saved Success
            Assert.AreEqual(true, CheckNewFormIsShown(), "Success behaviour form is display");
        }

        private bool CheckNewFormIsShown()
        {
            bool foundForm = false;
            //
            var element = SeleniumHelper.FindElements(SeleniumHelper.SelectByDataAutomationID("section_menu_Event Details"));

            if (element != null)
            {
                foundForm = true;
            }

            return foundForm;
        }

    }
}
