using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class AddPupilsDetailsPage : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".search-results"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private IWebElement _addSelectedButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_all_button']")]
        private IWebElement _addAllButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
        private IList<IWebElement> _pupilList;

        public IList<IWebElement> Pupils
        {
            get { return _pupilList; }
        }

        #endregion

        #region Actions

        public void AddSelectedPupils()
        {
            _addSelectedButton.ClickByJS();
        }

        public void AddAllPupils()
        {
            _addAllButton.ClickByJS();
        }

        #endregion

     
    }
}
