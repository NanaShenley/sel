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
    public class PromotePupilsPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("allocate_future_pupil_detail"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'well_know_action_save']")]
        private IWebElement _saveButton;

        [FindsBy(How = How.CssSelector, Using = ".webix_ss_body")]
        private IWebElement _promoteTable;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id = 'additional_columns_button']")]
        private IWebElement _additionalColumnButton;

        #endregion

        #region Actions

        public AdditionalColumnDialog ClickAdditionalColumnButton()
        {
            _additionalColumnButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new AdditionalColumnDialog();

        }

        public void Save()
        {
            _saveButton.Click();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        #endregion

        #region Grid

        public WebixComponent<PromoteCell> PromoteTable
        {
            get { return new WebixComponent<PromoteCell>(_promoteTable); }
        }

        public class PromoteCell : WebixCell
        {
            public PromoteCell() { }

            public PromoteCell(IWebElement webElement)
                : base(webElement)
            { }

            public string ValueDropDown
            {
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        Retry.Do(webElement.Click);
                    }
                    SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input")).Click();
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
                        Retry.Do(webElement.Click);
                    }
                    IWebElement editor = SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input"));
                    editor.Click();
                    string value = SeleniumHelper.FindElement(By.CssSelector(".webix_list_item.webix_selected")).GetText();
                    editor.SendKeys(Keys.Tab);
                    return value;
                }
            }

            public string CellText
            {
                set
                {
                    if (!(webElement.GetAttribute("class").Contains("webix_cell_select") &&
                        SeleniumHelper.DoesWebElementExist(By.CssSelector(".webix_dt_editor"))))
                    {
                        webElement.ClickByJS();
                    }
                    IWebElement input = SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input"));
                    input.SetText(value);
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
                        webElement.ClickByJS();
                    }
                    IWebElement input = SeleniumHelper.FindElement(By.CssSelector(".webix_dt_editor input"));
                    return input.GetValue();
                }
            }
        }

        #endregion
    }
}
