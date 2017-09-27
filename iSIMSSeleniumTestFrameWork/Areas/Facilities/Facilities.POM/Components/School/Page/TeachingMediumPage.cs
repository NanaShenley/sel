using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;

using System.Collections.Generic;

namespace POM.Components.School
{
    public class TeachingMediumPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.CssSelector("[data-detail-section-name='searchResults']"); }
        }

        #region Page properties

        public GridComponent<TeachingMediumRow> TeachingMedium
        {
            get
            {
                GridComponent<TeachingMediumRow> returnValue = null;
                Retry.Do(() =>
                {
                    returnValue = new GridComponent<TeachingMediumRow>(By.CssSelector("[data-maintenance-container='Rows']"), ComponentIdentifier);
                });
                return returnValue;
            }
        }


        public class TeachingMediumRow : GridRow
        {
            [FindsBy(How = How.CssSelector, Using = "[name$='Code']")]
            private IWebElement _code;

            [FindsBy(How = How.CssSelector, Using = "[name$='Description']")]
            private IWebElement _description;

            public string Code
            {
                set { _code.SetText(value); }
                get { return _code.GetValue(); }
            }

            public string Description
            {
                set { _description.SetText(value); }
                get { return _description.GetValue(); }
            }
        }


        #endregion

    }
}
