using OpenQA.Selenium;
using SeSugar.Automation;


namespace Staff.POM.Components.Staff
{
    public class ConfirmationDialog : ConfirmRequiredDialog
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("confirmation_required_dialog"); }
        }
    }
}
