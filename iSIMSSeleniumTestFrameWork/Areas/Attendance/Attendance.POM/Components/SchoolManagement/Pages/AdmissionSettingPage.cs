using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POM.Components.SchoolManagement
{
    public class AdmissionSettingPage : BaseComponent
    {
        public override By ComponentIdentifier
        {
            get { return SimsBy.Id("screen"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "LastAdmissionNumber")]
        private IWebElement _lastAdmissionNumberTextBox;

        public string LastAdmissionNumber
        {
            get { return _lastAdmissionNumberTextBox.GetValue(); }
            set { _lastAdmissionNumberTextBox.SetText(value); }
        }

        #endregion
    }
}
