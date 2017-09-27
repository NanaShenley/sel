using Assessment.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using System;
using System.Collections.Generic;
using TestSettings;
using WebDriverRunner.webdriver;
using SeSugar.Automation;
using System.Threading;

namespace Assessment.Components
{
    public class AssessmentMarksheetDetail : BaseSeleniumComponents
    {
        private static readonly string UrlUndertest = Configuration.GetSutUrl();
        private static readonly string Testuser = TestDefaults.Default.TestUser;
        public static readonly string Password = TestDefaults.Default.TestUserPassword;
        private static readonly string SchoolName = TestDefaults.Default.SchoolName;
        private static readonly int TenantId = TestDefaults.Default.TenantId;

        private string MenuLinkText = "Marksheets";

        [FindsBy(How = How.CssSelector, Using = "[data-show-distribution-chart ='']")]
        private IWebElement _distributionGraphPopover;

        [FindsBy(How = How.CssSelector, Using = "[class*=\"form-control formula-box\"]")]
        private IWebElement _formulaBar;

        [FindsBy(How = How.CssSelector, Using = "[data-formula-bar-next='']")]
        private IWebElement _formulaBarNext;
        public const string _formulaBarPreviousString = "[data-formula-bar-previous='']";
        [FindsBy(How = How.CssSelector, Using = _formulaBarPreviousString)]
        private IWebElement _formulaBarPrevious;

        private const string groupMembership = "[data-marksheet-toolbar='']";
        [FindsBy(How = How.CssSelector, Using = groupMembership)]
        private IWebElement _GroupMembership;

        private string _GroupMembershipHyperlink = "a";
        private string _GroupMembershipEffectiveDate = "[name='EffectiveDate']";
        private string _GroupMembershipApply = "[data-assessment-date-range='']";


        private string _dropDownSelectedValue = "[class='dropdown-form-view-description']";


        [FindsBy(How = How.CssSelector, Using = "[data-dropdown-radio='']")]
        private IWebElement _AssessmentYearControls;

        [FindsBy(How = How.CssSelector, Using = "[data-apply-marksheet-filters='']")]
        private IWebElement _applyfilter;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='clear_all_applied_filters']")]
        private IWebElement _clearAllfilters;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio']")]
        private IWebElement _AssessmentYear;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio_Description']")]
        private IWebElement _AssessmentYearSelectedValue;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_DropdownRadio_Button']")]
        private IWebElement _AssessmentYearShowMoreLess;

