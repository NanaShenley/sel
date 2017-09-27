using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using System.Linq;

namespace POM.Components.SchoolGroups
{
    public class AddPupilsDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public AddPupilsDialogTriplet()
        {
            _searchCriteria = new AddPupilsDialogSearchPage(this);
        }

        #region Search

        private readonly AddPupilsDialogSearchPage _searchCriteria;
        public AddPupilsDialogSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }
}
