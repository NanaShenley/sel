using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class MedicalPracticeSearchDialog : SearchCriteriaComponent<MedicalPracticeTripletDialog.MedicalPracticeSearchResultTile>
    {
        public MedicalPracticeSearchDialog(BaseDialogComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "Name")]
        private IWebElement _name;

        public string MedicalPracticeName
        {
            set { _name.SetText(value); }
            get { return _name.GetAttribute("value"); }
        }

        #endregion
    }
}
