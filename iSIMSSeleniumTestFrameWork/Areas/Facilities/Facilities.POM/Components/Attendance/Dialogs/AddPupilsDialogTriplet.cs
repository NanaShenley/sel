using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class AddPupilsDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='pupil_picker_dialog']"); }
        }

        public AddPupilsDialogTriplet()
        {
            _searchCriteria = new AddPupilsDialogSearchPage(this);
        }

        #region Search

        private readonly AddPupilsDialogSearchPage _searchCriteria;
        public AddPupilsDialogSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

        #region Properties
        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private IWebElement _addSelectedButton;

        #endregion Properties

    }
}
