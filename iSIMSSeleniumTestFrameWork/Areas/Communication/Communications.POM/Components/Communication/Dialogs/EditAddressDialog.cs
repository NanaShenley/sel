using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using POM.Base;
using POM.Helper;


namespace POM.Components.Communication
{
    public class EditAddressDialog : BaseDialogComponent
    {
        public override By DialogIdentifier
        {
            get { return SimsBy.AutomationId("find_address_detail"); }
        }

        #region Properties

        [FindsBy(How = How.Name, Using = "Address.PAONRange")]
        private IWebElement _houseBuidingNoTextbox;

        [FindsBy(How = How.Name, Using = "Address.PAONDescription")]
        private IWebElement _houseBuidingNameTextbox;

        public string BuidingNo
        {
            set { _houseBuidingNoTextbox.SetText(value); }
        }

        public string BuildingName
        {
            set { _houseBuidingNameTextbox.SetText(value); }
        }

        #endregion
    }
}
