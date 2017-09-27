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
using WebDriverRunner.Multipliers;

namespace WebDriverRunner.Exploders
{
    public class MethodExploderBase :IMethodExploder
    {
        protected static readonly Dictionary<Type, object> ClassInstances = new Dictionary<Type, object>();
        protected readonly MethodInfo Method;
        protected readonly TestMethodBaseAttribute Attribute;

        protected MethodExploderBase(MethodInfo method)
        {
            Method = method;
            var attrs = Method.GetCustomAttributes(typeof(TestMethodBaseAttribute), true);
            if (attrs.Length == 1)
            {
                Attribute = (TestMethodBaseAttribute)attrs[0];
            }
            
        }

        public TestMethodInstance GetSkippedInstance(RunnerException runnerException)
        {
            var metadata = new Dictionary<string, object>();
            var parametersValue = new object[0];
            var classInstance = GetClassInstance(Method.ReflectedType);
            return new TestMethodInstance(Attribute, metadata, parametersValue, classInstance, Method, runnerException);

        }

      

        private int NbParamFromDataProvider(IEnumerable<object[]> enumerable)
        {
            IEnumerable<object[]> objectses = enumerable as IList<object[]> ?? enumerable.ToList();
            if (objectses.ToList().First() == null)
            {
                return 0;
            }
            return objectses.ToList().First().Count();
        }

        private IEnumerable<object[]> FindParameters(Object classInstance)
        {
            // get the method parameters
            if (Attribute.DataProvider != null)
            {
                return GetParamsFromDataProvider(classInstance, Method, Attribute);
            }
            return new List<object[]> { null };
        }

        private IEnumerable<object[]> GetParamsFromDataProvider(Object classInstance, MethodInfo method, TestMethodBaseAttribute attr)
        {
            // find the method.
            var name = attr.DataProvider;
            if (name == null)
            {
                throw new DataProviderException("expected a data provider for " + method);
            }
            var provider = classInstance.GetType().GetMethod(name);
            if (provider == null)
            {
                throw new DataProviderException("No dataprovider method with name " + name);
            }
            return (List<object[]>)provider.Invoke(classInstance, null);
        }


        private object GetClassInstance(Type clazz)
        {
            if (!ClassInstances.ContainsKey(clazz))
            {
                ClassInstances[clazz] = Activator.CreateInstance(clazz, null);
            }
            return ClassInstances[clazz];
        }

        private List<TestMethodInstance> GetMethodInstance(IEnumerable<object[]> param, List<TestMethodInstance> result, object classInstance, IMultiplier methodMultiplier)
        {
            foreach (var parametersValue in param)
            {
                // TODO : move everything to a multiplier construct, ie InvocationCount * whatever else. ? 
                // for each invoc count
                for (var i = 0; i < Attribute.InvocationCount; i++)
                {
                    // multiply the tests if needed, for instance if multiple browers are needed
                    var res = methodMultiplier.Multiply(Method);
                    if (res.Count == 0)
                    {
                        var metadata = new Dictionary<string, object>();
                        result.Add(new TestMethodInstance(Attribute, metadata, parametersValue, classInstance, Method));
                    }
                    else
                    {
                        foreach (var o in res)
                        {
                            var metadata = new Dictionary<string, object>();
                            metadata[methodMultiplier.Key()] = o;
                            result.Add(new TestMethodInstance(Attribute, metadata, parametersValue, classInstance, Method));
                        }
                    }
                }
            }
            return result;
        }

        public IReadOnlyCollection<TestMethodInstance> Explode(IMultiplier testMethodMultiplier)
        {
            var result = new List<TestMethodInstance>();

            // get the class instance
            var classInstance = GetClassInstance(Method.ReflectedType);

            var param = FindParameters(classInstance);
            // do the param fit in the method ?

            var infos = Method.GetParameters();
            IEnumerable<object[]> enumerable = param as IList<object[]> ?? param.ToList();
            var nbParamFromDataProvider = NbParamFromDataProvider(enumerable);

            if (infos.Count() != nbParamFromDataProvider)
            {
                throw new DataProviderException("expected " + infos.Count() + " parameters but got " + nbParamFromDataProvider);
            }
            return GetMethodInstance(enumerable, result, classInstance, testMethodMultiplier);
        }

    }
}
