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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WebDriverRunner.Filters;

namespace WebDriverRunner.testfinder
{
    public class TestFinderBase : ITestFinder
    {   public Assembly Assembly;
        public IFilter<MethodInfo> MethodFilter;

        public IEnumerable<MethodInfo> FindAllMethods()
        {
            return GetTestMethods(MethodFilter, Assembly);
        }

        private IEnumerable<MethodInfo> GetTestMethods(IFilter<MethodInfo> filter, Assembly assembly)
        {

            return assembly.GetTypes().SelectMany(c => c.GetMethods((BindingFlags.Public | BindingFlags.Instance)).Where(filter.IsValid));
        }

        public IEnumerable<MethodInfo> GetSuiteMethods(IEnumerable<MethodInfo> methods, Type t)
        {
            IFilter<MethodInfo> before = new AttributeMethodFilter(t);
// ReSharper disable once PossibleNullReferenceException
            return methods.SelectMany(m => m.ReflectedType.GetMethods((BindingFlags.Public | BindingFlags.Instance)))
                .Where(before.IsValid).Distinct();
        }
    }
}
