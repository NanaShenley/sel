using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;

namespace POM.Components.Communication
{
    public class AddNewAgentDialog :BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='dialog-detail']"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _foreNameTextbox;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _surNameTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_button']")]
        private IWebElement _continueButton;

        public string SurName
        {
            set
            {
                _surNameTextbox.SetText(value);
            }
        }

        public string ForeName
        {
            set
            {
                _foreNameTextbox.SetText(value);
            }
        }
        
        #endregion

        #region Actions

        public void ClickContinue()
        {
            _continueButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }



        #endregion
    }
}
