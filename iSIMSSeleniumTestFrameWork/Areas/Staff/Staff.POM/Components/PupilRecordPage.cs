using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;
using Staff.POM.Base;
using SeSugar.Automation;
using System;

namespace Staff.POM.Components.Staff
{
    public class PupilRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("pupil_record_detail"); }
        }

        public static PupilRecordPage LoadPupilDetail(Guid pupilId)
        {
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;
            string js = "sims_commander.OpenDetail(undefined, '/{0}/Pupils/SIMS8LearnerMaintenanceSimpleLearner/ReadDetail/{1}')";

            Retry.Do(() => { jsExecutor.ExecuteScript(string.Format(js, TestSettings.TestDefaults.Default.Path, pupilId)); });

            AutomationSugar.WaitForAjaxCompletion();

            return new PupilRecordPage();
        }

        public GridComponent<TelephoneNumberRow> TelephoneNumberTable
        {
            get
            {
                GridComponent<TelephoneNumberRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumberRow>(By.CssSelector("[data-maintenance-container='LearnerTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumberRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _telephoneNumber;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerTelephones'][name$='UseForTextMessages']")]
            private IWebElement _ams;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerTelephones'][name$='IsMainTelephone']")]
            private IWebElement _mainNumber;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _addNoteButton;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
            private IWebElement _note;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            public string TelephoneNumber
            {
                set
                {
                    _telephoneNumber.SetText(value);
                }
                get
                {
                    return _telephoneNumber.GetAttribute("value");
                }
            }

            public string Location
            {
                set
                {
                    _location.EnterForDropDown(value);
                }
                get { return _location.GetAttribute("value"); }
            }

            public bool AMS
            {
                set
                {
                    _ams.Set(value);
                }
                get { return _ams.IsChecked(); }
            }

            public bool MainNumber
            {
                set
                {
                    _mainNumber.Set(value);
                }
                get { return _mainNumber.IsChecked(); }
            }

            public string Note
            {
                set
                {
                    _addNoteButton.ClickByJS();
                    _note.SetText(value);
                }
                get
                {
                    _addNoteButton.ClickByJS();
                    string result = _note.GetText();
                    return result;
                }
            }
        }

        public GridComponent<EmailRow> EmailTable
        {
            get
            {
                GridComponent<EmailRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<EmailRow>(By.CssSelector("[data-maintenance-container='LearnerEmails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class EmailRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
            private IWebElement _emailAddress;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerEmails'][name$='UseForTextMessages']")]
            private IWebElement _ams;

            [FindsBy(How = How.CssSelector, Using = "[name*='LearnerEmails'][name$='IsMainEmail']")]
            private IWebElement _mainEmail;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _addNoteButton;

            [FindsBy(How = How.CssSelector, Using = "[name$='Notes']")]
            private IWebElement _note;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _blankCell;

            public string EmailAddress
            {
                set
                {
                    _emailAddress.SetText(value);
                }
                get
                {
                    return _emailAddress.GetAttribute("value");
                }
            }

            public string Location
            {
                set
                {
                    _location.EnterForDropDown(value);
                }
                get { return _location.GetAttribute("value"); }
            }

            public bool AMS
            {
                set
                {
                    _ams.Set(value);
                }
                get { return _ams.IsChecked(); }
            }

            public bool MainEmail
            {
                set
                {
                    _mainEmail.Set(value);
                }
                get { return _mainEmail.IsChecked(); }
            }

            public string Note
            {
                set
                {
                    _addNoteButton.ClickByJS();
                    _note.SetText(value);
                }
                get
                {
                    _addNoteButton.ClickByJS();
                    string result = _note.GetText();
                    return result;
                }
            }
        }

        public GridComponent<ContactRow> ContactTable
        {
            get
            {
                GridComponent<ContactRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ContactRow>(By.CssSelector("[data-maintenance-container='LearnerContactRelationships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ContactRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Priority']")]
            private IWebElement _priority;

            [FindsBy(How = How.CssSelector, Using = "[id$='LearnerContactRelationshipType_dropdownImitator']")]
            private IWebElement _relationShip;

            [FindsBy(How = How.CssSelector, Using = "[name$='HasParentalResponsibility']")]
            private IWebElement _parentalReponsibility;

            [FindsBy(How = How.CssSelector, Using = "[name$='LearnerContactRelationshipsLearnerContact']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _edit;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='remove_button']")]
            private IWebElement _remove;

            #region Properties

            public string Priority
            {
                get { return _priority.Text; }
                set
                {
                    Retry.Do(_priority.Click);
                    _priority.SetText(value);
                }
            }

            public string RelationShip
            {
                get { return _relationShip.GetAttribute("value"); }
                set
                {
                    _relationShip.ClickByJS();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    _relationShip.EnterForDropDown(value);
                }
            }

            public bool ParentalReponsibility
            {
                set { _parentalReponsibility.Set(value); }
                get { return _parentalReponsibility.IsCheckboxChecked(); }
            }

            public string Name
            {
                set { _name.SetText(value); }
                get { return _name.GetValue(); }
            }

            #endregion Properties

            #region Action

            public void Edit()
            {
                _edit.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }

            #endregion Action
        }

        #region Tab

        public void SelectPhoneEmailTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Phone/Email");
        }

        public void SelectFamilyHomeTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Family/Contacts");
        }

        #endregion Tab
    }
}