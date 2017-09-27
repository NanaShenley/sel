using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using WebDriverRunner.webdriver;

namespace POM.Components.Staff
{
    public class StaffRecordPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_record_detail"); }
        }

        #region Page properties

        [FindsBy(How = How.Name, Using = "DateOfLeaving")]
        private IWebElement _staffRecordDateOfLeavingTextBox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='status_success']")]
        private IWebElement _successMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_more']")]
        private IWebElement _moreTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Personal']")]
        private IWebElement _personalDetailsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Personal']")]
        private IWebElement _personalDetailsHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Service']")]
        private IWebElement _serviceDetailsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Service']")]
        private IWebElement _serviceDetailsHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Addresses']")]
        private IWebElement _addressesTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Addresses']")]
        private IWebElement _addressesHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Phone/Email']")]
        private IWebElement _phoneEmailTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Phone/Email']")]
        private IWebElement _phoneEmailHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Contacts']")]
        private IWebElement _nextOfKinTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Contacts']")]
        private IWebElement _nextOfKinHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Absences']")]
        private IWebElement _absencesTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Absences']")]
        private IWebElement _absencesHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Medical']")]
        private IWebElement _medicalTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Medical']")]
        private IWebElement _medicalHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Ethnic/Cultural']")]
        private IWebElement _ethnicCulturalTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Ethnic/Cultural']")]
        private IWebElement _ethnicCulturalHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Training/Qualifications']")]
        private IWebElement _trainingQualificationsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Training/Qualifications']")]
        private IWebElement _trainingQualificationsHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Experience']")]
        private IWebElement _experiencesTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Experience']")]
        private IWebElement _experiencesHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Documents']")]
        private IWebElement _documentsTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_hidden_Documents']")]
        private IWebElement _documentsHiddenTab;

        [FindsBy(How = How.CssSelector, Using = "[title = 'Actions']")]
        private IWebElement _actionButton;

        [FindsBy(How = How.CssSelector, Using = "[data-loading-text='Staff Leaving Details']")]
        private IWebElement _staffLeavingDetailLink;

        [FindsBy(How = How.CssSelector, Using = ".clearfix")]
        private IWebElement _leaverMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='continue_with_delete_button']")]
        private IWebElement _continueDeleteButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_check_details_button']")]
        private IWebElement _addCheckDetailButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_languages_button']")]
        private IWebElement _addLanguageButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_bank_details_button']")]
        private IWebElement _bankBuildingSocialDetail;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_contract_details_button']")]
        private IWebElement _addContractDetailButton;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legaSurnameTextbox;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='image_upload_button']")]
        private IWebElement _photographUploadButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='image_remove_button']")]
        private IWebElement _photographRemoveButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='image_editor_img']")]
        private IWebElement _photo;

        [FindsBy(How = How.Name, Using = "LegalMiddleNames")]
        private IWebElement _middleName;

        [FindsBy(How = How.Name, Using = "LegalForename")]
        private IWebElement _legalForename;

        [FindsBy(How = How.Name, Using = "PreferredForename")]
        private IWebElement _preferForeNameTextBox;

        [FindsBy(How = How.Name, Using = "PreferredSurname")]
        private IWebElement _preferSurnameTextBox;

        [FindsBy(How = How.Name, Using = "Gender.dropdownImitator")]
        private IWebElement _gender;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _DOB;

        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        private IWebElement _dateOfArrival;

        [FindsBy(How = How.CssSelector, Using = "[class = 'accordion-toggle collapsed'][data-automation-id='section_menu_Experience']")]
        private IWebElement _experienceSectionTab;


        public string MessageSuccess
        {
            get { return _successMessage.GetValue(); }
        }

        public string LegelSurname
        {
            get { return _legaSurnameTextbox.GetValue(); }
        }

        public string LegalForeName
        {
            get { return _legalForename.GetAttribute("value"); }
        }

        public string MiddleName
        {
            get { return _middleName.GetAttribute("value"); }
        }

        public string DOB
        {
            get { return _DOB.GetDateTime(); }
        }

        public string Gender
        {
            get { return _gender.GetAttribute("value"); }
        }

        public string StaffRecordDateOfLeaving
        {
            set { _staffRecordDateOfLeavingTextBox.SetDateTime(value); }
            get { return _staffRecordDateOfLeavingTextBox.GetDateTime(); }
        }

        public string DateOfArrival
        {
            get { return _dateOfArrival.GetDateTime(); }
        }

        public string PreferSurname
        {
            get { return _preferSurnameTextBox.GetValue(); }
        }

        public string PreferForeName
        {
            get { return _preferForeNameTextBox.GetValue(); }
        }

        #endregion

        #region Page actions

        public static StaffRecordPage Create()
        {
            return new StaffRecordPage();
        }

        #region Tab Action

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Personal Details Tab
        /// </summary>
        public void SelectPersonalDetailsTab()
        {
            if (_personalDetailsTab.IsElementDisplayed())
            {
                _personalDetailsTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _personalDetailsHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au: An Nguyen
        /// Des : Select Service Details Tab
        /// </summary>
        public void SelectServiceDetailsTab()
        {
            if (_serviceDetailsTab.IsElementDisplayed())
            {
                Retry.Do(_serviceDetailsTab.ClickByJS);
            }
            else
            {
                _moreTab.ClickByJS();
                _serviceDetailsHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        public void DeleteStaffBackground(StaffBackgroundCheck row)
        {
            if (row != null)
            {
                row.ClickDelete();
                ConfirmDelete();
            }
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Addresses Tab
        /// </summary>
        public void SelectAddressesTab()
        {
            if (_addressesTab.IsElementDisplayed())
            {
                _addressesTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _addressesHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Phone/Email Tab
        /// </summary>
        public void SelectPhoneEmailTab()
        {
            if (_phoneEmailTab.IsElementDisplayed())
            {
                _phoneEmailTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _phoneEmailHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Next of kin Tab
        /// </summary>
        public void SelectNextOfKinTab()
        {
            if (_nextOfKinTab.IsElementDisplayed())
            {
                _nextOfKinTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _nextOfKinHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Absences Tab
        /// </summary>
        public void SelectAbsencesTab()
        {
            if (_absencesTab.IsElementDisplayed())
            {
                _absencesTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _absencesHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Medical Tab
        /// </summary>
        public void SelectMedicalTab()
        {
            if (_medicalTab.IsElementDisplayed())
            {
                Retry.Do(_medicalTab.Click);
            }
            else
            {
                _moreTab.ClickByJS();
                _medicalHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Ethnic/Cultural Tab
        /// </summary>
        public void SelectEthnicCulturalTab()
        {
            if (_ethnicCulturalTab.IsElementDisplayed())
            {
                _ethnicCulturalTab.ClickByJS();
            }
            else
            {
                _moreTab.ClickByJS();
                _ethnicCulturalHiddenTab.ClickByJS();
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Training Qualifications Tab
        /// </summary>
        public void SelectTrainingQualificationsTab()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Training/Qualifications']")).Displayed)
            {
                Retry.Do(_trainingQualificationsTab.Click);
            }
            else
            {
                SeleniumHelper.ClickAndWaitFor(_moreTab, By.CssSelector("[data-automation-id='section_menu_hidden_Training/Qualifications']"));
                Retry.Do(_trainingQualificationsHiddenTab.Click);
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Experience Tab
        /// </summary>
        public void SelectExprienceTab()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Experience']")).Displayed)
            {
                Retry.Do(_experiencesTab.Click);
            }
            else
            {
                _moreTab.ClickByJS();
                //SeleniumHelper.ClickAndWaitFor(_moreTab, By.CssSelector("[data-automation-id='section_menu_hidden_Experience']"));
                Retry.Do(_experiencesHiddenTab.Click);
            }
            Wait.WaitLoading();
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Experience Tab
        /// </summary>
        public void SelectDocumentsTab()
        {
            if (SeleniumHelper.Get(By.CssSelector("[data-automation-id='section_menu_Documents']")).Displayed)
            {
                Retry.Do(_documentsTab.Click);
            }
            else
            {
                SeleniumHelper.ClickAndWaitFor(_moreTab, By.CssSelector("[data-automation-id='section_menu_hidden_Documents']"));
                Retry.Do(_documentsHiddenTab.Click);
            }
            Wait.WaitLoading();
        }

        #endregion

        /// <summary>
        /// Au : An Nguyen
        /// Des : Verify success message is displayed
        /// </summary>
        /// <returns></returns>
        public bool IsSuccessMessageDisplayed()
        {
            return SeleniumHelper.DoesWebElementExist(_successMessage);
        }

        public StaffLeavingDetailPage NavigateToStaffLeavingDetail()
        {
            IWebElement actionElement = SeleniumHelper.FindElement(SimsBy.CssSelector("[title = 'Actions']"));
            if (actionElement.IsElementDisplayed())
            {
                actionElement.Click();
            }

            _staffLeavingDetailLink.Click();
            Wait.WaitForAjaxReady(By.Id("nprogress"));

            return new StaffLeavingDetailPage();
        }

        public bool IsLeaverStaff()
        {
            try
            {
                IWebElement _leaverMessage = SeleniumHelper.FindElement(SimsBy.CssSelector(".clearfix"));
                return _leaverMessage.IsExist();
            }
            catch (Exception)
            {
                return false;
            }


        }

        public bool IsPageDisplayed()
        {

            try
            {
                return SeleniumHelper.FindElement(SimsBy.AutomationId("tab_Staff_Record")).IsExist();
            }
            catch (NoSuchElementException e)
            {
                return false;
            }

        }

        public void SaveStaff()
        {
            Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
            Refresh();
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        public void DeleteStaff()
        {
            if (_deleteButton.IsExist())
            {
                _deleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                var deleteConfirmationPage = new DeleteConfirmationPage();
                deleteConfirmationPage.ConfirmDelete();
            }
        }

        public StaffRecordPage ContinueDeleteStaff()
        {
            if (_continueDeleteButton.IsExist())
            {
                _continueDeleteButton.ClickByJS();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            return new StaffRecordPage();
        }

        private AutomationElement OpenTheUploadDIalog(int timeout = 26)
        {
            bool isExist = false;
            AutomationElement DialogOpen = null;
            do
            {
                Retry.Do(_photographUploadButton.Click);

                ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;

                AutomationElement window = null;
                AutomationElement FileFieldName = null;

                // Detect element
                if (capabilities.BrowserName.ToLower().Contains("chrome"))
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));

                }
                else
                {
                    window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
                }

                if (window != null)
                {

                    DialogOpen = window.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "Dialog"));

                    if (DialogOpen != null)
                        return window;
                }

                isExist = Wait.WaitForUIAutoElementReady(DialogOpen);
                timeout--;
            }
            while (timeout > 0 && isExist == false);

            return DialogOpen;
        }
        public void UIAutoEnterValueIntoOpenDialog(AutomationElement window, string value)
        {
            ICapabilities capabilities = ((RemoteWebDriver)WebContext.WebDriver).Capabilities;
            AutomationElement DialogOpen = null;
            if (capabilities.BrowserName.ToLower().Contains("chrome"))
            {
                window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Google Chrome"));
            }
            else
            {
                window = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, "SIMS - Internet Explorer"));
            }

            if (window != null)
            {
                DialogOpen = window.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "Dialog"));
            }

            if (DialogOpen != null)
            {
                // Find Open dialog
                Condition condition = new AndCondition(
                new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "edit"),
                new PropertyCondition(AutomationElement.NameProperty, "File name:"));

                var FileFieldName = DialogOpen.FindFirst(TreeScope.Descendants, condition);
                FileFieldName.SetFocus();
                ValuePattern valuePatternA =
                FileFieldName.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                valuePatternA.SetValue(value);

                // 
                if (FileFieldName != null)
                {
                    Condition conditionButtonOpen = new AndCondition(
                                                    new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "button"),
                                                    new PropertyCondition(AutomationElement.NameProperty, "Open"));

                    var OpenButton = DialogOpen.FindFirst(TreeScope.Descendants, conditionButtonOpen);
                    SendKeys.SendWait("{Enter}");
                    //if (OpenButton != null)
                    //{
                    //    InvokePattern ClickInvoke =
                    //        OpenButton.GetCurrentPattern(InvokePattern.Pattern) as InvokePattern;
                    //    ClickInvoke.Invoke();
                    //}
                }
                //SendKeys.SendWait("{Enter}");
            }
        }

        private string createPNGFile(string path)
        {
            if (!File.Exists(path))
                using (Bitmap b = new Bitmap(50, 50))
                {
                    using (Graphics g = Graphics.FromImage(b))
                    {
                        g.Clear(Color.Green);
                    }

                    b.Save(path, ImageFormat.Png);
                }
            return path;
        }

        public void UploadAvatar()
        {

            var dialog = OpenTheUploadDIalog();
            string pathFile = TestSettings.TestDefaults.Default.PathAvatar;

            this.UIAutoEnterValueIntoOpenDialog(dialog, pathFile);
        }

        public bool IsAvatartUploadSuccess()
        {
            var result = _photo.GetAttribute("src");
            if (result.Equals("null"))
                return false;
            return true;
        }
        #endregion

        #region StaffRoleStandard Grid

        public GridComponent<StaffRoleStandardRow> StaffRoleStandardTable
        {
            get
            {
                GridComponent<StaffRoleStandardRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffRoleStandardRow>(By.CssSelector("[data-maintenance-container='StaffRoleAssignments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffRoleStandardRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[id$='StaffRole_dropdownImitator']")]
            private IWebElement _staffRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _staffStartDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _staffEndDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='.Description']")]
            private IWebElement _description;

            [FindsBy(How = How.CssSelector, Using = "[name$='.DisplayOrder']")]
            private IWebElement _displayOrder;

            [FindsBy(How = How.CssSelector, Using = "[name$='Parent.dropdownImitator']")]
            private IWebElement _category;


            public string StaffRole
            {
                set
                {
                    _staffRole.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get { return _staffRole.GetAttribute("value"); }
            }

            public string StaffStartDate
            {
                set
                {
                    _staffStartDate.Click();

                    _staffStartDate.SetDateTimeByJS(value);

                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                    Wait.WaitLoading();

                }
                get { return _staffStartDate.GetDateTime(); }
            }

            public string StaffEndDate
            {
                set
                {
                    if (value == null)
                    {
                        _staffEndDate.Click();
                        Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                        Wait.WaitLoading();
                        return;
                    }

                    _staffEndDate.Click();
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));

                    _staffEndDate.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _staffEndDate.GetValue();
                }
            }

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetValue(); }
            }

            public string DisplayOrder
            {
                set { _displayOrder.SetText(value); }
                get { return _displayOrder.GetValue(); }
            }

            public string Category
            {
                set { _category.EnterForDropDown(value); }
                get { return _category.GetValue(); }
            }
        }

        #endregion

        #region BackgroundCheck Grid

        public GridComponent<StaffBackgroundCheck> BackgroundCheckTable
        {
            get
            {
                GridComponent<StaffBackgroundCheck> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffBackgroundCheck>(By.CssSelector("[data-maintenance-container='StaffChecks']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffBackgroundCheck : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name $= 'StaffCheckType.dropdownImitator']")]
            private IWebElement _check;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'StaffCheckClearanceLevel.dropdownImitator']")]
            private IWebElement _clearanceLevel;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'ClearanceDate']")]
            private IWebElement _clearanceDate;

            [FindsBy(How = How.CssSelector, Using = "[name $= 'ExpiryDate']")]
            private IWebElement _expireDate;


            public string Check
            {
                set { _check.EnterForDropDown(value); }
                get { return _check.GetAttribute("value"); }
            }

            public string ClearanceLevel
            {
                set { _clearanceLevel.EnterForDropDown(value); }
                get { return _clearanceLevel.GetAttribute("value"); }
            }

            public string ClearanceDate
            {
                get { return _clearanceDate.GetDateTime(); }
            }

            public string ExpireDate
            {
                get { return _expireDate.GetDateTime(); }
            }

        }
        #endregion

        #region StaffBankBuilding Grid

        public GridComponent<StaffBankBuildingRow> StaffBankBuildingTable
        {
            get
            {
                GridComponent<StaffBankBuildingRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<StaffBankBuildingRow>(By.CssSelector("[data-maintenance-container='FinancialAccounts']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class StaffBankBuildingRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='BankAccountName']")]
            private IWebElement _accountName;

            [FindsBy(How = How.CssSelector, Using = "[name$='BankName']")]
            private IWebElement _bankBuildingSocietyName;

            public string AccountName
            {
                set { _accountName.SetText(value); }
                get { return _accountName.GetAttribute("value"); }
            }

            public string BankBuildingSocietyName
            {
                set { _bankBuildingSocietyName.SetText(value); }
                get { return _bankBuildingSocietyName.GetAttribute("value"); }
            }

        }

        #endregion

        #region StaffAddresses Grid

        public GridComponent<AddressRow> AddressTable
        {

            get
            {
                GridComponent<AddressRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AddressRow>(By.CssSelector("[data-maintenance-container='Addresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AddressRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='StaffAddressesAddress']")]
            private IWebElement _address;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
            private IWebElement _addressStatus;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
            private IWebElement _addressType;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            public string Address
            {
                get { return _address.GetText(); }
            }

            public string AddressStatus
            {
                get { return _addressStatus.GetValue(); }
            }

            public string AddressType
            {
                set { _addressType.EnterForDropDown(value); }
                get { return _addressType.GetAttribute("value"); }
            }

            public string StartDate
            {
                set { _startDate.SetText(value); }
                get { return _startDate.GetAttribute("value"); }
            }

            public string EndDate
            {
                set { _endDate.SetText(value); }
                get { return _endDate.GetAttribute("value"); }
            }
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

            [FindsBy(How = How.CssSelector, Using = "[name*='tri_chkbox_StaffTelephones'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='tri_chkbox_StaffTelephones'][name$='IsMainTelephone']")]
            private IWebElement _mainNumber;

            public string TelephoneNumber
            {
                set
                {
                    _telephoneNumber.SetText(value);
                    Retry.Do(_endCol.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
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

            [FindsBy(How = How.CssSelector, Using = "[name*='tri_chkbox_StaffEmails'][name$='UseForTextMessages']")]
            private IWebElement _aMS;

            [FindsBy(How = How.CssSelector, Using = "[name*='tri_chkbox_StaffEmails'][name$='IsMainEmail']")]
            private IWebElement _mainEmail;

            public string EmailAddress
            {
                set
                {
                    _emailAddress.SetText(value);
                    Retry.Do(_endCol.Click);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
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

        #region TrainingHistory Grid

        public GridComponent<TrainingHistoryRow> TrainingHistoryTable
        {

            get
            {
                GridComponent<TrainingHistoryRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TrainingHistoryRow>(By.CssSelector("[data-maintenance-container='StaffTrainingCourseEnrolments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class TrainingHistoryRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name $= 'TrainingCourseTitle']")]
            private IWebElement _title;


            public string Title
            {
                set { _title.SetText(value); }
                get { return _title.GetValue(); }
            }

        }
        #endregion

        #region Absences Grid

        public GridComponent<AbsencesRow> Absences
        {
            get
            {
                GridComponent<AbsencesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<AbsencesRow>(By.CssSelector("[data-maintenance-container='StaffAbsences']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class AbsencesRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='WorkingDaysLost']")]
            private IWebElement _WorkingLostDayField;

            [FindsBy(How = How.CssSelector, Using = "[name$='WorkingHoursLost']")]
            private IWebElement _NumberOfHoursLostField;

            [FindsBy(How = How.CssSelector, Using = "[name$='AbsenceType.dropdownImitator']")]
            private IWebElement _AbsenceType;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_note_button']")]
            private IWebElement _Note;

            [FindsBy(How = How.CssSelector, Using = "[name$='AbsencePayRate.dropdownImitator']")]
            private IWebElement _AuthorisedPayRate;

            public string FirstDay
            {
                get { return _startDateField.GetDateTime(true); }
            }

            public string LastDay
            {
                get { return _endDateField.GetDateTime(true); }
            }

            public string WorkingLostDay
            {
                get { return _WorkingLostDayField.GetAttribute("value"); }
            }

            public string NumberOfHoursLost
            {
                get { return _NumberOfHoursLostField.GetAttribute("value"); }
            }

            public string AbsenceType
            {
                get { return _AbsenceType.GetAttribute("value"); }
            }

            public string Note
            {
                get { return _Note.GetAttribute("value"); }
            }

            public string AuthorisedPayRate
            {
                get { return _AuthorisedPayRate.GetAttribute("value"); }
            }
        }
        #endregion

        #region Grid add action

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_an_additional_address_button']")]
        private IWebElement _addAddressButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_details_button'][data-ajax-url$='StaffImpairmentDialog']")]
        private IWebElement _addDetailMedicalButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_details_button'][data-ajax-url$='StaffExperienceDialog']")]
        private IWebElement _addDetailExperiencedButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_training_event_button']")]
        private IWebElement _addTrainingEventButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_absence_button']")]
        private IWebElement _addStaffAbsence;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_contact_button']")]
        private IWebElement _addStaffContactButton;

        public AddAddressTripletDialog ClickAddAddress()
        {
            _addAddressButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddAddressTripletDialog();
        }

        public AddImpairmentDialog ClickMedicalDetail()
        {
            _addDetailMedicalButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddImpairmentDialog();
        }

        public AddBackgroundCheckDialog ClickAddBackGroudCheck()
        {
            _addCheckDetailButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddBackgroundCheckDialog();
        }

        public void ClickAddLanguage()
        {
            _addLanguageButton.ClickByJS();
            _addLanguageButton.WaitUntilState(ElementState.Enabled);
            Wait.WaitLoading();
        }


        public AddStaffExperiencedDialog ClickStaffExperience()
        {
            _addDetailExperiencedButton.ClickByJS();

            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStaffExperiencedDialog();
        }

        public BankBuildingSocialDetailDialog ClickBankBuildingSocialDetail()
        {
            _bankBuildingSocialDetail.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new BankBuildingSocialDetailDialog();
        }
        public AddContractDetailDialog ClickContractDetail()
        {
            _addContractDetailButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddContractDetailDialog();
        }

        public AddStaffContactTripletDialog ClickAddContact()
        {
            _addStaffContactButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddStaffContactTripletDialog();
        }

        public FindTrainingCourseEventDialog ClickTrainingCourse()
        {
            _addTrainingEventButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new FindTrainingCourseEventDialog();
        }

        public StaffAbsenceDialog ClickAddAbsence()
        {
            _addStaffAbsence.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new StaffAbsenceDialog();
        }

        #endregion

        #region Grid delete action

        public void ClickDeleteTableRow(TableRowElement record)
        {
            if (record != null)
            {
                record.ClickDelete();
            }
        }

        public void ConfirmDelete()
        {

            SeleniumHelper.Get(SimsBy.AutomationId("Yes_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        /// <summary>
        /// Au: Hieu Pham
        /// Des: Click to delete a row
        /// </summary>

        public void DeleteStaffRole(StaffRoleStandardRow row)
        {
            if (row != null)
            {
                row.DeleteRow();
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

        #region Experience Grid

        public GridComponent<ExperienceRow> ExperienceTable
        {
            get
            {
                GridComponent<ExperienceRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ExperienceRow>(By.CssSelector("[data-maintenance-container='StaffExperience']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ExperienceRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Employer']")]
            private IWebElement _employer;

            [FindsBy(How = How.CssSelector, Using = "[name$='StaffRole']")]
            private IWebElement _role;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateOfArrival']")]
            private IWebElement _dateOfArrival;

            public string Employer
            {
                set
                {
                    _employer.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                }
                get
                {
                    return _employer.GetAttribute("value");
                }
            }

            public string Role
            {
                get
                {
                    return _role.GetAttribute("value");
                }
            }

            public string DateOfArrival
            {
                get { return _dateOfArrival.GetAttribute("value"); }
            }

        }

        #endregion

        #region Impairment Grid

        public GridComponent<ImpairmentRow> ImpairmentTable
        {
            get
            {
                GridComponent<ImpairmentRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ImpairmentRow>(By.CssSelector("[data-maintenance-container='StaffImpairments']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ImpairmentRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Impairment']")]
            private IWebElement _impairment;

            [FindsBy(How = How.CssSelector, Using = "[id$='ImpairmentCategory_dropdownImitator']")]
            private IWebElement _category;

            [FindsBy(How = How.CssSelector, Using = "[name$='DateAdvised']")]
            private IWebElement _advisedDate;

            public string Impairment
            {
                set
                {
                    _impairment.SetText(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                }
                get { return _impairment.GetValue(); }
            }

            public string Category
            {
                get { return _category.GetAttribute("value"); }
            }

            public string AdvisedDate
            {
                get { return _advisedDate.GetDateTime(); }
            }

        }

        #endregion

        #region Language Grid

        public GridComponent<LanguageRow> LanguageTable
        {
            get
            {
                GridComponent<LanguageRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<LanguageRow>(By.CssSelector("[data-maintenance-container='StaffLanguageRelationships']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class LanguageRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='StaffLanguage.dropdownImitator']")]
            private IWebElement _language;

            [FindsBy(How = How.CssSelector, Using = "[name$='IsFirstLanguage']")]
            private IWebElement _firstLanguage;

            public string Language
            {
                set
                {
                    _language.EnterForDropDown(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                }
                get { return _language.GetValue(); }
            }

            public bool FirstLanguage
            {
                set
                {
                    _firstLanguage.Set(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
                }
                get { return _firstLanguage.IsCheckboxChecked(); }
            }
        }

        #endregion

        #region MedicalNotes Grid

        public GridComponent<MedicalNotesRow> MedicalNotesTable
        {
            get
            {
                GridComponent<MedicalNotesRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<MedicalNotesRow>(By.CssSelector("[data-maintenance-container='StaffMedicalNotes']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class MedicalNotesRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='Summary']")]
            private IWebElement _summary;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id^='document']")]
            private IWebElement _document;

            public string Summary
            {
                set { _summary.SetText(value); }
                get { return _summary.GetValue(); }
            }

            public void AddDocument()
            {
                _document.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }

            public void WaitAddDocumentCellEnable(int timeOut = 4)
            {

                while (timeOut > 0)
                {
                    try
                    {
                        string value = _document.GetAttribute("disabled");
                        if (value.Equals("disabled"))
                        {

                            Thread.Sleep(1000);
                            timeOut--;
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        #endregion

        #region Contracts Grid

        public GridComponent<ContractsTableRow> ContractsTable
        {
            get
            {
                GridComponent<ContractsTableRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ContractsTableRow>(By.CssSelector("[data-maintenance-container='EmployeeEmploymentContracts']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ContractsTableRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='ServiceTermDescription']")]
            private IWebElement _ServiceTermDescription;

            [FindsBy(How = How.CssSelector, Using = "[name$='PostTypeDisplay']")]
            private IWebElement _PostType;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string ServiceTerm
            {
                get { return _ServiceTermDescription.GetAttribute("value"); }
            }

            public string PostType
            {
                get { return _PostType.GetAttribute("value"); }
            }
            public EditContractDialog Edit()
            {
                _editButton.ClickByJS();
                Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
                return new EditContractDialog();
            }

        }

        #endregion

        #region VehicleDetail Grid

        public GridComponent<VehicleDetailRow> VehicleDetailTable
        {

            get
            {
                GridComponent<VehicleDetailRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<VehicleDetailRow>(By.CssSelector("[data-maintenance-container='StaffVehicleDetails']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class VehicleDetailRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='VehicleRegistration']")]
            private IWebElement _vehicleRegistration;

            [FindsBy(How = How.CssSelector, Using = "[name$='Make']")]
            private IWebElement _make;

            [FindsBy(How = How.CssSelector, Using = "[name$='VehicleModel']")]
            private IWebElement _model;


            [FindsBy(How = How.CssSelector, Using = "[name$='Color']")]
            private IWebElement _color;

            [FindsBy(How = How.CssSelector, Using = "[name$='PermitNumber']")]
            private IWebElement _permitNumber;

            public string VehicleRegistration
            {
                set { _vehicleRegistration.SetText(value); }
                get { return _vehicleRegistration.GetAttribute("value"); }
            }

            public string Make
            {
                set { _make.SetText(value); }
                get { return _make.GetAttribute("value"); }
            }

            public string Model
            {
                set { _model.SetText(value); }
                get { return _model.GetAttribute("value"); }
            }

            public string Colour
            {
                set { _color.SetText(value); }
                get { return _color.GetAttribute("value"); }
            }

            public string PermitNumber
            {
                set { _permitNumber.SetText(value); }
                get { return _permitNumber.GetAttribute("value"); }
            }

        }
        #endregion
    }
}
