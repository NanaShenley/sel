using POM.Base;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;
using POM.Components.Common;

namespace Facilities.POM.Components.SchoolManagement.Page
{
    public class CreatedAcademicYearPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("editableData"); }
        }

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='edit_academic_year_button']")]
        private IWebElement _editAcademicYearButton;

        public NewAcademicYearDetailPage EditButton()
        {
            _editAcademicYearButton.ClickByJS();
            return new NewAcademicYearDetailPage();
        }
    }
}
