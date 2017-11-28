using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Newtonsoft.Json;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Portal.Core.Services;
using SFA.DAS.Portal.Web.Settings;

namespace SFA.DAS.Portal.Web
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var logger = DependencyResolver.Current.GetService<ILog>();
            var roleSettings = DependencyResolver.Current.GetService<IRoleSettings>();
            var authSettings = DependencyResolver.Current.GetService<IAuthSettings>();

            logger.Debug("Configuring Authentication");
            if (!string.IsNullOrEmpty(authSettings.AdfsMetadata))
            {
                app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

                app.UseCookieAuthentication(new CookieAuthenticationOptions());
                app.UseWsFederationAuthentication(
                    new WsFederationAuthenticationOptions
                    {
                        Wtrealm = authSettings.Realm,
                        MetadataAddress = authSettings.AdfsMetadata,
                        Notifications = new WsFederationAuthenticationNotifications
                        {
                            SecurityTokenValidated = (notification) =>
                            {
                                logger.Debug("SecurityTokenValidated");
                                try
                                {
                                    logger.Debug("Authentication Properties", new Dictionary<string, object>
                                    {
                                        //{"ticket.properties", notification.AuthenticationTicket?.Properties?.Dictionary },
                                        {"claims", JsonConvert.SerializeObject(notification.AuthenticationTicket.Identity.Claims.Select(x=>new {x.Value, x.ValueType, x.Type})) },
                                        {"authenticeation-type", notification.AuthenticationTicket.Identity.AuthenticationType },
                                        {"role-type", notification.AuthenticationTicket.Identity.RoleClaimType }

                                    });
                                    if (notification.AuthenticationTicket.Identity.HasClaim("http://schemas.xmlsoap.org/claims/Group", roleSettings.Tier2Claim))
                                    {
                                        logger.Debug("Adding Tier2 Role");
                                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, roleSettings.T2Role));
                                        logger.Debug("Adding ConsoleUser Role");
                                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, roleSettings.ConsoleUserRole));
                                    }
                                    else if (notification.AuthenticationTicket.Identity.HasClaim("http://schemas.xmlsoap.org/claims/Group", roleSettings.GroupClaim))
                                    {
                                        logger.Debug("Adding ConsoleUser Role");
                                        notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, roleSettings.ConsoleUserRole));
                                    }
                                    else
                                    {
                                        throw new System.IdentityModel.Tokens.SecurityTokenValidationException();
                                    }

                                    var userEmail = notification.AuthenticationTicket.Identity.Claims.Single(x => x.Type == ClaimTypes.Upn).Value;
                                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email, userEmail));
                                }
                                catch (Exception ex)
                                {
                                    logger.Error(ex, "callback error");
                                }
                                logger.Debug("End of callback");

                                return Task.FromResult(0);
                            },
                            SecurityTokenReceived = nx =>
                            {
                                logger.Debug("SecurityTokenReceived");
                                return Task.FromResult(0);
                            },
                            AuthenticationFailed = nx =>
                            {
                                logger.Debug("AuthenticationFailed");
                                return Task.FromResult(0);
                            },
                            MessageReceived = nx =>
                            {
                                logger.Debug("MessageReceived");
                                return Task.FromResult(0);
                            },
                            RedirectToIdentityProvider = nx =>
                            {
                                logger.Debug("RedirectToIdentityProvider");
                                return Task.FromResult(0);
                            },
                        }
                    });
            }
        }
    }
}
