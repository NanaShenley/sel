using Staff.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using Staff.POM.Helper;

namespace Staff.POM.Components.Staff.Dialogs
{
    public class SharedContactDetailsMatchesDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get
            {
                return SimsBy.AutomationId("shared_contact_detail_matches_dialog");
            }
        }

        public void ClickSave(string automationID)
        {
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SeSugar.Automation.SimsBy.AutomationId(automationID)));
            AutomationSugar.WaitForAjaxCompletion();

            System.Threading.Thread.Sleep(5000);
        }

        public GridComponent<Match> Matches
        {
            get
            {
                return new GridComponent<Match>(By.CssSelector("[data-maintenance-container]"), ComponentIdentifier);
            }
        }

        public class Match : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='.ColumnSelector']")]
            private IWebElement _selected;

            [FindsBy(How = How.CssSelector, Using = "[name$='Name']")]
            private IWebElement _Name;

            [FindsBy(How = How.CssSelector, Using = "[name$='Person Type']")]
            private IWebElement _PersonType;

            [FindsBy(How = How.CssSelector, Using = "[name$='Email Address']")]
            private IWebElement _EmailAddress;

            [FindsBy(How = How.CssSelector, Using = "[name$='Telephone Number']")]
            private IWebElement _TelephoneNumber;

            [FindsBy(How = How.CssSelector, Using = "[name$='Location Type']")]
            private IWebElement _LocationType;

            public bool Selected
            {
                set { _selected.Set(value); }
                get { return _selected.IsChecked(); }
            }

            public string Name
            {
                get { return _Name.GetValue(); }
            }

            public string PersonType
            {
                get { return _PersonType.GetValue(); }
            }

            public string EmailAddress
            {
                get { return _EmailAddress.GetValue(); }
            }

            public string TelephoneNumber
            {
                get { return _TelephoneNumber.GetValue(); }
            }
            public string LocationType
            {
                get { return _LocationType.GetValue(); }
            }
        }
    }
}
