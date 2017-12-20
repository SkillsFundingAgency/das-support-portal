using StructureMap.Configuration.DSL;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Health
{
    [ExcludeFromCodeCoverage]
    public class HealthRegistry : Registry
    {
        public HealthRegistry()
        {
            For<IHealthService>().Use<HealthService>();
        }
    }
}
