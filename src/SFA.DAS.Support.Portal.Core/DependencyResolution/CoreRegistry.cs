using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Helpers;
using StructureMap.Configuration.DSL;

namespace SFA.DAS.Support.Portal.Core.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            For<IPayeSchemeObfuscator>().Use<PayeSchemeObfuscator>();
        }
    }
}