        private string _dropDownSelectedValueTemp = "[class='dropdown-form-view-description ']";

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'validation-summary-errors')]")]
        private IWebElement _ValidationMessage;

        [FindsBy(How = How.CssSelector, Using = "[data-action-show-search='']")]
        public IWebElement _marksheetSearchElement;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='MarksheetDescription']")]
        public IWebElement _marksheetDescription;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Extended_Dropdown']")]
        public IWebElement _extendedDropDownElement;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='section_menu_Class']")]
        public IWebElement _ClassGroupSearchElement;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_criteria_submit']")]
        public IWebElement _SearchCriteriaButtonElement;

        [FindsBy(How = How.Id, Using = "toolbar_singleview")]
        public IWebElement _SingleViewMode;

        [FindsBy(How = How.Id, Using = "toolbar_multiview")]
        public IWebElement _OverviewMode;

        [FindsBy(How = How.CssSelector, Using = "button[name='filterViewBy']")]
        private IWebElement filter_column;

        [FindsBy(How = How.CssSelector, Using = "[data-dropdown-container='']")]
        private IWebElement filter_dropdown;

        [FindsBy(How = How.Name, Using = "AssessmentSubjects.dropdownImitator")]
        private IWebElement _assignSubjectFilterCombobox;

        [FindsBy(How = How.CssSelector, Using = "[data-flag-id='#ShowAdvanced']")]
        private IWebElement ShowAdvanced;

        [FindsBy(How = How.CssSelector, Using = "span[data-automation-id='search_results_counter']")]
        private IWebElement _searchResultCounter;

        public static readonly By DefaultViewColumn = By.CssSelector("[data-automation-id='Default']");
        public static readonly By CompactViewColumn = By.CssSelector("[data-automation-id='Compact']");

        public static readonly By VerticalOrientation = By.CssSelector("[data-automation-id='Vertical']");
        public static readonly By HorizontalOrientation = By.CssSelector("[data-automation-id='Horizontal']");

        WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
        public AssessmentMarksheetDetail(SharedComponents.Helpers.SeleniumHelper.iSIMSUserType userType = SharedComponents.Helpers.SeleniumHelper.iSIMSUserType.AssessmentCoordinator)
        {
            WebContext.WebDriver.Manage().Window.Maximize();
            PageFactory.InitElements(WebContext.WebDriver, this);
            SeleniumHelper.Login(userType);
            CommonFunctions.GotToMarksheetMenu();
            WaitUntillAjaxRequestCompleted();
        }

        public string GetValidationMessage()
        {
            return _ValidationMessage.Text;
        }

        #region Marksheet Search Group Filter
        public AssessmentMarksheetDetail ClickMarksheetClassGroupFilter()
        {
            WaitForElement(By.CssSelector("[data-automation-id='section_menu_Class']"));
            _ClassGroupSearchElement.Click();
            return this;
        }
        public AssessmentMarksheetDetail CheckClassGroup()
        {
            WaitForElement(By.CssSelector("[name='Classes.SelectedIds']"));
            IEnumerable<IWebElement> elements = WebContext.WebDriver.FindElements(By.CssSelector("[name='Classes.SelectedIds']"));

            foreach (IWebElement webElement in elements)
            {
                webElement.Click();
            }
            //_ClassGroupSearchElement.Click();
            return this;
        }

        public AssessmentMarksheetDetail selectFilterOptions()
        {
            WaitForElement(By.CssSelector("[data-dropdown-container='']"));
            IEnumerable<IWebElement> elements = WebContext.WebDriver.FindElements(By.CssSelector("[data-dropdown-container='']"));

            foreach (IWebElement webElement in elements)
            {
                if (webElement.Text == "Assessment Period")
                {
                    webElement.Click();
                    Thread.Sleep(1000);
                    MarksheetGridHelper.PerformEnterKeyBehavior();
                    Thread.Sleep(1000);
                }
            }
            return this;
        }

        public AssessmentMarksheetDetail ClickAdditionalColumnSwitch()
        {
            //Thread.Sleep(1000);
            WaitForElement(By.CssSelector("[class='switch-label-control']"));
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector("[class='switch-label-control']"));

            element.Click();
            return this;

        }

        public AssessmentMarksheetDetail ClickSearchCriteriaButton()
        {
            WaitForElement(By.CssSelector("[data-action-show-search='']"));
            _SearchCriteriaButtonElement.Click();
            return this;
        }

        public AssessmentMarksheetDetail clickFilterbutton()
        {
            Thread.Sleep(1000);
            //WaitForElement(By.CssSelector("button[name='filterViewBy']")); 
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector("button[name='filterViewBy']"));

            element.Click();
            return this;
        }

        public AssessmentMarksheetDetail ClickAdditionalColumnDetails()
        {
            //Thread.Sleep(1000);
            AdditionalColumns additionalColumn = new AdditionalColumns();
            additionalColumn.ClickAdditionalColumns();

            bool checkedAlready = additionalColumn.SelectAdditonalColumnForFilter("Term of Birth");
            additionalColumn.ClickOk();
            WaitUntillAjaxRequestCompleted();
            return this;
        }


        #endregion

        #region Open/Search Marksheets
        public AssessmentMarksheetDetail EnterSearchMarksheet(string marksheetName)
        {
            return this;
        }

        public AssessmentMarksheetDetail ClickSearchMarksheet()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-action-show-search='']"));
            //_marksheetSearchElement.Click();
            return this;
        }

        public AssessmentMarksheetDetail ClickApplyFilter()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-apply-marksheet-filters='']"));
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        public AssessmentMarksheetDetail ClickClearAllFilter()
        {
            Thread.Sleep(1000);
            WaitForElement(By.CssSelector("[data-automation-id='clear_all_applied_filters']"));
            IWebElement element = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='clear_all_applied_filters']"));
            element.Click();
            return this;
        }

        public AssessmentMarksheetDetail OpenMarksheet(string marksheetName)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.LinkText(marksheetName));
            //var wait = new WebDriverWait(WebContext.WebDriver, BrowserDefaults.TimeOut);
            //WaitLogic(wait, By.LinkText(marksheetName));

            //var element = WebContext.WebDriver.FindElement(By.LinkText(marksheetName));
            //element.SendKeys(Keys.Enter);

            WaitUntillAjaxRequestCompleted();
            return this;
        }
        #endregion

        public AssessmentMarksheetDetail ClickMarksheetDescriptionIcon()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='MarksheetDescription']"));
            //_marksheetDescription.Click();
            return this;
        }

        #region Compact / Default View
        public AssessmentMarksheetDetail ClickMarksheetExtendedDropDownIcon()
        {
            _extendedDropDownElement.Click();
            return this;
        }

        public AssessmentMarksheetDetail ClickDefaultView()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(DefaultViewColumn));
            WebContext.WebDriver.FindElement(DefaultViewColumn).Click();
            return this;

        }

        public AssessmentMarksheetDetail ClickCompactView()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(CompactViewColumn));
            WebContext.WebDriver.FindElement(CompactViewColumn).Click();
            return this;
        }

        public string CheckStateofColumn()
        {
            string state = "";
            string returnvalue = "";
            state = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='Default']")).FindChild(By.CssSelector("[name='IsColumnNarrowed']")).GetAttribute("checked");

            if (state == "true")
            {
                returnvalue = "Default";
            }
            else
            {
                returnvalue = "Compact";
            }
            return returnvalue;
        }
        #endregion

        #region Cell Navigation Orientation
        public AssessmentMarksheetDetail ClickMarksheetCellNavigationDropdown()
        {
            var cellNavigator = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='cell_navigator']"));
            cellNavigator.Click();
            return this;
        }

        public bool CheckStateofCellNavigation()
        {
            string state = "";
            //state = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='Horizontal']")).FindChild(By.CssSelector("[name='IsVertical']")).GetAttribute("checked");
            var vertical = WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='Vertical']"));
            state = vertical.FindChild(By.CssSelector("[name='Orientation']")).GetAttribute("checked");
            return Convert.ToBoolean(state);
        }

        public AssessmentMarksheetDetail ClickHorizontalOrientation()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(HorizontalOrientation));
            WebContext.WebDriver.FindElement(HorizontalOrientation).Click();
            return this;

        }

        public AssessmentMarksheetDetail ClickVerticalOrientation()
        {
            WebDriverWait waiter = new WebDriverWait(WebContext.WebDriver, TimeSpan.FromSeconds(MarksheetConstants.Timeout));
            waiter.Until(ExpectedConditions.ElementExists(VerticalOrientation));
            WebContext.WebDriver.FindElement(VerticalOrientation).Click();
            return this;
        }

        #endregion

        #region Group Membership
        public AssessmentMarksheetDetail OpenGroupMembership()
        {
            WaitUntilDisplayed(By.CssSelector("[data-marksheet-toolbar='']"));
            IWebElement hyperlink = _GroupMembership.FindChild(By.CssSelector(_GroupMembershipHyperlink));
            waiter.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector(_GroupMembershipHyperlink)));
            hyperlink.Click();
            return this;
        }

        public AssessmentMarksheetDetail GroupMembershipEnterEffectiveDate(string effectiveDate)
        {
            _GroupMembership.FindChild(By.CssSelector(_GroupMembershipEffectiveDate)).Clear();
            _GroupMembership.FindChild(By.CssSelector(_GroupMembershipEffectiveDate)).SendKeys(effectiveDate);
            return this;
        }

        public AssessmentMarksheetDetail GroupMembershipApply()
        {
            _GroupMembership.FindChild(By.CssSelector(_GroupMembershipApply)).Click();
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        public string GroupMembershipSelectedValue()
        {
            WaitUntilDisplayed(By.CssSelector(groupMembership));
            return _GroupMembership.FindElement(By.CssSelector(_dropDownSelectedValue)).Text;
        }
        #endregion

        #region AssessmentYear
        public AssessmentMarksheetDetail OpenAssessmentYear()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='Button_DropdownRadio']"));
            return this;
        }

        public AssessmentMarksheetDetail AssessmentYearShowMoreLess()
        {
            _AssessmentYearShowMoreLess.Click(); ;
            return this;
        }

        public string AssessmentYearSelectedValue()
        {
            WaitUntilDisplayed(BrowserDefaults.TimeOut, By.CssSelector("[data-automation-id='Button_DropdownRadio_Description']"));
            return _AssessmentYearSelectedValue.Text;
        }
        #endregion

        #region Additional Columns

        public AdditionalColumns OpenAdditionalColumns()
        {
            AdditionalColumns addcolumn = new AdditionalColumns();
            addcolumn.ClickAdditionalColumns();
            return addcolumn;

        }

        #endregion

        public AssessmentMarksheetDetail ClickColumnCaret(int columnNumber)
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.CssSelector("td[column=\"" + columnNumber.ToString() + "\"] > div > i[class = 'fa fa-angle-down fa-fw']"));
            return this;
        }

        public AssessmentMarksheetDetail ShowMoreFilters()
        {
            ShowAdvanced.Click();
            return this;
        }

        public AssessmentMarksheetDetail OpenColumnDetails()
        {
            return this;
        }

        public ViewDistribution ClickViewDistribution(int columnNumber)
        {
            IWebElement sortlink = WebContext.WebDriver.FindElement(By.CssSelector("td[column=\"" + columnNumber.ToString() + "\"] > div > i[data-grid-id='marksheetGrid']"));
            string columnId = sortlink.GetAttribute("data-sort-asc-indicator");

            var columnDetailsDiv = BaseSeleniumComponents.WaitForAndGet(BrowserDefaults.TimeOut, By.CssSelector("[data-menu-column-id=\"" + columnId + "\"]"));
            _distributionGraphPopover.Click();
            return new ViewDistribution();
        }

        #region Formula Bar
        public AssessmentMarksheetDetail FormulaBarClick()
        {
            _formulaBar.Click();
            return this;
        }

        public AssessmentMarksheetDetail EnterInFormulaBar(string keyText)
        {
            _formulaBar.Clear();
            _formulaBar.SendKeys(keyText);
            return this;
        }

        public string GetFormulaBarText()
        {
            return _formulaBar.GetAttribute("value");
        }

        public bool IsFormulaBarPreviousNextSeen()
        {
            return (_formulaBarPrevious != null && _formulaBarPrevious != null);
        }

        public bool IsFormulaBarPreviousNextDisabled()
        {
            return (this.IsFormulaBarPreviousDisabled() && this.IsFormulaBarNextDisabled());
        }

        public bool IsFormulaBarPreviousDisabled()
        {
            WaitForElement(By.CssSelector(_formulaBarPreviousString));
            return (_formulaBarPrevious.GetAttribute("disabled") == "true");
        }

        public AssessmentMarksheetDetail FormulaBarPreviousClick()
        {
            _formulaBarPrevious.Click();
            return this;
        }

        public bool IsFormulaBarNextDisabled()
        {
            return (_formulaBarNext.GetAttribute("disabled") == "true");
        }

        public AssessmentMarksheetDetail FormulaBarNextClick()
        {
            _formulaBarNext.Click();
            return this;
        }

        public AssessmentMarksheetDetail SingleViewClick()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.Id("toolbar_singleview"));
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        public AssessmentMarksheetDetail OverviewClick()
        {
            WaitForAndClick(BrowserDefaults.TimeOut, By.Id("toolbar_multiview"));
            WaitUntillAjaxRequestCompleted();
            return this;
        }

        #endregion

        public string SubjectFilter
        {
            set { _assignSubjectFilterCombobox.EnterForDropDown(value); }
            get { return _assignSubjectFilterCombobox.GetText(); }
        }

        public string NumberofSearchResults
        {
            get { return _searchResultCounter.Text; }
        }
    }
}
