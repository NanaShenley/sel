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
using WebDriverRunner.ReportPlugin;

namespace WebDriverRunner.internals
{
    class PluginLoader
    {
        private readonly Type _desiredType;

        public PluginLoader(Type type)
        {
            _desiredType = type;
        }

        private Type FindClassInLoadedAssemblies(string clazz)
        {
            var loaded = AppDomain.CurrentDomain.GetAssemblies();
            var types = loaded.SelectMany(a => a.GetTypes()).Where(t => t.FullName.Contains(clazz));
            var tt = types.ToList();
            if (tt.Count != 1)
            {
                throw new RunnerException("Cannot find class " + clazz + " in the loaded assemblies.");
            }
            return tt[0];

        }


        private static IEnumerable<Type> FindPluginInDll(string dll)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dll);
                if (assembly == null)
                {
                    throw new RunnerException("Cannot find assembly " + dll);
                }
                return assembly.GetTypes().Where(t => typeof(IReporter).IsAssignableFrom(t)).ToList();
            }
            catch (Exception e)
            {
                throw new RunnerException("Cannot load DLL " + dll + e);
            }
        }
        public List<Type> GetPluginEntryPoints(string plugin)
        {
            var res = new List<Type>();
            if (!plugin.EndsWith(".dll"))
            {
                // not a DLL. Should be a class.
                res.Add(FindClassInLoadedAssemblies(plugin));
            }
            else
            {
                res.AddRange(FindPluginInDll(plugin));
            }
            if (res.Count == 0)
            {
                throw new RunnerException("Cannot find plugin of type " + _desiredType + " in " + plugin);
            }
            return res;
        }
    }
}
