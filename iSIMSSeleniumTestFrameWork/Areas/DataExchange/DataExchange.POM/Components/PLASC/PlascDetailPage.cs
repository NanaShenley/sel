using DataExchange.POM.Base;
using OpenQA.Selenium;

namespace DataExchange.POM.Components.PLASC
{
    /// <summary>
    /// Represents entire detail page
    /// </summary>
    public class PlascDetailPage: BaseComponent
    {
        public override By ComponentIdentifier
        {
            get
            {
                return By.Id("screen");
            }
        }
    }
}
