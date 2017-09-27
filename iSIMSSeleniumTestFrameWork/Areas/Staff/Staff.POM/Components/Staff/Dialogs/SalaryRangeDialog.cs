using SeSugar.Automation;
using Staff.POM.Base;
using Staff.POM.Components.Staff.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff.POM.Components.Staff.Dialogs
{
    public class SalaryRangeDialog : BaseDialogComponent
    {
        private SalaryRangePage _salaryRangeDetails = new SalaryRangePage();
        
        public SalaryRangePage SalaryRangeDetails
        {
            get
            {
                return _salaryRangeDetails;
            }
        }

        public override OpenQA.Selenium.By DialogIdentifier
        {
            get { return SimsBy.AutomationId("SalaryRange_triplet"); }
        }
    }
}
