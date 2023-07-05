using System;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Web.Settings;
using OpenAthens.Owin.Security.OpenIdConnect;
using SFA.DAS.Support.Portal.Web.Interfaces;

namespace SFA.DAS.Support.Portal.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            DependencyResolver.Current.GetService<ILog>().Debug("Configuring Authentication");

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            
            var webConfiguration = DependencyResolver.Current.GetService<IWebConfiguration>();

            if (webConfiguration != null && webConfiguration.UseDfESignIn)
            {
                var dfESignInServiceConfiguration = DependencyResolver.Current.GetService<IDfESignInServiceConfiguration>();
                var openIdConnectConfiguration = DependencyResolver.Current.GetService<IOpenIdConnectConfiguration>();
                var authenticationSessionStore = DependencyResolver.Current.GetService<IAuthenticationSessionStore>();
                DependencyResolver.Current.GetService<ILog>().Info("Using DfESignIn Authentication");
                app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    ExpireTimeSpan = TimeSpan.FromMinutes(dfESignInServiceConfiguration.DfEOidcConfiguration.LoginSlidingExpiryTimeOutInMinutes),
                    SlidingExpiration = true,
                    SessionStore = authenticationSessionStore,
                });
                app.UseOpenIdConnectAuthentication(openIdConnectConfiguration.GetOpenIdConnectOptions());
            }
            else
            {
                DependencyResolver.Current.GetService<ILog>().Info("Using WsFederation Authentication");
                app.UseCookieAuthentication(new CookieAuthenticationOptions());
                var adfsConfiguration = DependencyResolver.Current.GetService<IADFSConfiguration>();
                app.UseWsFederationAuthentication(adfsConfiguration.GetADFSOptions());
            }
        }
    }
}