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
    public class AddMemberDialogDetail : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".search-results"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private IWebElement _addSelectedButton;

        [FindsBy(How = How.CssSelector, Using = "[title*='Member']")]
        private IList<IWebElement> _memberList;

        public IList<IWebElement> Members
        {
            get 
            {
                try
                {
                    Wait.WaitForElement(SimsBy.CssSelector("[title*='Member'][data-automation-id='resultTile']"));
                    return SeleniumHelper.FindElements(SimsBy.CssSelector("[title*='Member'][data-automation-id='resultTile']"));
                }
                catch (Exception)
                {
                    return null;
                }
                
            }
        }

        public IList<string> MemberList
        {
            get
            {
                IList<string> resultList = new List<string>();
                foreach (var element in _memberList)
                {
                    resultList.Add(element.GetText());
                }
                return resultList;
            }
        }

        #endregion

        #region Actions

        public void AddSelectedMembers()
        {
            _addSelectedButton.ClickByJS();
        }

        #endregion

    }
}
