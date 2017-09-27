using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SeSugar.Automation;
using Staff.POM.Helper;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Staff.POM.Base
{
    public abstract class BaseDialogComponent : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return DialogIdentifier; }
        }

        public abstract By DialogIdentifier { get; }

        #region Buttons

        public virtual void ClickOk(bool waitForStale = false)
        {
            AutomationSugar.ClickOn(new ByChained(DialogIdentifier, SeSugar.Automation.SimsBy.AutomationId("ok_button")));
            if (waitForStale)
                AutomationSugar.WaitUntilStale(this.ComponentIdentifier);
            AutomationSugar.WaitForAjaxCompletion();

            // This is to allow merge change scope to complete - Sat working on a longer term solution.
            System.Threading.Thread.Sleep(1000);
        }

        public void ClickCancel()
        {
            AutomationSugar.ClickOnAndWaitForUntilStale(new ByChained(DialogIdentifier, SeSugar.Automation.SimsBy.AutomationId("cancel_button")), DialogIdentifier);
        }

        #endregion
    }
}
