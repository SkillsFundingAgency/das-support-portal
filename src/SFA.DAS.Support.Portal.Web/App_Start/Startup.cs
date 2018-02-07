using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SFA.DAS.NLog.Logger;

namespace SFA.DAS.Support.Portal.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            DependencyResolver.Current.GetService<ILog>().Debug("Configuring Authentication");

            var adfsConfiguration = DependencyResolver.Current.GetService<IADFSConfiguration>();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseWsFederationAuthentication(adfsConfiguration.GetADFSOptions());
        }
    }
}