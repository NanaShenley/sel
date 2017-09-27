using OpenQA.Selenium;
using SeSugar.Automation;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil.Dialogs
{
    public class SharedAddressDetailsMatchesDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get
            {
                return SeSugar.Automation.SimsBy.AutomationId("co_resident_matches_dialog");
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

            [FindsBy(How = How.CssSelector, Using = "[name$='Address']")]
            private IWebElement _Address;

            [FindsBy(How = How.CssSelector, Using = "[name$='Address Type']")]
            private IWebElement _AddressType;

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
                get { return _Address.GetValue(); }
            }
            public string LocationType
            {
                get { return _AddressType.GetValue(); }
            }
        }
    }
}
