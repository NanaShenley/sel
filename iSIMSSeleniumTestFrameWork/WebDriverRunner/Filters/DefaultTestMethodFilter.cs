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
using System.Reflection;
using WebDriverRunner.internals;

namespace WebDriverRunner.Filters
{
    public class DefaultTestMethodFilter : IFilter<MethodInfo>
    {        
        private readonly bool _includeNotDoneTests;
        private readonly Variant _variantUnderTest;
        private readonly Guid _automatedRunId;

        public DefaultTestMethodFilter(bool includeNotDoneTests, Variant variantUnderTest, Guid automatedRunId)
        {
            _includeNotDoneTests = includeNotDoneTests;
            _variantUnderTest = variantUnderTest;
            _automatedRunId = automatedRunId;
        }

        public bool IsValid(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(UnitTestAttribute), true);

            var isValid = false;

            if (_includeNotDoneTests)
            {
                isValid = attrs.Length == 1 && ((UnitTestAttribute)attrs[0]).Enabled;
            }
            else
            {
                var notDone = method.GetCustomAttribute<NotDoneAttribute>() != null;
                isValid = attrs.Length == 1 && ((UnitTestAttribute)attrs[0]).Enabled && !notDone;
            }

            isValid = isValid && TestIsApplicableForVariant(method, _variantUnderTest, _automatedRunId);

            return isValid;
        }

        public static bool TestIsApplicableForVariant(MethodInfo method, Variant variantUnderTest, Guid automatedRunId)
        {
            if (automatedRunId == Guid.Empty) return true;

            var variantInfo = method.GetCustomAttribute<VariantAttribute>();

            //If there is no variant specified for the test and the SUT variant is NI Primary, include this test for execution.
            //Reason for this is that tests were initialy written against an NI Primary SUT; this is the "default behaviour"
            if (variantInfo == null && variantUnderTest == Variant.NorthernIrelandStatePrimary)
            {
                return true;
            }
            //For any other case where there isn't a variant specified, exclude the test from running.
            else if (variantInfo == null)
            {
                return false;
            }

            bool isApplicable = variantInfo.ApplicableVariants.HasFlag(variantUnderTest);

            return isApplicable;
        }
    }
}