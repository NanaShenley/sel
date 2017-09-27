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

namespace WebDriverRunner.internals
{
    public class TestMethodInstance
    {
        // attributes
        // metadata
        // parameters
        // classInstance
        // method
        public readonly TestMethodBaseAttribute Attr;
        private readonly Dictionary<string, Object> _metadata;
        private readonly Object[] _parameters;
        private readonly Object _classInstance;
        private readonly MethodInfo _method;
        private readonly RunnerException _ex;

        public readonly Guid TestId = Guid.NewGuid();

        public TestMethodInstance(TestMethodBaseAttribute attr, Dictionary<string, object> metadata, object[] parameters, object classInstance, MethodInfo method)
            : this(attr, metadata, parameters, classInstance, method, null)
        {

        }
        public TestMethodInstance(TestMethodBaseAttribute attr, Dictionary<string, object> metadata, object[] parameters, object classInstance, MethodInfo method, RunnerException ex)
        {
            Attr = attr;
            _metadata = metadata;
            _parameters = parameters;
            _classInstance = classInstance;
            _method = method;
            _ex = ex;
        }

        public RunnerException RunnerException { get { return _ex; } }


        public void SetMetadata(string key, Object value)
        {
            _metadata[key] = value;
        }

        public Dictionary<string, object> GetMetadata()
        {
            return _metadata;
        }

        public object[] GetParameters()
        {
            return _parameters;
        }



        public Object GetMetadata(string key)
        {
            Object value;
            _metadata.TryGetValue(key, out value);
            return value;
        }

	    /// <exception cref="TargetException">In the .NET for Windows Store apps or the Portable Class Library, catch <see cref="T:System.Exception" /> instead.The <paramref name="obj" /> parameter is null and the method is not static.-or- The method is not declared or inherited by the class of <paramref name="obj" />. -or-A static constructor is invoked, and <paramref name="obj" /> is neither null nor an instance of the class that declared the constructor.</exception>
	    public void Invoke()
        {
            _method.Invoke(_classInstance, _parameters);
        }

        public override string ToString()
        {
            //ParameterInfo[] ps = method.GetParameters();
            var p = "";
            if (_parameters != null)
            {
                p = _parameters.Aggregate("", (current, info) => current + info /*+ " " + info.GetType().Name */+ ", ");
            }
            return _method.Name + "(" + p + ")";
        }

        public MethodInfo GetMethod()
        {
            return _method;
        }
    }
}
