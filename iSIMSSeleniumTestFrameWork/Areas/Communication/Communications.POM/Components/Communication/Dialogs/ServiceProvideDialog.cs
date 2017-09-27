using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Communication
{
    public class ServiceProvideDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='dialog-detail']"); }
        }

        #region Properties 

        [FindsBy(How = How.CssSelector, Using = "[data-automation-id='add_record_button']")]
        private IWebElement _createButton;

        [FindsBy(How = How.CssSelector, Using = "[name='add_record_button']")]
        private IList<IWebElement> _serviceTypeList;

        private string _serviceTypeId;

        public string ServiceProvide
        {
            set
            {
                var _serviceType = SeleniumHelper.Get(SimsBy.AutomationId(value));
                _serviceType.Set(true);
            }
        } 

        public string ServiceTypeId{
            set{
                _serviceTypeId = value;
            }
            get { return _serviceTypeId; }
        }
        #endregion

        #region Actions
        public static AgentDetailPage CreateNewRecord()
        {
            //_createButton.Click();
            Wait.WaitForAjaxReady(SimsBy.CssSelector(".locking-mask"));
            SeleniumHelper.Sleep(5);
            return AgentDetailPage.Create();
        }

         public bool IsServiceTypeExisted()
        {
            bool isExisted = true;
            try
            {
                isExisted = SeleniumHelper.IsElementExists(SimsBy.AutomationId(ServiceTypeId));
            
            }
            catch (Exception)
            {
                isExisted = false;
            }
            return isExisted;
        }

        public static bool IsServiceTypeExistedNew(String ServiceTypeId)
        {
            bool isExisted = true;
            try
            {
                isExisted = SeleniumHelper.IsElementExists(SimsBy.AutomationId(ServiceTypeId));

            }
            catch (Exception)
            {
                isExisted = false;
            }
            return isExisted;
        }


        #endregion
    }
}
