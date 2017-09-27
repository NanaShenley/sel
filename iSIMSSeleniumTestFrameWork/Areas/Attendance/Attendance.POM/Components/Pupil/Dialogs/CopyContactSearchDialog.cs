using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class CopyContactSearchDialog : SearchCriteriaComponent<CopyContactTripletDialog.CopyContactSearchResultTile>
    {
        public CopyContactSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _nameTextBox;

        public string LegalSurname
        {
            set { _nameTextBox.SetText(value); }
            get { return _nameTextBox.GetValue(); }
        }

        #endregion

    }
}
