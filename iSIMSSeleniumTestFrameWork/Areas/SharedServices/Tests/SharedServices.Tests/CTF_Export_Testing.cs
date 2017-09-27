using SharedComponents.BaseFolder;
using SharedComponents.Utils;
using SharedServices.Components;
using SharedServices.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverRunner.internals;

namespace SharedServices.Tests
{
    public class CTF_Export_Testing : BaseSeleniumComponents
    {


        [WebDriverTest(InvocationCount = 1, Groups = new[] { Constants.Export },
           Browsers = new[] { BrowserDefaults.Ie })]
        public void CTF_Exporttest()
        {
            CTF_Export  export= new CTF_Export();

            export.CreateCTF_Export();
            WaitForElement(Constants.SearchRecordsToFindtext);
            export.pupilselector();


            

        }





    }
}
