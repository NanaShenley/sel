using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;
using System.Linq;
namespace POM.Components.SchoolGroups
{
    public class AddMemberDialogTriplet : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-palette-editor"); }
        }

        public AddMemberDialogTriplet()
        {
            _searchCriteria = new AddMemberDialogSearchPage(this);
        }

        #region Search

        private readonly AddMemberDialogSearchPage _searchCriteria;
        public AddMemberDialogSearchPage SearchCriteria { get { return _searchCriteria; } }

        #endregion
    }
}
