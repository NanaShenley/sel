using POM.Base;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;

namespace Facilities.POM.Components.SchoolGroups.Dialog
{
    public class AddSourceClassDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".modal-content"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private IWebElement _addSelectedButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='resultTile']")]
        private IList<IWebElement> _resultList;

        public IList<IWebElement> SearchResult
        {
            get { return _resultList; }
        }

        public IList<string> SearchResultName
        {
            get
            {
                IList<string> result = new List<string>();
                foreach (var element in SearchResult)
                {
                    result.Add(element.GetText());
                }
                return result;
            }
        }

        #endregion

        #region Actions

        public void AddSelectedClass()
        {
            _addSelectedButton.ClickByJS();
        }


        public void OkButton()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("ok_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion


    }
}
