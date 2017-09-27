using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using System.Linq;

namespace POM.Components.SchoolGroups
{
    public class AddSupervisorsDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public AddSupervisorsDialogTriplet()
        {
            _searchCriteria = new AddSupervisorsDialogSearchPage(this);
        }

        #region Search

        private readonly AddSupervisorsDialogSearchPage _searchCriteria;
        public AddSupervisorsDialogSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion

    }
}
