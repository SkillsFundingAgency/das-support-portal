using StructureMap.Configuration.DSL;

namespace SFA.DAS.Portal.Health
{
    public class HealthRegistry : Registry
    {
        public HealthRegistry()
        {
            For<IHealthService>().Use<HealthService>();
        }
    }
}
