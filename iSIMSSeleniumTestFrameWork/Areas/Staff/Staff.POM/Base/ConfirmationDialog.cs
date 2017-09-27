using OpenQA.Selenium;

namespace Staff.POM.Base
{
    public class ConfirmationDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return By.CssSelector("[data-section-id=\"generic-confirm-dialog\"]"); }
        }

        //public BackgroundUpdateDialog ClickOK()
        //{
        //    ClickOk();
        //    return new BackgroundUpdateDialog();
        //}
    }
}
