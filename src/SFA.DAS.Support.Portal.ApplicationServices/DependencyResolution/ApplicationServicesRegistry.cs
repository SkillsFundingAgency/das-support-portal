using MediatR;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.ApplicationServices.Settings;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace SFA.DAS.Support.Portal.ApplicationServices.DependencyResolution
{
    public sealed class ApplicationServicesRegistry : Registry
    {
        public ApplicationServicesRegistry()
        {
            this.Scan(
                scan =>
                {
                    scan.AssemblyContainingType<ApplicationServicesRegistry>();
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf(typeof(IRequestHandler<,>));
                    scan.AddAllTypesOf(typeof(IAsyncRequestHandler<,>));
                });
            this.For<ICrypto>().Use<Crypto>();
            this.For<ICryptoSettings>().Use<CryptoSettings>();
            this.For<IChallengeService>().Use<ChallengeService>();
            this.For<IDatetimeService>().Use<DatetimeService>();
            For<ISiteSettings>().Use<SiteSettings>();
            For<IManifestRepository>().Use<ManifestRepository>();
        }
    }
}
