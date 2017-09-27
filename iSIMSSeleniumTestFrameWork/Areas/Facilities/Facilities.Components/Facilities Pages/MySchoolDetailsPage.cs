using Facilities.Components.Common;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using SharedComponents.BaseFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.webdriver;

namespace Facilities.Components.Facilities_Pages
{
    public class MySchoolDetailsPage : BaseFacilitiesPage
    {
#pragma warning disable 0649
        [FindsBy(How = How.Id, Using = "editableData")]
        private readonly IWebElement _main;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_new_button']")]
        private IWebElement _addNewButton;

        [FindsBy(How = How.Name, Using = "AddressesAddress")]
        private IWebElement _medicalPracticeAddress;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        private IWebElement _editNewButton;

        [FindsBy(How = How.CssSelector, Using = "[title='Delete address detail']")]
        private IWebElement _deleteAddessButton;

        public void WaitLoading()
        {
            Wait.WaitLoading();
        }

        public void ClickDeleteAddrss()
        {
            Wait.WaitForControl(SimsBy.CssSelector("[title='Delete address detail']"));
            _deleteAddessButton.ClickAndWaitFor(SimsBy.AutomationId("continue_with_delete_button"));
            SeSugar.Automation.AutomationSugar.ClickOn("continue_with_delete_button");
            Wait.WaitLoading();
        }

        public string MedicalPracticeAddresss
        {
            set { _medicalPracticeAddress.SetText(value); }
            get { return _medicalPracticeAddress.GetValue(); }
        }


        public void ClickEditAddrss()
        {
            Wait.WaitForControl(SimsBy.AutomationId("edit_button"));
            _editNewButton.ClickAndWaitFor(SimsBy.AutomationId("find_address_detail")); ;
        }

        public MySchoolDetailsPage()
        {
            WaitUntilDisplayed(MySchoolDetailsElements.Save);
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public IWebElement SchoolSitesBuildingsAccPanel
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='section_menu_School Sites and Buildings']"));
            }
        }

        public IWebElement SchoolAddressPanel
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='section_menu_School Address']"));
            }
        }
        public IWebElement AddSchoolAddressButton
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='add_new_button']"));
            }
        }

        public IWebElement AddSchoolSitebldnglink
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='add_site_and_buildings_button']"));
            }
        }

        public void ClickAddAddrss()
        {
            Wait.WaitForControl(SimsBy.AutomationId("add_new_button"));
            _addNewButton.ClickAndWaitFor(SimsBy.AutomationId("find_address_detail")); ;
        }

        public void ExpandSchoolSitebldng()
        {

            SchoolSitesBuildingsAccPanel.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            WaitForElement(MySchoolDetailsElements.AddschoolsiteLink);
        }

        public void ExpandSchoolAddress()
        {
            SchoolAddressPanel.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            WaitForElement(MySchoolDetailsElements.MySchoolAddress);
        }

        public bool IsAddSchoolAddress()
        {
            try
            {
                if (AddSchoolAddressButton == null)
                {
                    return false;
                }
                return AddSchoolAddressButton.IsElementExists();
            }
            catch {
                return false;
            }
        }

        public AddSchoolSitepopupPage ClickAddSchoolSitebldnglink()
        {
            AddSchoolSitebldnglink.Click();
            WaitUntilDisplayed(MySchoolDetailsElements.Addschoolsitepopup);
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSchoolSitepopupPage();
        }
        public IWebElement EditSchoolDetailsLink
        {
            get
            {
                return _main.FindElement(By.CssSelector("[data-automation-id='edit..._button']"));
            }
        }
        public AddSchoolSitepopupPage ClickEditButton()
        {
            EditSchoolDetailsLink.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AddSchoolSitepopupPage();
        }

    }
}

