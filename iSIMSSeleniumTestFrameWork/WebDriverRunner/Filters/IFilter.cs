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

namespace WebDriverRunner.Filters
{
    public interface IFilter<T>
    {
        bool IsValid(T item);
    }

    public class SingleAssembly : IFilter<Assembly>
    {
        private readonly string _name;

        public SingleAssembly(string name)
        {
            _name = name.Replace(".dll", "");
        }
        public bool IsValid(Assembly item)
        {
            return item.GetName().Name.Equals(_name);
        }
    }
    public class All<T> : IFilter<T>
    {
        public bool IsValid(T item)
        {
            return true;
        }
    }

    public class And<T> : IFilter<T>
    {
        private readonly List<IFilter<T>> _filters;
        public And(params IFilter<T>[] filters)
        {
            _filters = filters.ToList();
        }
        public And(List<IFilter<T>> filters)
        {
            _filters = filters.ToList();
        }
        public bool IsValid(T item)
        {
            return _filters.All(filter => filter.IsValid(item));
        }
    }

    public class Or<T> : IFilter<T>
    {
        private readonly List<IFilter<T>> _filters;
        public Or(params IFilter<T>[] filters)
        {
            _filters = filters.ToList();
        }
        public Or(List<IFilter<T>> filters)
        {
            _filters = filters.ToList();
        }
        public bool IsValid(T item)
        {
            return _filters.Any(filter => filter.IsValid(item));
        }
    }

    public class ClassFilter : IFilter<MethodInfo>
    {
        private readonly string _clazz;
        public ClassFilter(string clazz)
        {
            _clazz = clazz;
        }

        public bool IsValid(MethodInfo method)
        {
            return method.ReflectedType.FullName.Equals(_clazz);
        }
    }

    public class GroupFilter : IFilter<MethodInfo>
    {
        private readonly string _group;
        private readonly bool _includeNotDoneTests;
        private readonly Variant _variantUnderTest;
        private readonly Guid _automatedRunId;

        public GroupFilter(string group, bool includeNotDoneTests, Variant variantUnderTest, Guid automatedRunId)
        {
            _group = group;
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
                isValid = attrs.Length == 1 && ((UnitTestAttribute)attrs[0]).Enabled && ((UnitTestAttribute)attrs[0]).Groups.Contains(_group);
            }
            else
            {
                var notDone = method.GetCustomAttribute<NotDoneAttribute>() != null;
                isValid = attrs.Length == 1 && ((UnitTestAttribute)attrs[0]).Enabled && ((UnitTestAttribute)attrs[0]).Groups.Contains(_group) && !notDone;
            }

            isValid = isValid && DefaultTestMethodFilter.TestIsApplicableForVariant(method, _variantUnderTest, _automatedRunId);

            return isValid;
        }
    }

    public class NotDoneFilter : IFilter<MethodInfo>
    {
        public bool IsValid(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(typeof(UnitTestAttribute), true);
            var notDone = method.GetCustomAttribute<NotDoneAttribute>() != null;
            return attrs.Length == 1 && ((UnitTestAttribute)attrs[0]).Enabled && notDone;
        }
    }

    public class AttributeMethodFilter : IFilter<MethodInfo>
    {
        private readonly Type _t;
        public AttributeMethodFilter(Type t)
        {
            _t = t;
        }
        public bool IsValid(MethodInfo method)
        {
            var attrs = method.GetCustomAttributes(_t, true);
            return attrs.Length == 1;
        }
    }
}
