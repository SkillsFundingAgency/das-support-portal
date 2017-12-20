using System.Web;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Services;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Web.Logging;
using SFA.DAS.Support.Portal.Web.Services;
using SFA.DAS.Support.Portal.Web.Settings;
using StructureMap.Configuration.DSL;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using SFA.DAS.Support.Portal.Infrastructure.Services;
using SFA.DAS.Support.Portal.Infrastructure.Settings;

namespace SFA.DAS.Support.Portal.Web.DependencyResolution
{
    [ExcludeFromCodeCoverage]
    public class WebRegistry : Registry
    {
        public WebRegistry()
        {
            For<ISiteConnectorSettings>().Use<SiteConnectorSettings>();
            For<IClientAuthenticator>().Use<ActiveDirectoryClientAuthenticator>();

            For<HttpClient>()
                .Use((c) => SecureClient(
                    c.GetInstance<ISiteConnectorSettings>(),
                    c.GetInstance<IClientAuthenticator>()));

            For<IRequestContext>().Use(x => new RequestContext(new HttpContextWrapper(HttpContext.Current)));

            For<IMappingService>().Use<MappingService>();
            For<ICheckPermissions>().Use<PermissionCookieProvider>();
            For<IGrantPermissions>().Use<PermissionCookieProvider>();
            For<IChallengeSettings>().Use<ChallengeSettings>();
            For<IRoleSettings>().Use<RoleSettings>();
            For<IAuthSettings>().Use<AuthSettings>();

            For<IGetCurrentIdentity>().Use<IdentityService>();
        }

        private HttpClient SecureClient(ISiteConnectorSettings settings, IClientAuthenticator authenticator)
        {
            var secureClient = new HttpClient();

            var token = authenticator.Authenticate(settings.ClientId,
                                                    settings.AppKey,
                                                    settings.ResourceId,
                                                    settings.Tenant).Result;

            secureClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",
                                                                                                token);

            return secureClient;
        }
    }
}