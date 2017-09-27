using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using System;

namespace POM.Components.SchoolGroups
{
    public class TeachingGroupPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        #region Properties
        [FindsBy(How = How.Name, Using = "EffectiveDate")]
        private IWebElement _effectDate;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='select_effective_date_range_button']")]
        private IWebElement _selectEffectDateRange;

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _fullName;

        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _shortName;

        [FindsBy(How = How.Name, Using = "AssessmentSubject.dropdownImitator")]
        private IWebElement _subjectDropdown;

        [FindsBy(How = How.Id, Using = "tri_chkbox_IsVisible")]
        private IWebElement _visibility;

        [FindsBy(How = How.Name, Using = "Notes")]
        private IWebElement _notes;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_pupils_button']")]
        private IWebElement _addPupilButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_supervisors_button']")]
        private IWebElement _addSupervisor;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Supervisors']")]
        private IWebElement _sectionSupervisor;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Group Details']")]
        private IWebElement _sectionGroupDetail;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Pupils']")]
        private IWebElement _sectionPupil;

        public string EffectiveDate
        {
            set { _effectDate.SetText(value); }
            get
            {
                string effectiveDateRange = _effectDate.GetValue();
                string startDate = effectiveDateRange.Split('-')[0].Trim();
                string endDate = effectiveDateRange.Split('-')[1].Trim();
                string dateFormat = "M/d/yyyy";
                if (SeleniumHelper.GetBrowserName().Contains("chrome"))
                {
                    dateFormat = "d/M/yyyy";
                }
                return String.Format("{0} - {1}", SeleniumHelper.Format(startDate, dateFormat), SeleniumHelper.Format(endDate, dateFormat));
            }
        }

        public string FullName
        {
            set { _fullName.SetText(value); }
            get { return _fullName.GetValue(); }
        }

        public string ShortName
        {
            set { _shortName.SetText(value); }
            get { return _shortName.GetValue(); }
        }

        public string Subject
        {
            set { _subjectDropdown.EnterForDropDown(value); }
            get { return _subjectDropdown.GetValue(); }
        }

        public bool Visibility
        {
            set { _visibility.Set(value); }
            get { return _visibility.IsCheckboxChecked(); }
        }

        public string Notes
        {
            set { _notes.SetText(value); }
            get { return _notes.GetValue(); }
        }

        #endregion

        #region Action
        public SelectEffectiveDateRangeDialog ClickSelectEffectDateRange()
        {
            _selectEffectDateRange.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new SelectEffectiveDateRangeDialog();
        }

        public AddPupilsDialogTriplet OpenAddPupilsDialog()
        {
            _addPupilButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddPupilsDialogTriplet();
        }

        public AddSupervisorsDialogTriplet OpenAddSupervisorDialog()
        {
            bool does = SeleniumHelper.IsElementExists(_addSupervisor);
            _addSupervisor.ClickByJS();

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSupervisorsDialogTriplet();
        }

        public void WaitForEffectDateUpdate(string value, int count = 10, bool isSingleValue = false)
        {
            IWebElement dateTimeElement;
            string formatPattern = null;
            string valueText = String.Empty;
            try
            {
                dateTimeElement = SeleniumHelper.FindElement(By.CssSelector("[data-date-validator-format]"));
            }
            catch (Exception)
            {
                dateTimeElement = null;
            }

            if (dateTimeElement != null)
            {
                formatPattern = dateTimeElement.GetAttribute("[data-date-validator-format]");
            }

            if (isSingleValue)
            {
                do
                {
                    this.Refresh();
                    if (formatPattern != null)
                    {
                        valueText = SeleniumHelper.Format(this._effectDate.GetValue(), formatPattern);
                    }
                    if (valueText.Equals(value))
                        break;
                    SeleniumHelper.Sleep(1);
                    count--;
                } while (count > 0);
            }
            else
            {
                valueText = this._effectDate.GetValue();
                string startDate = valueText.Split('-')[0].Trim();
                string endDate = valueText.Split('-')[1].Trim();
                do
                {
                    this.Refresh();
                    if (formatPattern != null)
                    {
                        valueText = String.Format("{0} - {1}", SeleniumHelper.Format(startDate, formatPattern), SeleniumHelper.Format(endDate, formatPattern));
                    }

                    if (valueText.Equals(value))
                        break;
                    SeleniumHelper.Sleep(1);
                    count--;
                } while (count > 0);

            }


        }

        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("well_know_action_save")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            Refresh();
        }

        public void ScrollToSupervisor()
        {
            if (_sectionSupervisor.GetAttribute("class").Contains("collapsed"))
            {
                _sectionSupervisor.ClickByJS();
            }
            else
            {
                _sectionSupervisor.ClickByJS();
                Wait.WaitLoading();
                _sectionSupervisor.Click();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='GroupSupervisors']"));
        }
        public void ScrollToGroupDetail()
        {
            if (_sectionGroupDetail.GetAttribute("class").Contains("collapsed"))
            {
                _sectionGroupDetail.ClickByJS();
            }
            else
            {
                _sectionGroupDetail.ClickByJS();
                Wait.WaitLoading();
                _sectionGroupDetail.Click();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[name$='EffectiveDate']"));
        }

        public void ScrollToPupil()
        {
            if (_sectionPupil.GetAttribute("class").Contains("collapsed"))
            {
                _sectionPupil.ClickByJS();
            }
            else
            {
                _sectionPupil.ClickByJS();
                Wait.WaitLoading();
                _sectionPupil.Click();
            }
            Wait.WaitForElementDisplayed(SimsBy.CssSelector("[data-maintenance-container='LearnerTeachingGroupMembership']"));
        }

        #endregion

        #region Table
        public GridComponent<PupilRow> PupilsTable
        {
            get
            {
                GridComponent<PupilRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PupilRow>(By.CssSelector("[data-maintenance-container='LearnerTeachingGroupMembership']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public GridComponent<Supervisor> SupervisorTable
        {
            get
            {
                GridComponent<Supervisor> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Supervisor>(By.CssSelector("[data-maintenance-container='TeachingGroupStaff']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class Supervisor : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.StaffPreferredListName']")]
            private IWebElement _nameTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StaffRole.dropdownImitator']")]
            private IWebElement _roleDropdown;

            [FindsBy(How = How.CssSelector, Using = "[name$='.IsMainSupervisor']")]
            private IWebElement _mainCheckbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _fromTextbox;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _toTextbox;

            public string Name
            {
                get
                {
                    return _nameTextbox.GetValue().Replace("  ", " ");
                }
            }

            public string Role
            {
                set
                {
                    _roleDropdown.EnterForDropDown(value);
                }

                get
                {
                    return _roleDropdown.GetValue();
                }
            }

            public bool Main
            {
                set { _mainCheckbox.Set(value); }
                get { return _mainCheckbox.IsChecked(); }
            }


            public string From
            {
                set { _fromTextbox.SetDateTime(value); }
                get { return _fromTextbox.GetDateTime(); }
            }

            public string To
            {
                get { return _toTextbox.GetDateTime(); }
                set { _toTextbox.SetDateTime(value); }
            }

        }

        public class PupilRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='LearnerPreferredListName']")]
            private IWebElement _name;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='MemberAsOnToday']")]
            private IWebElement _MemberAsOnToday;

            public string Name
            {
                set
                {
                    _name.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _name.GetValue();
                }
            }
            public string From
            {
                set
                {
                    _startDate.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _startDate.GetDateTime();
                }
            }
            public string To
            {
                set
                {
                    _endDate.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _endDate.GetDateTime();
                }
            }

            public string OnEDRStartDate
            {
                set
                {
                    _MemberAsOnToday.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _MemberAsOnToday.GetValue();
                }
            }
        }
        #endregion
    }
}
