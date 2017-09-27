using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Helper;
using POM.Base;
using POM.Components.Common;


namespace POM.Components.School
{
    public class TeachingMediumSearchPage : SearchTableCriteriaComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-automation-id='search_criteria']"); }
        }

        public TeachingMediumSearchPage(BaseComponent parent) : base(parent) { }

        #region Properties

        [FindsBy(How = How.Name, Using = "CodeOrDescription")]
        private IWebElement _codeAndDescriptionTextBox;

        public string CodeAndDescription
        {
            set { _codeAndDescriptionTextBox.SetText(value); }
            get { return _codeAndDescriptionTextBox.GetText(); }
        }

        #endregion
    }
}
