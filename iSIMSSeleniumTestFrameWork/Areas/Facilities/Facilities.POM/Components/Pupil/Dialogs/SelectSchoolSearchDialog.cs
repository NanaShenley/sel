using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class SelectSchoolSearchDialog : SearchCriteriaComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile>
    {
        public SelectSchoolSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _name;

        public string SchoolName
        {
            set { _name.SetText(value); }
            get { return _name.GetAttribute("value"); }
        }

        #endregion

    }
}
