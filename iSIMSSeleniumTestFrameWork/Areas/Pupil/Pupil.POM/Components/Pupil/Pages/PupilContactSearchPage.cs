using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;
using POM.Components.Common;

namespace POM.Components.Pupil
{
    public class PupilContactSearchPage : SearchCriteriaComponent<PupilSearchTriplet.PupilSearchResultTile>
    {
        public PupilContactSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "Surname")]
        private IWebElement _surname;

        [FindsBy(How = How.Name, Using = "Forename")]
        private IWebElement _forename;

        [FindsBy(How = How.Name, Using = "NameSearchText")]
        private IWebElement _contactName;

        public string Surname
        {
            set { _surname.SetText(value); }
            get { return _surname.GetValue(); }
        }

        public string Forename
        {
            set { _forename.SetText(value); }
            get { return _forename.GetValue(); }
        }
        
       public string  ContactName
        {
            set { _contactName.SetText(value); }
            get { return _contactName.GetValue(); }
        }

        #endregion

    }
}
