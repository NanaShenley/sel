using OpenQA.Selenium;

namespace Staff.Components.StaffRegression
{
    public class ConfirmationDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return By.CssSelector("[data-section-id=\"generic-confirm-dialog\"]"); }
        }

        public void ClickOK()
        {
            ClickOk();
        }
    }
}
