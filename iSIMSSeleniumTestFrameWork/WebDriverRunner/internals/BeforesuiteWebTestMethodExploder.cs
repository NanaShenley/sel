using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebDriverRunner.internals
{
    internal class BeforesuiteWebTestMethodExploder
    {
        private readonly MethodInfo _method;
        private readonly TestMethodBaseAttribute _attribute;
        private static readonly Dictionary<Type, Object> ClassInstances = new Dictionary<Type, object>();
        private readonly ITestMultiplier _multiplier;

        public BeforesuiteWebTestMethodExploder(MethodInfo method, Configuration config)
        {
            _method = method;
            var attrs = _method.GetCustomAttributes(typeof(TestMethodBaseAttribute), true);
            if (attrs.Length == 1)
            {
                _attribute = (TestMethodBaseAttribute)attrs[0];
            }
            _multiplier = new BeforesuiteWebTestMethodMultiplier(config);
        }

        private List<Object[]> GetParamsFromDataProvider(Object classInstance, MethodInfo method, TestMethodBaseAttribute attr)
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

        public static Object GetClassInstance(Type clazz)
        {
            if (!ClassInstances.ContainsKey(clazz))
            {
                ClassInstances[clazz] = Activator.CreateInstance(clazz, null);
            }
            return ClassInstances[clazz];
        }

        private IEnumerable<object[]> FindParameters(Object classInstance)
        {
            // get the method parameters
            if (_attribute.DataProvider!=null)
            {
                return GetParamsFromDataProvider(classInstance, _method, _attribute);
            }
            return new List<object[]> { null };
        }

        public IReadOnlyCollection<TestMethodInstance> ExplodeBeforeSuiteWebTest()
        {

            var result = new List<TestMethodInstance>();

            // get the class instance
            var classInstance = GetClassInstance(_method.ReflectedType);

            var param = FindParameters(classInstance);
            // do the param fit in the method ?

            var infos = _method.GetParameters();
            var nbParamFromDataProvider = NbParamFromDataProvider(param);


            if (infos.Count() != nbParamFromDataProvider)
            {
                throw new DataProviderException("expected " + infos.Count() + " parameters but got " + nbParamFromDataProvider);
            }

            // for each set of data parameters available to the method
            foreach (var parametersValue in param)
            {


                // TODO : move everything to a multiplier construct, ie InvocationCount * whatever else. ? 
                // for each invoc count
                for (var i = 0; i < _attribute.InvocationCount; i++)
                {
                    // multiply the tests if needed, for instance if multiple browers are needed
                    var res = _multiplier.MultiplyBeforeSuiteWebTest(_method);
                    if (res.Count == 0)
                    {
                        var metadata = new Dictionary<string, object>();
                        result.Add(new TestMethodInstance(_attribute, metadata, parametersValue, classInstance, _method));
                    }
                    else
                    {
                        foreach (var o in res)
                        {
                            var metadata = new Dictionary<string, object>();
                            metadata[_multiplier.Key()] = o;
                            result.Add(new TestMethodInstance(_attribute, metadata, parametersValue, classInstance, _method));
                        }
                    }
                }
            }
            return result;
        }


        public IReadOnlyCollection<TestMethodInstance> Explode()
        {

            var result = new List<TestMethodInstance>();

            // get the class instance
            var classInstance = GetClassInstance(_method.ReflectedType);

            var param = FindParameters(classInstance);
            // do the param fit in the method ?
            
            var infos = _method.GetParameters();
            var nbParamFromDataProvider = NbParamFromDataProvider(param);
            

            if (infos.Count() != nbParamFromDataProvider)
            {
                throw new DataProviderException("expected " + infos.Count() + " parameters but got " + nbParamFromDataProvider);
            }

            // for each set of data parameters available to the method
            foreach (var parametersValue in param)
            {


                // TODO : move everything to a multiplier construct, ie InvocationCount * whatever else. ? 
                // for each invoc count
                for (var i = 0; i < _attribute.InvocationCount; i++)
                {
                    // multiply the tests if needed, for instance if multiple browers are needed
                    var res = _multiplier.Multiply(_method);
                    if (res.Count == 0)
                    {
                        var metadata = new Dictionary<string, object>();
                        result.Add(new TestMethodInstance(_attribute, metadata, parametersValue, classInstance, _method));
                    }
                    else
                    {
                        foreach (var o in res)
                        {
                            var metadata = new Dictionary<string, object>();
                            metadata[_multiplier.Key()] = o;
                            result.Add(new TestMethodInstance(_attribute, metadata, parametersValue, classInstance, _method));
                        }
                    }
                }
            }
            return result;
        }

        private int NbParamFromDataProvider(IEnumerable<object[]> enumerable)
        {
            if (enumerable.ToList().First() == null)
            {
                return 0;
            }
            else
            {
                return enumerable.ToList().First().Count();
            }
           
        }


        public TestMethodInstance GetSkippedInstance(RunnerException runnerException)
        {
            var metadata = new Dictionary<string, object>();
            var parametersValue = new object[0];
            var classInstance = GetClassInstance(_method.ReflectedType);
            return new TestMethodInstance(_attribute, metadata, parametersValue, classInstance, _method, runnerException);

        }
    }
}
