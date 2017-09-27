using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Components.Calendar;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facilities.POM.Components.Calendar.Dialogs
{
    public class EventPopoverCard: BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.AutomationId("popover-custom-id"); }
        }

        #region Properties

        [FindsBy(How = How.CssSelector, Using = ".btn-close")]
        private IWebElement _closeButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_button']")]
        private IWebElement _editButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='delete_button']")]
        private IWebElement _deleteButton;

        #endregion

        #region Actions

        public void ClosePopup()
        {
            _closeButton.ClickByJS();
        }

        public AddEventDialog EditEvent()
        {
            _editButton.ClickByJS();
            return new AddEventDialog();
        }
        

        public ConfirmDeleteDialog EventDeleteConfirmation()
        {
            _deleteButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            return new ConfirmDeleteDialog();
        }
        #endregion
    }
}
