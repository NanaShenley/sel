using System;
using DataExchange.POM.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using DataExchange.POM.Helper;
using WebDriverRunner.webdriver;



namespace DataExchange.POM.Components.PLASC
{
    public class PlascCreateDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get
            {
                return By.Id("screen");
            }
        }

        public PlascCreateDialog()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        [FindsBy(How = How.CssSelector, Using = "form[id='dialog-editableData'] input[name='StatutoryReturnType.dropdownImitator']")]
        private IWebElement _returnTypeDropdown;

        [FindsBy(How = How.CssSelector, Using = "form[id='dialog-editableData'] input[name='StatutoryReturnVersion.dropdownImitator']")]
        private IWebElement _returnTypeVersionDropdown;

        public string ReturnTypeDropdown
        {
            get { return _returnTypeDropdown.GetAttribute("value"); }
            set {  _returnTypeDropdown.EnterForDropDown(value); }
            
        }

        public string ReturnTypeVersionDropdown
        {
            get { return _returnTypeVersionDropdown.GetAttribute("value"); }
            set { _returnTypeVersionDropdown.EnterForDropDown(value); }
        }


        /// <summary>
        /// Handles Ok click flow on create dialog
        /// </summary>
        /// <returns></returns>
        public PlascTripletPage ClickOkButtonAndWaitAjaxForCompletion()
        {
            //ensure ok button is visible
            string jsOkButtonVisiblePredicate = "return $(\"[data-automation-id = 'ok_button']\").is(':visible')";
            Console.WriteLine("Waiting for OK button to be visible");
            Wait.WaitTillConditionIsMet(jsOkButtonVisiblePredicate, 10);

            //click ok
            WebContext.WebDriver.FindElement(By.CssSelector("[data-automation-id='ok_button']")).ClickByJS();
            Wait.WaitTillAllAjaxCallsComplete();

            if(HandleConfirmationDialogIfExist())
            {
                Wait.WaitTillAllAjaxCallsComplete();
            }

            return new PlascTripletPage();
        }

        /// <summary>
        /// Checks if confirmation dialog exists and clicks Ok button to proceed
        /// </summary>
        /// <returns></returns>
        private bool HandleConfirmationDialogIfExist()
        {
            string script = "return $(\"[data-automation-id = 'create_Deni_dialog'] .text-warning\").length";
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)WebContext.WebDriver;
            Int64 elementCount = (Int64)jsExecutor.ExecuteScript(script);
            
            if(elementCount > 0)
            {
                /*Note: with the used selector expression 2 buttons get selected. click the 2nd one*/
                script = "$(\"[data-automation-id = 'create_Deni_dialog']\").find(\"button[data-ajax-url = '/iSIMSMVCClient/StatutoryReturn/CreateStatutoryReturn/SaveReturn']\")[1].click();";
                jsExecutor.ExecuteScript(script);
                return true;
            }
            else
            {
                return false;
            }




        }

        
    }
}
