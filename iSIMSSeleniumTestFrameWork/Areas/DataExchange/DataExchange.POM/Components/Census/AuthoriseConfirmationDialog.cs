using DataExchange.POM.Base;
using DataExchange.POM.Helper;
using OpenQA.Selenium;

namespace DataExchange.POM.Components.Census
{
    public class AuthoriseConfirmationDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("statutory_return_dialog"); }
        }

        public void Clickok()
        {
            ClickOk();
        }
    }
}
