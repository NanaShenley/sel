using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace POM.Components.Attendance
{
    public class AddPupilsDialogSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector(".search-criteria"); }
        }

        public AddPupilsDialogSearchPage(BaseComponent parent) : base(parent) { }

        #region Page properties

        [FindsBy(How = How.Name, Using = "LegalSurname")]
        private IWebElement _pupilName;

        public TreeMenu Classes
        {
            get { return new TreeMenu(); }
        }

        public TreeMenu YearGroups
        {
            get { return new TreeMenu(); }
        }

        #endregion

        #region Actions

        public string SearchByPupilName
        {
            get { return _pupilName.GetValue(); }
            set { _pupilName.SetText(value); }
        }

        #endregion


        #region Class

        public class TreeMenu
        {
            public NodeItem this[string name]
            {
                get
                {
                    var _element = GetClassItem(name);
                    return new NodeItem(_element);
                }
            }

            public IWebElement GetClassItem(string name)
            {
                var _classNodeItems = SeleniumHelper.FindElements(SimsBy.CssSelector(".checkboxlist-column"));

                foreach (var nodeItem in _classNodeItems)
                {
                    string label = String.Empty;
                    try
                    {
                        var labelElement = nodeItem.FindElement(SimsBy.CssSelector("label"));
                        label = labelElement.GetText();
                    }
                    catch (Exception)
                    {

                    }

                    if (label.Trim().Equals(name))
                    {
                        return nodeItem.FindElement(SimsBy.CssSelector("input"));
                    }
                }

                return null;
            }
        }

        public class NodeItem
        {
            private IWebElement _element;

            public NodeItem(IWebElement e)
            {
                _element = e;
            }

            public bool Select
            {
                set { _element.Set(value); }
                get { return _element.IsChecked(); }
            }

        }

        #endregion
     
    }
}
