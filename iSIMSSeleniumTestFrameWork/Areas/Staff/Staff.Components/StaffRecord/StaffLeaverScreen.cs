using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using Staff.Components.StaffRegression;

namespace Staff.Components.StaffRecord
{
    public class StaffLeaverScreen : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("staff_leaving_maintenance"); }
        }

        #region By Strings

        #region Fields

        private const string _dateOfLeavingBy = "[name=\"DOL\"]";
        private const string _staffReasonForLeavingBy = "[name=\"StaffReasonForLeaving.dropdownImitator\"]";
        private const string _nextEmployerBy = "[name='NextEmployer']";

        #endregion

        #region Buttons

        private const string _saveButtonBy = "a[title=\"Save Record\"]";

        #endregion

        #endregion

        #region Web Elements

        #region Fields

        [FindsBy(How = How.CssSelector, Using = _dateOfLeavingBy)]
        public IWebElement DateOfLeaving;

        [FindsBy(How = How.CssSelector, Using = _staffReasonForLeavingBy)]
        public IWebElement StaffReasonForLeaving;

        [FindsBy(How = How.CssSelector, Using = _nextEmployerBy)]
        public IWebElement NextEmployer;

        #endregion

        #region Buttons

        [FindsBy(How = How.CssSelector, Using = _saveButtonBy)]
        public IWebElement SaveButton;

        #endregion

        #endregion

        #region Actions

        public void EnterDOL(string value)
        {
            DateOfLeaving.Clear();
            DateOfLeaving.SendKeys(value);
        }

        public void SelectReasonForLeaving(string value)
        {
            By loc = By.CssSelector(_staffReasonForLeavingBy);
            BaseSeleniumComponents.WaitForAndGet(loc);
            StaffReasonForLeaving.ChooseSelectorOption(value);
        }

        public void EnterNextEmployer(string value)
        {
            NextEmployer.Clear();
            NextEmployer.SendKeys(value);
        }

        public ConfirmationDialog Save()
        {
            SaveButton.Click();
            return new ConfirmationDialog();
        }

        #endregion
    }
}
