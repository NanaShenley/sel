using POM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using POM.Helper;
using OpenQA.Selenium.Support.PageObjects;

namespace Facilities.POM.Components.SchoolGroups.Dialog
{
    public class AddStructuralGroupDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.Id("dialog-editableData"); }
        }

        #region Page properties

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='ok_button']")]
        private IWebElement _okButton;

        [FindsBy(How = How.Name, Using = "DestinationGroup.ShortName")]
        private IWebElement _shortName;

        [FindsBy(How = How.Name, Using = "DestinationGroup.DisplayOrder")]
        private IWebElement _displayOrder;

       
        public string ShortName
        {
            get { return _shortName.GetValue(); }
            set { _shortName.SetText(value); }
        }

        public int DisplayOrder
        {
            get { return Convert.ToInt32(_displayOrder.GetValue()); }
            set { _displayOrder.SetText(value.ToString()); }
        }


        #endregion

        #region Action
        public void Save()
        {
            SeleniumHelper.Get(SimsBy.AutomationId("ok_button")).ClickByJS();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
        }

        #endregion 
    }
}
