using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Pupil
{
    public class AccommodationTypeDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[id^='ui-id'][data-section-id='LearnerTravellersDetails-grid-editor-dialog']"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_accommodation_type_button']")]
        private IWebElement _addAccomodationTypeButton;

        public void AddNewAccomodationType()
        {
            _addAccomodationTypeButton.ClickByJS();
            Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            Refresh();
        }

        #region Accommodation Grid

        public GridComponent<Accommodation> Accommodations
        {
            get
            {
                GridComponent<Accommodation> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<Accommodation>(By.CssSelector("[data-maintenance-container='LearnerTravellersDetails']"), DialogIdentifier);
                });
                return returnValue;
            }
        }

        public class Accommodation
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='TravellerAccommodationType.dropdownImitator']")]
            private IWebElement _accommodationTypeDropDown;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDateField;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDateField;

            public string AccommodationType
            {
                set
                {
                    _accommodationTypeDropDown.ChooseSelectorOption(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _accommodationTypeDropDown.GetValue();
                }
            }
            public string StartDate
            {
                set
                {
                    _startDateField.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _startDateField.GetDateTime();
                }
            }
            public string EndDate
            {
                set
                {
                    _endDateField.SetDateTime(value);
                    Wait.WaitForAjaxReady(By.CssSelector(".locking-mask-loading"));
                }
                get
                {
                    return _endDateField.GetDateTime();
                }
            }
        }

        #endregion
    }
}
