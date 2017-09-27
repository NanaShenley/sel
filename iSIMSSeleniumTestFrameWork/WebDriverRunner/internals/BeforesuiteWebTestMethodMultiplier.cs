using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebDriverRunner.webdriver;

namespace WebDriverRunner.internals
{
    class BeforesuiteWebTestMethodMultiplier : ITestMultiplier
    { 
        private readonly Configuration _config;
        public BeforesuiteWebTestMethodMultiplier(Configuration config)
        {
            _config = config;
        }
        public string Key()
        {
            return (WebContext.BrowserKey);
        }

        public List<Object> MultiplyBeforeSuiteWebTest(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(BeforeSuiteWebTestAttribute), true);
            if (attrs.Length == 1)
            {
                var att = (BeforeSuiteWebTestAttribute)attrs[0];
                var res = new List<object>();
                if (att.Browsers == null || att.Browsers.Count() == 0)
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

        public List<Object> Multiply(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(WebDriverTestAttribute), true);
            if (attrs.Length == 1)
            {
                var att = (WebDriverTestAttribute)attrs[0];
                var res = new List<object>();
                if (att.Browsers == null || att.Browsers.Count() == 0)
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