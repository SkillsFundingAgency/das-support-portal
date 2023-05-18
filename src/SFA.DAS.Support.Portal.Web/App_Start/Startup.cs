using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Web.App_Start;
using SFA.DAS.Support.Portal.Web.Settings;
using OpenAthens.Owin.Security.OpenIdConnect;

namespace SFA.DAS.Support.Portal.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            DependencyResolver.Current.GetService<ILog>().Debug("Configuring Authentication");

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            var webConfiguration = DependencyResolver.Current.GetService<IWebConfiguration>();

            if (webConfiguration != null && webConfiguration.UseDfESignIn)
            {
                DependencyResolver.Current.GetService<ILog>().Info("Using DfESignIn Authentication");
                var openIdConnectConfiguration = DependencyResolver.Current.GetService<IOpenIdConnectConfiguration>();
                app.UseOpenIdConnectAuthentication(openIdConnectConfiguration.GetOpenIdConnectOptions());
            }
            else
            {
                DependencyResolver.Current.GetService<ILog>().Info("Using WsFederation Authentication");
                var adfsConfiguration = DependencyResolver.Current.GetService<IADFSConfiguration>();
                app.UseWsFederationAuthentication(adfsConfiguration.GetADFSOptions());
            }
        }
    }
}