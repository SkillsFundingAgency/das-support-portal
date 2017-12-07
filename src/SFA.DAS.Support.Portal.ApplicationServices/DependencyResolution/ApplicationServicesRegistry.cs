using MediatR;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SFA.DAS.Support.Portal.ApplicationServices.DependencyResolution
{
    //[ExcludeFromCodeCoverage]
    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            Scan(
                scan =>
                {
                    scan.AssemblyContainingType<ApplicationServicesRegistry>();
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf(typeof(IRequestHandler<,>));
                    scan.AddAllTypesOf(typeof(IAsyncRequestHandler<,>));
                });
            For<ICrypto>().Use<Crypto>();
            For<ICryptoSettings>().Use<CryptoSettings>();
            For<IChallengeService>().Use<ChallengeService>();
            For<IDatetimeService>().Use<DatetimeService>();
            For<ISiteSettings>().Use<SiteSettings>();
            For<IManifestRepository>().Use<ManifestRepository>();
        }
    }
}
