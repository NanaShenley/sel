using OpenQA.Selenium.Support.PageObjects;
using SharedComponents.Helpers;
using WebDriverRunner.webdriver;

namespace SharedComponents.Popups
{
    public class ConfirmCommitPopupDialog
    {
        private const string CancelCommitChangesPopup = "cancel_commit_dialog";

        private const string DontSaveCommitChangesPopup = "ignore_commit_dialog";

        private const string SaveAndContinuePopup = "save_continue_commit_dialog";

        public ConfirmCommitPopupDialog()
        {
            PageFactory.InitElements(WebContext.WebDriver, this);
        }

        public void CancelConfirmDialog()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(CancelCommitChangesPopup));
        }

        public void DontSaveConfirmDialog()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(DontSaveCommitChangesPopup));
        }

        public void SaveAndContinueDialog()
        {
            SeleniumHelper.WaitForElementClickableThenClick(SeleniumHelper.SelectByDataAutomationID(SaveAndContinuePopup));
        }
    }
}
