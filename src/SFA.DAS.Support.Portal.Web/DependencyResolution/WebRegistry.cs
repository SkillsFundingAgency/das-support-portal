using System.Collections.Generic;
using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Logging;
using SFA.DAS.Support.Portal.Web.Services;
using StructureMap.Configuration.DSL;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using Microsoft.Owin.Security.Provider;
using Nest;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class WebRegistry : Registry
    {
        

        public WebRegistry()
        {
           
            For<List<SiteManifest>>()
                .Singleton()
                .Use<List<SiteManifest>>(x =>  Startup.SiteManifests);
            For<Dictionary<string, SiteChallenge>>()
                .Singleton()
                .Use<Dictionary<string, SiteChallenge>>(x =>  Startup.SiteChallenges);
            For<Dictionary<string, SiteResource>>()
                .Singleton()
                .Use<Dictionary<string, SiteResource>>(x =>  Startup.SiteResources);

            For<IHttpStatusCodeStrategy>().Use<StrategyForSystemErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForClientErrorStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForRedirectionStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForSuccessStatusCode>();
            For<IHttpStatusCodeStrategy>().Use<StrategyForInformationStatusCode>();

            
            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<HttpClient>().Use((c) => new HttpClient());

            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));

            For<IMappingService>().Use<MappingService>();
            For<ICheckPermissions>().Use<PermissionCookieProvider>();
            For<IGrantPermissions>().Use<PermissionCookieProvider>();
           
            For<IGetCurrentIdentity>().Use<IdentityService>();
        }

       
    }
}