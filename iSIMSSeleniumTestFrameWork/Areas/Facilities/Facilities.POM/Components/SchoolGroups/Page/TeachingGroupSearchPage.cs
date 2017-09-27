using POM.Base;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium;

namespace POM.Components.SchoolGroups
{
    public class TeachingGroupSearchPage : SearchCriteriaComponent<TeachingGroupTriplet.TeachingGroupResultTile>
    {
        public TeachingGroupSearchPage(BaseComponent parent) : base(parent) { }

        #region Search roperties

        [FindsBy(How = How.Name, Using = "FullName")]
        private IWebElement _groupFullName;


        [FindsBy(How = How.Name, Using = "ShortName")]
        private IWebElement _groupShortName;

        [FindsBy(How = How.Id, Using = "tri_chkbox_IsVisibleTemp")]
        private IWebElement _visibilityCheckbox;

        [FindsBy(How = How.Name, Using = "AssessmentSubject.dropdownImitator")]
        private IWebElement _subject;

        public string GroupFullName
        {
            set { _groupFullName.SetText(value); }
            get { return _groupFullName.GetValue(); }
        }

        public string GroupShortName
        {
            set { _groupShortName.SetText(value); }
            get { return _groupShortName.GetValue(); }
        }

        public string Subject
        {
            set { _subject.EnterForDropDown(value); }
            get { return _subject.GetValue(); }
        }

        public bool Visibility
        {
            set { _visibilityCheckbox.Set(value); }
            get { return _visibilityCheckbox.IsCheckboxChecked(); }
        }

        #endregion

    }
}
