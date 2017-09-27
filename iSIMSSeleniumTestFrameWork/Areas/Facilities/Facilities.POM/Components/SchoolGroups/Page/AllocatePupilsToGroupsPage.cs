using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace POM.Components.SchoolGroups
{
    public class AllocatePupilsToGroupsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("allocate_future_pupil_detail"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.Name, Using = "EffectiveDate")]
        private IWebElement _effectiveDateTextBox;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _allocateTable;

        public string EffectiveDate
        {
            set
            {
                _effectiveDateTextBox.SetDateTimeByJS(value);
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                Refresh();
            }
            get { return _effectiveDateTextBox.GetDateTime(); }
        }

        #endregion

        #region Actions

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        #endregion

        #region Grid

        public WebixComponent<AllocateCell> AllocateTable
        {
            get
            {
                return new WebixComponent<AllocateCell>(_allocateTable);
            }
        }

        public class AllocateCell : WebixCell
        {
            public AllocateCell() { }

            public AllocateCell(IWebElement webElement)
                : base(webElement)
            { }

            public string ValueDropDown
            {
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.ClickUntilAppearElement(By.CssSelector(".webix_dt_editor input"));
                    }
                    SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input")).SetText(value);
                    IList<IWebElement> dropdown = SeleniumHelper.FindElements(By.CssSelector(".webix_list_item")).ToList();
                    foreach (var item in dropdown)
                    {
                        if (item.Text.Equals(value))
                        {
                            item.ClickByJS();
                            break;
                        }
                    }
                }
                get
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.ClickUntilAppearElement(By.CssSelector(".webix_dt_editor input"));
                    }
                    return SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input")).GetValue();
                }
            }
        }

        #endregion
    }
}
