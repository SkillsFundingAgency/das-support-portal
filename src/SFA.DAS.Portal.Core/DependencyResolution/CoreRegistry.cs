using SFA.DAS.Portal.Core.Helpers;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Portal.Core.DependencyResolution
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IPayeSchemeObfuscator>().Use<PayeSchemeObfuscator>();
        }
    }
}
