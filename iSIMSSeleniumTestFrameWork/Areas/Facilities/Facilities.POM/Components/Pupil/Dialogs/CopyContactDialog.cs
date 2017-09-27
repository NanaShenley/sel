using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace POM.Components.Pupil
{
    public class CopyContactDialog : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-section-id='dialog-detail']"); }
        }

    }
}
