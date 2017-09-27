using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataExchange.POM.Base;
using OpenQA.Selenium;
using SeSugar.Automation;

namespace DataExchange.POM.Components.DENI
{
   public  class DENIDetailPage : BaseComponent
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
