using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.BaseFolder;
using SharedComponents.Helpers;
using SharedServices.Components.Common;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using WebDriverRunner.webdriver;

namespace SharedServices.Components.AdditionalColumn
{
    public class FuturePupilsAdditionalColumnsDialog : BaseSeleniumComponents
    {
        private int beforeCount, afterCount;

        [FindsBy(How = How.CssSelector, Using = SharedServicesElements.AdditionalColumns.SearchButtonToFind)]
        private IWebElement _searchButton;

        //[FindsBy(How = How.CssSelector, Using = SharedServicesElements.AdditionalColumns.AdditionalColumnButtonToFind)]
        //private IWebElement _additionalColumnButton;

        public IWebElement _additionalColumnButton
        {
            get { return WebContext.WebDriver.FindElement(SeleniumHelper.SelectByDataAutomationID("additional_columns_button")); }
        }

        public void WaitForAdditonalColumn()
        {
            WaitUntilDisplayed(new System.TimeSpan(0, 0, 0, 10), SeleniumHelper.SelectByDataAutomationID("additional_columns_button"));
        }


        [FindsBy(How = How.CssSelector, Using = SharedServicesElements.AdditionalColumns.OkButton)]
        private IWebElement _oKButton;

        public FuturePupilsAdditionalColumnsDialog()
        {
            SeleniumHelper.Login(SeleniumHelper.iSIMSUserType.SchoolAdministrator); 
            SeleniumHelper.NavigateMenu("Tasks", "School Groups", "Allocate Future Pupils");
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void OpenAdditionalColumn()
        {
            IWebElement searchCriteria = SeleniumHelper.Get(SharedServicesElements.CommonElements.SearchCriteria);
            searchCriteria.FindCheckBoxAndClick("Year Group", new List<string> { "Year 1", "Year 2", "Year 3" });

            _searchButton.Click();

            //WaitUntilDisplayed(By.CssSelector(SharedServicesElements.AdditionalColumns.AdditionalColumnButtonToFind));
            //_additionalColumnButton.Click();
            WaitForAdditonalColumn();
            _additionalColumnButton.Click();
           

            SeleniumHelper.FindAndClick(SharedServicesElements.AdditionalColumns.CheckboxClick);
            SeleniumHelper.FindAndClick(SharedServicesElements.AdditionalColumns.AdmissionNumberCheckBox);

            beforeCount = GetCheckedItemCount();

        //    WebContext.Screenshot();
            _oKButton.Click();

        }

        public bool VerifyCheckedColumns()
        {
            //wait until the modal dialog box attribute changes
            WaitUntilEnabled(SharedServicesElements.AdditionalColumns.ModalDialogBox);
            _additionalColumnButton.Click();

            afterCount = GetCheckedItemCount();

            WaitUntilDisplayed(SharedServicesElements.AdditionalColumns.AdmissionNumberCheckBox);

            WebContext.Screenshot();
            _oKButton.Click();

            //for test purpose the count of column is 7
            return beforeCount == afterCount ? true : false;
        }

        private int GetCheckedItemCount()
        {
            //fetch the count of checkboxes which are checked
            IWebElement parent = SeleniumHelper.Get(SharedServicesElements.AdditionalColumns.EditableColumnTreeNode);
            ReadOnlyCollection<IWebElement> checkBoxList = parent.FindElements(SharedServicesElements.AdditionalColumns.CheckBox);
            int count = 0;

            foreach (var item in checkBoxList)
            {
                var checkedValue = item.GetAttribute("checked");

                //Assuming !NULL means checked.
                if (checkedValue != null)
                    count++;
            }
            return count;
        }

    }
}
