using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;

namespace POM.Components.Calendar
{
    public class CalendarDetailPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return By.CssSelector(".layout-two-column-detail"); }
        }
        
        public By EventTileIdentifier(string automationID)
        {
            return SimsBy.AutomationId(automationID);
        }


        #region Properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='Button_Dropdown']")]        
        private IWebElement _addEventdropdownbutton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_school_event']")]
        private IWebElement _addEventButton;

        #endregion

        #region Actions

        public AddEventDialog AddEvent()
        {
            if (_addEventdropdownbutton.IsExist())
            {
                _addEventdropdownbutton.ClickByJS();
                //if (_addEventButton.IsExist())
                //{
                    _addEventButton.ClickByJS();
                //}
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            return new AddEventDialog();
        }


        #endregion
    }
}
