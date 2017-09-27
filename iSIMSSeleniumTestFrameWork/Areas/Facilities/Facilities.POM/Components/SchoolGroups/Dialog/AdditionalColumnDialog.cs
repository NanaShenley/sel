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
    public class AdditionalColumnDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("editablecolumntreenode"); }
        }

        #region Actions

        public void ClearSelection()
        {
            SeleniumHelper.Get(SimsBy.CssSelector("[data-clear-container-id='editablecolumntreenode']")).Click();
        }

        #endregion
    }
}
