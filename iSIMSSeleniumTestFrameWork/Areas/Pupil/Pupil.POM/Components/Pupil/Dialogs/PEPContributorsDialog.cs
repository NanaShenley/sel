using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil.Dialogs
{
    public class PEPContributorsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-planContributors-palette-editor"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_plan_contributor_button']")]
        private IWebElement _addPepContributorContactButtton;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateField;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _noteTextArea;

        public string StartDate
        {
            set
            {
                _startDateField.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _startDateField.SendKeys(Keys.Tab);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _startDateField.GetDateTime();
            }
        }

        public string EndDate
        {
            set
            {
                _endDateField.SetDateTime(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                _startDateField.SendKeys(Keys.Tab);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _endDateField.GetDateTime();
            }
        }

        public string Notes
        {
            set
            {
                _noteTextArea.SetText(value);
                _noteTextArea.SendKeys(Keys.Tab);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _noteTextArea.GetValue();
            }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PersonalEducationPlanContributors']")]
        public GridComponent<PersonalEducationPlanContributorsRow> PersonalEducationPlanContributors
        {
            get
            {
                GridComponent<PersonalEducationPlanContributorsRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PersonalEducationPlanContributorsRow>(By.CssSelector("[data-maintenance-container='PersonalEducationPlanContributors']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class PersonalEducationPlanContributorsRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _name;

            [FindsBy(How = How.Name, Using = "PepContributorRole.dropdownImitator")]
            private IWebElement _relationshipDropDown;

            public string RelationshipDropDown
            {
                set { _relationshipDropDown.ChooseSelectorOption(value); }
                get { return _relationshipDropDown.GetValue(); }
            }
        }

        public SelectPeopleDialog OpenAddPepContributorContactDialog()
        {
            _addPepContributorContactButtton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectPeopleDialog();
        }
    }
}
