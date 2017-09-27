using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace CommunicationLog.Components.Component.Dialogs
{
    public class AddLogDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("add_communicationlog_detail"); }
        }

        [FindsBy(How = How.Name, Using = "Date")]
        private IWebElement _datePicker;

        [FindsBy(How = How.Name, Using = "Description")]
        private IWebElement _descriptionTextBox;

        [FindsBy(How = How.CssSelector, Using = "input[name='MessageFormatTypeLookup.dropdownImitator']")]
        private IWebElement _messageFormatTypeDropdown;

        [FindsBy(How = How.CssSelector, Using = "input[name='Category.dropdownImitator']")]
        private IWebElement _categoryDropdown;

        //save_button
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='save_button']")]
        private IWebElement _saveButton;

        public string Date
        {
            set { _datePicker.SetText(value); }
            get { return _datePicker.GetValue(); }
        }

        public string Description
        {
            set { _descriptionTextBox.SetText(value); }
            get { return _descriptionTextBox.GetValue(); }
        }

        public void SetMessageFormatType()
        {
            _messageFormatTypeDropdown.Clear();
            _messageFormatTypeDropdown.EnterForDropDown("Letter");
            //_messageFormatTypeDropdown.SendKeys(Keys.Enter);
            
        }

        public void SetCategory()
        {
            _categoryDropdown.Clear();
            //_categoryDropdown.SendKeys(Keys.Enter);
            _categoryDropdown.EnterForDropDown("General");
        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
        }
    }
}
