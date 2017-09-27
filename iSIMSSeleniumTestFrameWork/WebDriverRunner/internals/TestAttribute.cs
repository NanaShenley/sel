///*************************************************************************
//* 
//* Copyright © Capita Children's Services 2015
//* All Rights Reserved.
//* Proprietary and confidential
//* Written by Steve Gray <steve.gray@capita.co.uk> and Francois Reynaud<Francois.Reynaud@capita.co.uk> 2015
//* 
//* NOTICE:  All Source Code and information contained herein remains
//* the property of Capita Children's Services. The intellectual and technical concepts contained
//* herein are proprietary to Capita Children's Services 2015 and may be covered by U.K, U.S and Foreign Patents,
//* patents in process, and are protected by trade secret or copyright law.
//* Dissemination of this information or reproduction of this material
//* is strictly forbidden unless prior written permission is obtained
//* from Capita Children's Services.
//*
//* Source Code distributed under the License is distributed on an
//* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
//* KIND, either express or implied.  
//*/
//using System;
//using System.Runtime.CompilerServices;
//using System.Runtime.Serialization;

//namespace WebDriverRunner.internals
//{
//    [DataContract]
//    public class TestMethodBaseAttribute : Attribute
//    {
//        [DataMember]
//        public int InvocationCount = 1;
//        public bool Enabled = true;
//        [DataMember]
//        public int TimeoutSeconds = 600;
//        [DataMember]
//        public string DataProvider = null;
//        [DataMember]
//        public string[] Groups = { };

//        [DataMember]
//        public readonly string File;
//        [DataMember]
//        public readonly string Member;
//        [DataMember]
//        public readonly int Line;

//        [DataMember]
//        public string UserDataDir = null;
//        public TestMethodBaseAttribute
//            (
//            [CallerFilePath] string file = "",
//            [CallerMemberName] string member = "",
//            [CallerLineNumber] int line = 0)
//        {
//            File = file;
//            Member = member;
//            Line = line;
//        }
//    }

//    public class UnitTestAttribute : TestMethodBaseAttribute
//    {
//        public UnitTestAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
//            : base(file, member, line)
//        {

//        }
//    }

//    public class ConfigurationMethodAttribute : TestMethodBaseAttribute
//    {
//        public ConfigurationMethodAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
//            : base(file, member, line)
//        {

//        }
//    }

//    //public class BeforeSuiteAttribute : TestMethodBaseAttribute
//    //{
//    //    public BeforeSuiteAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
//    //        : base(file, member, line)
//    //    {

//    //    }
//    //}
//    //public class AfterSuiteAttribute : TestMethodBaseAttribute
//    //{
//    //    public AfterSuiteAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "", [CallerLineNumber] int line = 0)
//    //        : base(file, member, line)
//    //    {

//    //    }
//    //}


//    //AfterSuiteWebTest
//    public class AfterSuiteWebTestAttribute : TestMethodBaseAttribute
//    {
//        public AfterSuiteWebTestAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "",
//            [CallerLineNumber] int line = 0)
//            : base(file, member, line)
//        {

//        }

//        public string[] Browsers = null;


//    }

//    //BeforeSuiteWebTestAttribute
//    public class BeforeSuiteWebTestAttribute : TestMethodBaseAttribute
//    {
//        public BeforeSuiteWebTestAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "",
//            [CallerLineNumber] int line = 0)
//            : base(file, member, line)
//        {

//        }

//        public string[] Browsers = null;


//    }
//    public class WebDriverTestAttribute : UnitTestAttribute
//    {
//        public WebDriverTestAttribute([CallerFilePath] string file = "", [CallerMemberName] string member = "",
//            [CallerLineNumber] int line = 0)
//            : base(file, member, line)
//        {

//        }

//        public string[] Browsers = null;


//    }

//    public class ChromeUiTestAttribute : WebDriverTestAttribute
//    {
//        public ChromeUiTestAttribute(params string[] groups)
//            : base("", "", 0)
//        {
//            Groups = groups;
//            Browsers = new[] { "chrome" };
//            TimeoutSeconds = 600;
//        }
//    }

//    public class IeUiTestAttribute : WebDriverTestAttribute
//    {
//        public IeUiTestAttribute(params string[] groups)
//            : base("", "", 0)
//        {
//            Groups = groups;
//            Browsers = new[] { "internet explorer" };
//            TimeoutSeconds = 600;
//        }
//    }

//    public class AllBrowserUiTestAttribute : WebDriverTestAttribute
//    {
//        public AllBrowserUiTestAttribute(params string[] groups)
//            : base("", "", 0)
//        {
//            Groups = groups;
//            Browsers = new[] { "chrome", "internet explorer" };
//            TimeoutSeconds = 600;
//        }
//    }
//}
