using System.Collections.Generic;
using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Web.Logging;
using SFA.DAS.Support.Portal.Web.Services;
using StructureMap.Configuration.DSL;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Shared;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class WebRegistry : Registry
    {
        

        public WebRegistry()
        {
           
            For<List<SiteManifest>>().Singleton()
                .Use<List<SiteManifest>>(x =>  Startup.SiteManifests);

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