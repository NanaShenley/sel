using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class PreviousAddressPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        public class PreviousAddressRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='LearnerAddressesAddress']")]
            private IWebElement _address;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressStatus']")]
            private IWebElement _status;

            [FindsBy(How = How.CssSelector, Using = "[name$='AddressType.dropdownImitator']")]
            private IWebElement _type;

            [FindsBy(How = How.CssSelector, Using = "[name$='StartDate']")]
            private IWebElement _startDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='EndDate']")]
            private IWebElement _endDate;

            [FindsBy(How = How.CssSelector, Using = "[name$='Note']")]
            private IWebElement _note;

            public string Address
            {
                get { return _address.GetText(); }
            }

            public string AddressStatus
            {
                get { return _status.GetAttribute("value"); }
            }

            public string AddressType
            {
                get { return _type.GetAttribute("value"); }
            }

            public string StartDate
            {
	            get { return _startDate.GetDateTime(); }
            }

            public string EndDate
            {
                get { return _endDate.GetDateTime(); }
            }

            public string Note
            {
                get { return _note.GetText(); }
            }
        }

        public GridComponent<PreviousAddressRow> PreviousAddress
        {
            get
            {
                GridComponent<PreviousAddressRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<PreviousAddressRow>(By.CssSelector("[data-maintenance-container='PreviousAddresses']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }

    }
}
