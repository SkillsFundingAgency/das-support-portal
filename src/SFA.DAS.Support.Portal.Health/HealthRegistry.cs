using System.Diagnostics.CodeAnalysis;
using StructureMap.Configuration.DSL;

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