using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace POM.Components.Pupil
{
    public class LinkedPeopleAndOrganisationPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Page Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_agent_button']")]
        private IWebElement _addButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _messageSuccess;

        #endregion

        #region Actions

        public void AddAgent()
        {
            _addButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public bool IsMessageSuccessAppear()
        {
            return _messageSuccess.IsElementExists();
        }

        #endregion

        #region Linked Organisations Grid

        public GridComponent<LinkedOrganisation> LinkedOrganisationGrid
        {
            get
            {
                GridComponent<LinkedOrganisation> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<LinkedOrganisation>(By.CssSelector("[data-maintenance-container='LearnerAgentServiceRelationships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class LinkedOrganisation
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _name;

            public string Name
            {
                set
                {
                    _name.SetText(value);
                }
                get
                {
                    return _name.GetValue();
                }
            }

        }
        #endregion
    }
}
