using OpenQA.Selenium;
using POM.Base;
using POM.Helper;

namespace Communications.POM.Components.Communication.Pages
{
    public class SaveTemplatePage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }
    }
}
