using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Components.Staff.Dialogs;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using WebDriverRunner.webdriver;

namespace Staff.POM.Components.Staff
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

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_contract_button']")]
        private IWebElement _addContractButton;

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _legalSurnameTextbox;

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

        [FindsBy(How = How.Name, Using = "Title.dropdownImitator")]
        private IWebElement _title;

        [FindsBy(How = How.Name, Using = "MaritalStatus.dropdownImitator")]
        private IWebElement _maritalStatus;

        [FindsBy(How = How.Name, Using = "DateOfBirth")]
        private IWebElement _DOB;

        [FindsBy(How = How.Name, Using = "DateOfArrival")]
        private IWebElement _dateOfArrival;

        [FindsBy(How = How.Name, Using = "QuickNote")]
        private IWebElement _quickNote;

        [FindsBy(How = How.CssSelector, Using = "[class = 'accordion-toggle collapsed'][data-automation-id='section_menu_Experience']")]
        private IWebElement _experienceSectionTab;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_staff_role_button']")]
        private IWebElement _addStaffRoleButton;

        [FindsBy(How = How.CssSelector, Using = "div.validation-summary-errors>ul>li")]
        private IList<IWebElement> _validationErrors;

        [FindsBy(How = How.Name, Using = "QualifiedTeacherStatus.dropdownImitator")]
        private IWebElement _qualifiedTeacherStatus;

        [FindsBy(How = How.Name, Using = "QTSRoute.dropdownImitator")]
        private IWebElement _qtsRoute;

        [FindsBy(How = How.CssSelector, Using = "[name$='IsEligibleForSWC']")]
        private IWebElement _isEligibleForSWC;

        [FindsBy(How = How.Name, Using = "StaffReligion.dropdownImitator")]
        private IWebElement _staffReligion;

        [FindsBy(How = How.Name, Using = "NationalIdentity.dropdownImitator")]
        private IWebElement _nationalIdentity;

        public string MessageSuccess
        {
            get { return _successMessage.GetValue(); }
        }

        public string LegalSurname
        {
            get { return _legalSurnameTextbox.GetValue(); }
            set
            {
                SeleniumHelper.SetTextInUpdatePanel(value, new ByChained(this.ComponentIdentifier, By.Name("LegalSurname")), false);
                SeleniumHelper.TriggerChange("LegalSurname");
            }
        }

        public string LegalForeName
        {
            get { return _legalForename.GetValue(); }
            set
            {
                SeleniumHelper.SetTextInUpdatePanel(value, new ByChained(this.ComponentIdentifier, By.Name("LegalForename")), false);
                SeleniumHelper.TriggerChange("LegalForename");
            }
        }


        public string MiddleName
        {
            get { return _middleName.GetAttribute("value"); }
            set { _middleName.SetText(value); }
        }

        public string DOB
        {
            get { return _DOB.GetValue(); }
            set { _DOB.SetText(value); }
        }

        public string Gender
        {
            get { return _gender.GetValue(); }
            set { _gender.ChooseSelectorOption(value); }
        }

        public string Title
        {
            get { return _title.GetValue(); }
            set { _title.ChooseSelectorOption(value); }
        }

        public string MaritalStatus
        {
            get { return _maritalStatus.GetValue(); }
            set { _maritalStatus.ChooseSelectorOption(value); }
        }

        public string StaffRecordDateOfLeaving
        {
            get { return _staffRecordDateOfLeavingTextBox.GetValue(); }
        }

        public string DateOfArrival
        {
            get { return _dateOfArrival.GetDateTime(); }
            set { _dateOfArrival.SetText(value); }
        }

        public string PreferSurname
        {
            get { return _preferSurnameTextBox.GetValue(); }
            set { _preferSurnameTextBox.SetText(value); }
        }

        public string PreferForeName
        {
            get { return _preferForeNameTextBox.GetValue(); }
            set { _preferForeNameTextBox.SetText(value); }
        }

        public string QuickNote
        {
            get { return _quickNote.GetValue(); }
            set { _quickNote.SetText(value); }
        }

        public IEnumerable<string> ValidationErrors
        {
            get { return _validationErrors.Select(x => x.Text); }
        }

        public string QualifiedTeacherStatus
        {
            get { return _qualifiedTeacherStatus.GetValue(); }
            set
            {
                _qualifiedTeacherStatus.ChooseSelectorOption(value);

                if (value.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    WebDriverWait wait = new WebDriverWait(SeSugar.Environment.WebContext.WebDriver, SeSugar.Environment.Settings.ElementRetrievalTimeout);
                    Retry.Do(() => { wait.Until(x => x.FindElement(By.Name("QTSRoute.dropdownImitator")).Enabled); });
                }
            }
        }

        public string QTSRoute
        {
            get { return _qtsRoute.GetValue(); }
            set { _qtsRoute.ChooseSelectorOption(value); }
        }

        public bool IsEligibleForSWC
        {
            set { _isEligibleForSWC.Set(value); }
            get { return _isEligibleForSWC.IsChecked(); }
        }

        public string StaffReligion
        {
            get { return _staffReligion.GetValue(); }
            set { _staffReligion.ChooseSelectorOption(value); }
        }

        public string NationalIdentity
        {
            get { return _nationalIdentity.GetValue(); }
            set { _nationalIdentity.ChooseSelectorOption(value); }
        }

        #endregion

        #region Page actions

        public bool QTSRouteEnabled()
        {
            return _qtsRoute.Enabled;
        }

        public static StaffRecordPage Create()
        {
            return new StaffRecordPage();
        }

        public static StaffRecordPage LoadStaffDetail(Guid staffId)
        {
            var jsExecutor = (IJavaScriptExecutor)SeSugar.Environment.WebContext.WebDriver;
            string js = "sims_commander.OpenDetail(undefined, '/{0}/Staff/SIMS8StaffMaintenanceTripleStaff/ReadDetail/{1}')";

            Retry.Do(() => { jsExecutor.ExecuteScript(string.Format(js, TestSettings.TestDefaults.Default.Path, staffId)); });

            AutomationSugar.WaitForAjaxCompletion();

            return new StaffRecordPage();
        }

        public void DeleteStaffBackground(StaffBackgroundCheck row)
        {
            if (row != null)
            {
                row.ClickDelete();
                ConfirmDelete();
            }
        }

        #region Tab Action

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Personal Details Tab
        /// </summary>
        public void SelectPersonalDetailsTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Personal");
        }

        /// <summary>
        /// Au: An Nguyen
        /// Des : Select Service Details Tab
        /// </summary>
        public void SelectServiceDetailsTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Service");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Addresses Tab
        /// </summary>
        public void SelectAddressesTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Addresses");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Phone/Email Tab
        /// </summary>
        public void SelectPhoneEmailTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Phone/Email");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Next of kin Tab
        /// </summary>
        public void SelectNextOfKinTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Contacts");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Absences Tab
        /// </summary>
        public void SelectAbsencesTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Absences");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Medical Tab
        /// </summary>
        public void SelectMedicalTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Medical");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Ethnic/Cultural Tab
        /// </summary>
        public void SelectEthnicCulturalTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Ethnic/Cultural");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Training Qualifications Tab
        /// </summary>
        public void SelectTrainingQualificationsTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Training/Qualifications");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Experience Tab
        /// </summary>
        public void SelectExprienceTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Experience");
        }

        /// <summary>
        /// Au : An Nguyen
        /// Des : Select Experience Tab
        /// </summary>
        public void SelectDocumentsTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_Documents");
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

        public void SelectUDFTab()
        {
            AutomationSugar.ExpandAccordionPanel("section_menu_User Defined Fields");
        }

        public StaffLeavingDetailPage NavigateToStaffLeavingDetail()
        {
            By actionsSelector = By.CssSelector("[title='Actions']");

            var actions = SeSugar.Environment.WebContext.WebDriver.FindElements(actionsSelector);
            bool actionsExists = actions.Any();
            bool actionsVisible = actions.Any() ? actions[0].Displayed : false;

            if (actionsVisible)
            {
                AutomationSugar.WaitFor("contextual_action_links_button");
                AutomationSugar.ClickOn("contextual_action_links_button");
            }

            AutomationSugar.WaitFor("service_navigation_contextual_link_Make_Leaver");
            AutomationSugar.ClickOn("service_navigation_contextual_link_Make_Leaver");
            AutomationSugar.WaitForAjaxCompletion();

            return new StaffLeavingDetailPage();
        }

        public bool IsDolDisplayed
        {
            get
            {
                //This doesn't mean theyve left - this means the DOL readonly text box is present. 
                //The DOL could be in the future.
                var dolTextBoxLocator = new ByChained(this.ComponentIdentifier, By.Name("DateOfLeaving"));
                return SeSugar.Environment.WebContext.WebDriver.FindElements(dolTextBoxLocator).Any();
            }
        }

        public bool IsReAdmitDisplayed
        {
            get
            {
                var reAdmitButtonLocator = new ByChained(this.ComponentIdentifier, SimsBy.AutomationId("re-admit_staff_button"));
                return SeSugar.Environment.WebContext.WebDriver.FindElements(reAdmitButtonLocator).Any();
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

        public ConfirmRequiredDialog SaveStaffWithDeleteConfirmation()
        {
            Wait.WaitUntilDisplayed(By.CssSelector("[data-automation-id='well_know_action_save']"));
            Refresh();
            _saveButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ConfirmRequiredDialog();
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

        public void ClickConfirmLegalNameChange(string automationIdStartsWith)
        {
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.WaitFor(new ByChained(By.CssSelector(".modal-dialog"), By.CssSelector("[data-automation-id^='" + automationIdStartsWith + "']")));
            AutomationSugar.ClickOn(By.CssSelector("[data-automation-id^='" + automationIdStartsWith + "']"));
            AutomationSugar.WaitUntilStale(By.CssSelector("[data-automation-id^='" + automationIdStartsWith + "']"));
        }

        public ServiceDetailDialog ClickReAdmitStaff()
        {
            AutomationSugar.WaitForAjaxCompletion();
            AutomationSugar.ClickOnAndWaitFor("re-admit_staff_button", "save_record_button");
            return new ServiceDetailDialog();
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

                if (FileFieldName != null)
                {
                    Condition conditionButtonOpen = new AndCondition(
                                                    new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "button"),
                                                    new PropertyCondition(AutomationElement.NameProperty, "Open"));

                    var OpenButton = DialogOpen.FindFirst(TreeScope.Descendants, conditionButtonOpen);
                    SendKeys.SendWait("{Enter}");
                }
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

        public GridComponent<ServiceAgreementStandardRow> ServieAgreementStandardTable
        {
            get
            {
                return new GridComponent<ServiceAgreementStandardRow>(By.CssSelector("[data-maintenance-container='ServiceAgreements']"), ComponentIdentifier);
            }
        }

        public class ServiceAgreementStandardRow : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _endDate;

        }

        public GridComponent<StaffRoleStandardRow> StaffRoleStandardTable
        {
            get
            {
                return new GridComponent<StaffRoleStandardRow>(By.CssSelector("[data-maintenance-container='StaffRoleAssignments']"), ComponentIdentifier);
            }
        }

        public class StaffRoleStandardRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[id$='StaffRole_dropdownImitator']")]
            private IWebElement _staffRole;

            [FindsBy(How = How.CssSelector, Using = "[name$='.StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='.EndDate']")]
            private IWebElement _endDate;

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
                    AutomationSugar.WaitForAjaxCompletion();
                }
                get { return _staffRole.GetAttribute("value"); }
            }

            public string StartDate
            {
                set { _startDate.SetText(value); }
                get { return _startDate.GetValue(); }
            }

            public string EndDate
            {
                set { _endDate.SetText(value); }
                get { return _endDate.GetValue(); }
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
            private IWebElement _expiryDate;


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
                get { return _clearanceDate.GetValue(); }
            }

            public string ExpiryDate
            {
                get { return _expiryDate.GetValue(); }
            }

            public AddBackgroundCheckDialog ClickEdit()
            {
                AutomationSugar.WaitFor("edit..._button");
                AutomationSugar.ClickOn("edit..._button");
                AutomationSugar.WaitForAjaxCompletion();
                return new AddBackgroundCheckDialog();
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

            public void ClickEditAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Edit_Address_Action");
                AutomationSugar.ClickOn("Edit_Address_Action");
                AutomationSugar.WaitForAjaxCompletion();
            }

            public void ClickMoveAddress()
            {
                AutomationSugar.WaitFor("Action_Dropdown");
                AutomationSugar.ClickOn("Action_Dropdown");
                AutomationSugar.WaitForAjaxCompletion();

                AutomationSugar.WaitFor("Move_address_Action");
                AutomationSugar.ClickOn("Move_address_Action");
                AutomationSugar.WaitForAjaxCompletion();
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

        #region Grid add action

        public AddAddressTripletDialog ClickAddAddress()
        {
            AutomationSugar.WaitFor("add_address_button");
            AutomationSugar.ClickOn("add_address_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddAddressTripletDialog();
        }     

        public AddImpairmentDialog ClickMedicalDetail()
        {
            AutomationSugar.WaitFor("add_impairment_button");
            AutomationSugar.ClickOn("add_impairment_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddImpairmentDialog();
        }

        public AddBackgroundCheckDialog ClickAddBackGroudCheck()
        {
            AutomationSugar.WaitFor("add_background_check_button");
            AutomationSugar.ClickOn("add_background_check_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddBackgroundCheckDialog();
        }

        public void ClickAddLanguage()
        {
            AutomationSugar.WaitFor("add_language_button");
            AutomationSugar.ClickOn("add_language_button");
            AutomationSugar.WaitForAjaxCompletion();
            Wait.WaitLoading();
        }

        public AddStaffExperiencedDialog ClickStaffExperience()
        {
            AutomationSugar.WaitFor("add_experience_button");
            AutomationSugar.ClickOn("add_experience_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddStaffExperiencedDialog();
        }

        public ServiceAgreementDialog ClickAddServiceAgreement()
        {
            AutomationSugar.WaitFor("add_service_agreement_button");
            AutomationSugar.ClickOn("add_service_agreement_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new ServiceAgreementDialog();
        }

        public BankBuildingSocialDetailDialog ClickBankBuildingSocialDetail()
        {
            AutomationSugar.WaitFor("add_bank/building_society_details_button");
            AutomationSugar.ClickOn("add_bank/building_society_details_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new BankBuildingSocialDetailDialog();
        }

        public AddContractDetailDialog ClickAddContract()
        {
            AutomationSugar.WaitFor("add_contract_button");
            AutomationSugar.ClickOn("add_contract_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddContractDetailDialog();
        }

        public AddStaffContactTripletDialog ClickAddContact()
        {
            AutomationSugar.WaitFor("add_contact_button");
            AutomationSugar.ClickOn("add_contact_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new AddStaffContactTripletDialog();
        }

        public FindTrainingCourseEventDialog ClickTrainingCourse()
        {
            AutomationSugar.WaitFor("add_training_course_button");
            AutomationSugar.ClickOn("add_training_course_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new FindTrainingCourseEventDialog();
        }

        public StaffAbsenceDialog ClickAddAbsence()
        {
            AutomationSugar.WaitFor("add_absence_button");
            AutomationSugar.ClickOn("add_absence_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new StaffAbsenceDialog();
        }

        public void ClickAddStaffAbsence()
        {
            AutomationSugar.WaitFor("add_absence_button");
            AutomationSugar.ClickOn("add_absence_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public void ClickAddStaffRole()
        {
            AutomationSugar.WaitFor("add_staff_role_button");
            AutomationSugar.ClickOn("add_staff_role_button");
            AutomationSugar.WaitForAjaxCompletion();
        }

        public ServiceRecordDialog ClickAddServiceRecord()
        {
            AutomationSugar.WaitFor("add_service_record_button");
            AutomationSugar.ClickOn("add_service_record_button");
            AutomationSugar.WaitForAjaxCompletion();
            return new ServiceRecordDialog();
        }

        #endregion

        #region TrainingHistory Grid

        public GridComponent<TrainingHistoryRow> TrainingHistoryTable
        {

            get
            {
                return new GridComponent<TrainingHistoryRow>(By.CssSelector("[data-maintenance-container='StaffTrainingCourseEnrolments']"), ComponentIdentifier);
            }
        }


        public class TrainingHistoryRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name $= 'TrainingCourseTitle']")]
            private IWebElement _title;

            [FindsBy(How = How.CssSelector, Using = "[name$='AdditionalCosts']")]
            private IWebElement _AdditionalCosts;

            public string Title
            {
                set { _title.SetText(value); }
                get { return _title.GetValue(); }
            }

            public string AdditionalCosts
            {
                get { return _AdditionalCosts.GetValue(); }
                set { _AdditionalCosts.SetText(value); }
            }
        }

        #endregion

        #region Absences Grid

        public GridComponent<AbsencesRow> Absences
        {
            get
            {
                return new GridComponent<AbsencesRow>(By.CssSelector("[data-maintenance-container='StaffAbsences']"), ComponentIdentifier);
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
                get { return _startDateField.GetValue(); }
            }

            public string LastDay
            {
                get { return _endDateField.GetValue(); }
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
            private IWebElement _serviceTermDescription;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='PostTypeDisplay']")]
            private IWebElement _postType;

            [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit..._button']")]
            private IWebElement _editButton;

            public string ServiceTerm
            {
                get { return _serviceTermDescription.GetAttribute("value"); }
            }

            public string PostType
            {
                get { return _postType.GetAttribute("value"); }
            }

            public string StartDate
            {
                get { return _startDate.GetAttribute("value"); }
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

        #region BackgroundCheck Grid

        public GridComponent<ServiceRecord> ServiceRecordTable
        {
            get
            {
                GridComponent<ServiceRecord> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<ServiceRecord>(By.CssSelector("[data-maintenance-container='StaffServiceRecords']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

        public class ServiceRecord : GridRow
        {

            [FindsBy(How = How.CssSelector, Using = "[name$='.DOA']")]
            public IWebElement _doa;

            [FindsBy(How = How.CssSelector, Using = "[name$='.DOL']")]
            public IWebElement _dol;

            public string DOA
            {
                set { _doa.SetText(value); }
                get { return _doa.GetValue(); }
            }

            public string DOL
            {
                set { _dol.SetText(value); }
                get { return _dol.GetValue(); }
            }

            public ServiceRecordDialog ClickEdit()
            {
                AutomationSugar.WaitFor("edit..._button");
                AutomationSugar.ClickOn("edit..._button");
                AutomationSugar.WaitForAjaxCompletion();
                return new ServiceRecordDialog();
            }
        }
        #endregion
    }
}