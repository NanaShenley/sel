using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace SharedComponents.Popups
{
    public class ConfirmationPopupDialogPage
    {
        private const string ConfirmationPopupDialogOkButton = "ok_button";
        
        public ConfirmationPopupDialogPage()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void ClickOkButton()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(ConfirmationPopupDialogOkButton));
        }
    }
}
