using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;


namespace POM.Components.SchoolGroups
{
    public class AddSupervisorsDialogDetail : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".search-results"); }
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

        public void AddSelectedSupervisor()
        {
            _addSelectedButton.ClickByJS();
        }

        #endregion


    }
}
