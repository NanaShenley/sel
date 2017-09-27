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
    public class AddPupilsDialogDetail : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector(".search-results"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_selected_button']")]
        private IWebElement _addSelectedButton;

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='search_result']")]
        private IList<IWebElement> _pupilList;

        [FindsBy(How = How.CssSelector, Using = "[title*='Pupil']")]
        private IList<IWebElement> _pupilNameList;

        [FindsBy(How = How.CssSelector, Using = "[title='Year Group']")]
        private IList<IWebElement> _yearGroupList;

        [FindsBy(How = How.CssSelector, Using = "[title='Class']")]
        private IList<IWebElement> _classList;

        public IList<IWebElement> Pupils
        {
            get
            {
                try
                {
                    Wait.WaitForElement(SimsBy.CssSelector("[title*='Pupil'][data-automation-id='resultTile']"));
                    return SeleniumHelper.FindElements(SimsBy.CssSelector("[title*='Pupil'][data-automation-id='resultTile']"));
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public IList<string> PupilNameList
        {
            get
            {
                IList<string> resultList = new List<string>();
                foreach (var element in _pupilNameList)
                {
                    resultList.Add(element.GetText());
                }
                return resultList;
            }
        }

        public IList<string> YearGroupList
        {
            get
            {
                IList<string> resultList = new List<string>();
                foreach (var element in _yearGroupList)
                {
                    if (!element.GetText().Trim().Equals(String.Empty))
                    {
                        resultList.Add(element.GetText());
                    }
                }
                return resultList;
            }
        }

        public IList<string> ClassList
        {
            get
            {
                IList<string> resultList = new List<string>();
                foreach (var element in _classList)
                {
                    resultList.Add(element.GetText());
                }
                return resultList;
            }
        }

        #endregion

        #region Actions

        public void AddSelectedPupils()
        {
            _addSelectedButton.ClickByJS();
        }

        #endregion


    }
}
