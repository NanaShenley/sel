using OpenQA.Selenium;
using POM.Base;
using POM.Helper;


namespace POM.Components.Common
{
    public class ConfirmRequiredDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {

            get { return SimsBy.CssSelector("[data-section-id='generic-confirm-dialog']"); }
        }

        public static T Confirm<T>() where T : BaseComponent, new()
        {
            if (SeleniumHelper.DoesWebElementExist(SimsBy.CssSelector("[data-automation-id='ok_button']")) &&
                SeleniumHelper.FindElement(SimsBy.CssSelector("[data-automation-id='ok_button']")).Displayed)
            {
                SeleniumHelper.FindElement(SimsBy.CssSelector("[data-automation-id='ok_button']")).Click();
                Wait.WaitForAjaxReady(By.CssSelector(".locking-mask"));
            }
            var page = default(T);
            Retry.Do(() =>
            {
                page = new T();
            });
            page.Refresh();
            return page;
        }
    }
}
