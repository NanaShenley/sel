using System.Collections.Generic;
using System.Reflection;
using WebDriverRunner.internals;
using WebDriverRunner.Multipliers;

namespace WebDriverRunner.Exploders
{
    public class MethodExploder : MethodExploderBase
    {
        private ConfigTestMethodMultiplier _testMethodMultiplier;

        public MethodExploder(MethodInfo method, Configuration configuration) : base(method)
        {
            _testMethodMultiplier = new ConfigTestMethodMultiplier(configuration);
        }

        public IReadOnlyCollection<TestMethodInstance> Explode()
        {
            return Explode(_testMethodMultiplier);
        }
    }
}