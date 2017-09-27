using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using SeSugar.Automation;
using Retry = POM.Helper.Retry;
using SimsBy = POM.Helper.SimsBy;

namespace POM.Components.Pupil
{
    public class StaffRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_detail"); }
        }

        public static StaffRecordPage LoadStaffDetail(Guid staffId)
        {
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;
            string js = "sims_commander.OpenDetail(undefined, '/{0}/Staff/SIMS8StaffMaintenanceTripleStaff/ReadDetail/{1}')";

            Retry.Do(() => { jsExecutor.ExecuteScript(string.Format(js, TestSettings.TestDefaults.Default.Path, staffId)); });

            AutomationSugar.WaitForAjaxCompletion();

            return new StaffRecordPage();
        }

        #region Tab Action

        public void SelectPhoneEmailTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Phone/Email");
        }

        public void SelectNextOfKinTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Contacts");
        }

        #endregion

        #region TelephoneNumber Grid

        public GridComponent<TelephoneNumberRow> TelephoneNumberTable
        {
            get
            {
                GridComponent<TelephoneNumberRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TelephoneNumberRow>(By.CssSelector("[data-maintenance-container='StaffTelephones']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class TelephoneNumberRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='TelephoneNumber']")]
            private IWebElement _telephoneNumber;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _endCol;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='StaffTelephones'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='StaffTelephones'][name$='IsMainTelephone']")]
            private IWebElement _mainNumber;

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
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _location.GetAttribute("value"); }
            }

            public bool AMS
            {
                set
                {
                    _aMS.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _aMS.IsChecked(); }
            }

            public bool MainNumber
            {
                set
                {
                    _mainNumber.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _mainNumber.IsChecked(); }
            }

        }

        #endregion

        #region Email Grid

        public GridComponent<EmailRow> EmailTable
        {
            get
            {
                GridComponent<EmailRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<EmailRow>(By.CssSelector("[data-maintenance-container='StaffEmails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class EmailRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EmailAddress']")]
            private IWebElement _emailAddress;

            [FindsBy(How = How.CssSelector, Using = ".grid-col-end")]
            private IWebElement _endCol;

            [FindsBy(How = How.CssSelector, Using = "[name$='LocationType.dropdownImitator']")]
            private IWebElement _location;

            [FindsBy(How = How.CssSelector, Using = "[name*='StaffEmails'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='StaffEmails'][name$='IsMainEmail']")]
            private IWebElement _mainEmail;

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
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _location.GetAttribute("value");
                }
            }

            public bool AMS
            {
                set
                {
                    _aMS.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _aMS.IsChecked();
                }
            }

            public bool MainEmail
            {
                set
                {
                    _mainEmail.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _mainEmail.IsChecked();
                }
            }

        }

        #endregion
       
        #region Contact Grid

        public GridComponent<ContactRow> ContactTable
        {
            get
            {
                GridComponent<ContactRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ContactRow>(By.CssSelector("[data-maintenance-container='StaffContactRelationships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ContactRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StaffContactRelationshipsStaffContact']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsNextOfKin']")]
            private IWebElement _nextOfKin;

            [FindsBy(How = How.CssSelector, Using = "[name$='StaffContactRelationshipType.dropdownImitator']")]
            private IWebElement _relationShip;

            [FindsBy(How = How.CssSelector, Using = "[name$='MainNumber']")]
            private IWebElement _mainTelephone;

            public string Name
            {
                get { return _name.GetAttribute("value"); }
            }

            public bool NextOfKin
            {
                set { _nextOfKin.Set(value); }
                get { return _nextOfKin.IsChecked(); }
            }

            public string RelationShip
            {
                set { _relationShip.EnterForDropDown(value); }
                get { return _relationShip.GetAttribute("value"); }
            }

            public string MainTelephone
            {
                get { return _mainTelephone.GetAttribute("value"); }
            }
        }

        #endregion
    }
}