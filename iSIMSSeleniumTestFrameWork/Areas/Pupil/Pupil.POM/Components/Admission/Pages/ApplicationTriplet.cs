using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using WebDriverRunner.webdriver;

namespace POM.Components.Admission
{
    public class ApplicationTriplet : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public ApplicationTriplet()
        {
            _searchCriteria = new ApplicationSearchPage(this);
        }

        #region Search

        private readonly ApplicationSearchPage _searchCriteria;

        public ApplicationSearchPage SearchCriteria
        {
            get { return _searchCriteria; }
        }

        public class ApplcationsSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")] private IWebElement _code;

            public string Code
            {
                get { return _code.Text; }
            }
        }

        #endregion

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_application_button']")] private
            IWebElement _addNewApplicationButton;

        #endregion

        #region Public methods

        /// <summary>
        /// Author: Huy.Vo
        /// Description: Init page
        /// </summary>
        /// <returns></returns>
        public AddNewApplicationDialog AddNewApplicationDialog()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("add_new_application_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new AddNewApplicationDialog();
        }

        /// <summary>
        /// Author: Huy.Vo
        /// Description: click Delete button to delete an existing Applicant
        /// </summary>

        public void SelectSearchTile(ApplcationsSearchResultTile applicationsTile)
        {
            if (applicationsTile != null)
            {
                applicationsTile.Click();
            }
        }

        public void ChangeToAdmit(ApplcationsSearchResultTile applicationsTile)
        {
            if (applicationsTile != null)
            {
                applicationsTile.Click();
                var applicationPage = new ApplicationPage();
                applicationPage.ApplicationStatus = "Admitted";
                applicationPage.ClickSave();
                // Continue confirm to change status
                var confirmChangeStatusDialog = new ConfirmRequiredChangeStatus();
                applicationPage = confirmChangeStatusDialog.ConfirmContinueChangeStatus();
                applicationPage.ClickSave();
            }
        }

        #endregion
    }

    public class ApplicationSearchPage : SearchCriteriaComponent<ApplicationTriplet.ApplcationsSearchResultTile>
    {
        public ApplicationSearchPage(BaseComponent parent) : base(parent)
        {
        }

        #region Search properties

        [FindsBy(How = How.Name, Using = "ApplicantName")] private IWebElement _searchByName;

        [FindsBy(How = How.Name, Using = "AdmissionGroup.dropdownImitator")] private IWebElement _searchByAdmissionGroup;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(1) input")] private IWebElement
            _appliedCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(2) input")] private IWebElement
            _offeredCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(3) input")] private IWebElement
            _acceptedCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(4) input")] private IWebElement
            _admittedCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(5) input")] private IWebElement
            _rejectedCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(6) input")] private IWebElement
            _reservedCheckbox;

        [FindsBy(How = How.CssSelector, Using = "div[class = 'checkboxlist'] :nth-child(7) input")] private IWebElement
            _witdDrawnCheckbox;

        public string SearchByName
        {
            get { return _searchByName.GetValue(); }
            set { _searchByName.SetText(value); }
        }

        public string SearchByAdmissionGroup
        {
            get { return _searchByAdmissionGroup.GetValue(); }
            set { _searchByAdmissionGroup.EnterForDropDown(value); }
        }

        public void SetStatus(string statusName, bool isCheck)
        {

            IList<IWebElement> listCheckboxLabel =
                SeleniumHelper.FindElements(
                    SimsBy.CssSelector("[data-automation-id = 'search_criteria'] .checkboxlist-column label"));
            IList<IWebElement> listCheckboxInput =
                SeleniumHelper.FindElements(
                    SimsBy.CssSelector("[data-automation-id = 'search_criteria'] .checkboxlist-column input"));
            IWebElement _checkbox;
            int index;

            foreach (var label in listCheckboxLabel)
            {

                if (label.GetText().Trim().Equals(statusName))
                {
                    index = listCheckboxLabel.IndexOf(label);
                    _checkbox = listCheckboxInput[index];
                    if (isCheck && !_checkbox.IsChecked() || !isCheck && _checkbox.IsChecked())
                    {
                        _checkbox.ClickByJS();
                    }

                }
            }
        }

        public bool Applied
        {
            set { _appliedCheckbox.Set(value); }
            get { return _appliedCheckbox.IsChecked(); }
        }

        public bool Offered
        {
            set { _offeredCheckbox.Set(value); }
            get { return _offeredCheckbox.IsChecked(); }
        }

        public bool Accepted
        {
            set { _acceptedCheckbox.Set(value); }
            get { return _acceptedCheckbox.IsChecked(); }
        }

        public bool Admitted
        {
            set { _admittedCheckbox.Set(value); }
            get { return _admittedCheckbox.IsChecked(); }
        }

        public bool Rejected
        {
            set { _rejectedCheckbox.Set(value); }
            get { return _rejectedCheckbox.IsChecked(); }
        }

        public bool Reserved
        {
            set { _reservedCheckbox.Set(value); }
            get { return _reservedCheckbox.IsChecked(); }
        }

        public bool WitdDrawn
        {
            set { _witdDrawnCheckbox.Set(value); }
            get { return _witdDrawnCheckbox.IsChecked(); }
        }

        #endregion

        public void DisplayIfHidden()
        {
            try
            {
                var filterButton = WebContext.WebDriver.FindElementSafe(By.ClassName("btn-show-panel"));

                filterButton.Click();
            }
            catch (Exception)
            {
                // Ignore - screen is not hiding filters
            }
            
            
        }
    }

}
