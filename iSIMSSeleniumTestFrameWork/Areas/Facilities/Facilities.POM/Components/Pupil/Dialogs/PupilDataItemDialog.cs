using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class PupilDataItemDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("bulk_update_pupil_basic_details_data_items_content"); }
        }

        #region Page properties

        [FindsBy(How = How.Id, Using = "data-items-ok-button-id")]
        private IWebElement _okButton;

        [FindsBy(How = How.CssSelector, Using = "[webix_tm_id='Learner.ModeOfTravel'] input")]
        private IWebElement _modeOfTravelCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[webix_tm_id='Learner.ServiceChildren'] input")]
        private IWebElement _serviceChildrenCheckbox;

        [FindsBy(How = How.CssSelector, Using = "[webix_tm_id='Learner.ServiceChildrenSource'] input")]
        private IWebElement _serviceChildrenSourceCheckbox;

        public bool ModeOfTravel
        {
            set { _modeOfTravelCheckbox.Set(value); }
        }

        public bool ServiceChildren
        {
            set { _serviceChildrenCheckbox.Set(value); }
        }

        public bool ServiceChildrenSource
        {
            set { _serviceChildrenSourceCheckbox.Set(value); }
        }

        #endregion

        #region Public actions

        public BulkUpdatePage ClickOK()
        {
            _okButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            return new BulkUpdatePage();
        }

        #endregion
    }
}
