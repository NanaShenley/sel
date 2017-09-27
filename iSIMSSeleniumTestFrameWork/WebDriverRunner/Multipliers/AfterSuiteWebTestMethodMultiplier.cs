/*************************************************************************
* 
* Copyright © Capita Children's Services 2015
* All Rights Reserved.
* Proprietary and confidential
* Written by Steve Gray <steve.gray@capita.co.uk> and Francois Reynaud<Francois.Reynaud@capita.co.uk> 2015
* 
* NOTICE:  All Source Code and information contained herein remains
* the property of Capita Children's Services. The intellectual and technical concepts contained
* herein are proprietary to Capita Children's Services 2015 and may be covered by U.K, U.S and Foreign Patents,
* patents in process, and are protected by trade secret or copyright law.
* Dissemination of this information or reproduction of this material
* is strictly forbidden unless prior written permission is obtained
* from Capita Children's Services.
*
* Source Code distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied.  
*/
using Selene.Support.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebDriverRunner.internals;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.Multipliers
{
    public class AfterSuiteWebTestMethodMultiplier: IMultiplier
    {
        private readonly Configuration _config;
        public AfterSuiteWebTestMethodMultiplier(Configuration config)
        {
            _config = config;
        }
        public string Key()
        {
            return (WebContext.BrowserKey);
        }

        public List<object> Multiply(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(AfterSuiteWebTestAttribute), true);
            if (attrs.Length == 1)
            {
                var att = (AfterSuiteWebTestAttribute)attrs[0];
                var res = new List<object>();
                if (att.Browsers == null || !att.Browsers.Any())
                {
                    res.AddRange(_config.Browsers);
                }
                else
                {
                    res.AddRange(att.Browsers);
                }

                return res;

            }
            return new List<Object>();
        }
    }
}