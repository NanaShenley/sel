using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using Staff.POM.Base;
using Staff.POM.Helper;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class WarningSaveDataDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='confirm-dialog']"); }
        }        

        #region Page properties

        #endregion

        #region Page action

        public void Continue()
        {
            IWebElement _continueButton = null;
            bool isExisted = true;
            try
            {
                _continueButton = SeleniumHelper.FindElement(By.CssSelector("[data-section-id='confirm-Continue']"));
            }
            catch (NoSuchElementException)
            {
                isExisted = false;
            }

            if (isExisted == true)            
            {
                _continueButton.Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));            
            }            
        
        }

        public static WarningSaveDataDialog Create()
        {
            return new WarningSaveDataDialog();
        }
        #endregion

    }
}
