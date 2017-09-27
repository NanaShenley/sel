using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil.Dialogs
{
    public class PupilPickerDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-dialog-palette-editor"); }
        }

        public PupilPickerDialog()
        {
            _searchCriteria = new PupilPickerSearchPage(this);
        }

        private readonly PupilPickerSearchPage _searchCriteria;
        public PupilPickerSearchPage SearchCriteria { get { return _searchCriteria; } }
        
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private readonly IWebElement PupilPickerOkButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private readonly IWebElement AddSelectedPupilButton;
        
        public void AddSelectedPupil()
        {
            AddSelectedPupilButton.Click();
        }

        public void ClickOk()
        {
            PupilPickerOkButton.Click();

            // Wait for Pupil row to appear
            Wait.WaitForElementDisplayed(By.CssSelector("[data-row-name='LearnerAchievementEvents']"));
        }

        public class PupilPickerSearchResultTile : SearchResultTileBase
        {
            [FindsBy(How = How.CssSelector, Using = "[title='Name']")]
            private IWebElement _name;

            public string Name
            {
                get { return _name.Text; }
            }
        }
    }
}
