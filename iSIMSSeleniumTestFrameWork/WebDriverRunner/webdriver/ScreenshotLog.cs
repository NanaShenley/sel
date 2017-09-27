﻿/*************************************************************************
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
using System.Runtime.Serialization;
using WebDriverRunner.results;

namespace WebDriverRunner.webdriver
{
    [DataContract]
    public class ScreenshotLog : Log
    {
        public ScreenshotLog(string title, string url, string path) : base("Screenshot")
        {
            Title = title;
            Url = url;
            Path = path;
        }
        [DataMember]
        protected new readonly string Type = "Screenshot";
        [DataMember]
        public readonly string Title;
        [DataMember]
        public readonly string Url;
        [DataMember]
        public readonly string Path;
    }
}
