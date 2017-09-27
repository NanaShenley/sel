using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Pupil.Dialogs;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class CareArrangementsDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-maintenance-container='PersonalEducationPlans']")]
        private IWebElement _PersonalEducationPlans;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_personal_education_plan_button']")]
        private IWebElement _addPersonalEducationPlan;

        [FindsBy(How = How.Name, Using = "CareAuthority.dropdownImitator")]
        private IWebElement _careAuthority;

        [FindsBy(How = How.Name, Using = "LivingArrangement.dropdownImitator")]
        private IWebElement _livingArrangement;

        [FindsBy(How = How.Name, Using = "StartDate")]
        private IWebElement _startDateField;

        [FindsBy(How = How.Name, Using = "EndDate")]
        private IWebElement _endDateField;

        public string StartDate
        {
            set
            {
                _startDateField.SetDateTime(value);
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
            }
            get
            {
                return _endDateField.GetDateTime();
            }
        }

        public string CareAuthority
        {
            set
            {
                _careAuthority.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _careAuthority.GetValue();
            }
        }

        public string LivingArrangement
        {
            set
            {
                _livingArrangement.EnterForDropDown(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
            }
            get
            {
                return _livingArrangement.GetValue();
            }
        }

        public GridComponent<PersonalEducationPlansRow> PersonalEducationPlans
        {
            get
            {
                GridComponent<PersonalEducationPlansRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PersonalEducationPlansRow>(By.CssSelector("[data-maintenance-container='PersonalEducationPlans']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class PersonalEducationPlansRow
        {
            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_personal_education_plan_contributor_button']")]
            private IWebElement _editPersonalEducationPlan;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateField;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _addNoteButton;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
            private IWebElement _noteTextArea;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
            private IWebElement _note;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _columnEnd;

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
                    Retry.Do(_addNoteButton.ClickByJS, 100, 20, _note.ClickByJS);
                    _noteTextArea.SetText(value);
                    _noteTextArea.SendKeys(Keys.Tab);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _note.GetValue();
                }
            }

            public PEPContributorsDialog OpenEditPersonalEducationPlanDialog()
            {
                _editPersonalEducationPlan.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

                return new PEPContributorsDialog();
            }
        }

        public PEPContributorsDialog OpenAddPersonalEducationPlanDialog()
        {
            _addPersonalEducationPlan.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));

            return new PEPContributorsDialog();
        }
    }
}
