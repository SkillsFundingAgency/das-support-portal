using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Portal.Health
{
    public class HealthRegistry : Registry
    {
        public HealthRegistry()
        {
            For<IHealthService>().Use<HealthService>();
        }
    }
}
