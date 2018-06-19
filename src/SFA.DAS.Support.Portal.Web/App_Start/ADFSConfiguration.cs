using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.WsFederation;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Web.Settings;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class ADFSConfiguration : IADFSConfiguration
    {
        private readonly IAuthSettings _authSettings;
        private readonly ILog _logger;
        private readonly IRoleSettings _rolesSettings;

        public ADFSConfiguration(ILog logger, IAuthSettings authSettings, IRoleSettings rolesSettings)
        {
            _authSettings = authSettings;
            _rolesSettings = rolesSettings;
            _logger = logger;
        }


        public WsFederationAuthenticationOptions GetADFSOptions()
        {
            return new WsFederationAuthenticationOptions
            {
                Wtrealm = _authSettings.Realm,
                MetadataAddress = _authSettings.AdfsMetadata,
                Notifications = Notifications()
            };
        }

        private WsFederationAuthenticationNotifications Notifications()
        {
            return new WsFederationAuthenticationNotifications
            {
                SecurityTokenValidated = OnSecurityTokenValidated,
                SecurityTokenReceived = nx => OnSecurityTokenReceived(),
                AuthenticationFailed = nx => OnAuthenticationFailed(),
                MessageReceived = nx => OnMessageReceived(),
                RedirectToIdentityProvider = nx => OnRedirectToIdentityProvider()
            };
        }

        private Task OnRedirectToIdentityProvider()
        {
            _logger.Debug("RedirectToIdentityProvider");
            return Task.FromResult(0);
        }

        private Task OnMessageReceived()
        {
            _logger.Debug("MessageReceived");
            return Task.FromResult(0);
        }

        private Task OnSecurityTokenReceived()
        {
            _logger.Debug("SecurityTokenReceived");
            return Task.FromResult(0);
        }

        private Task OnAuthenticationFailed()
        {
            _logger.Debug("AuthenticationFailed");
            return Task.FromResult(0);
        }

        private Task OnSecurityTokenValidated(
            SecurityTokenValidatedNotification<WsFederationMessage, WsFederationAuthenticationOptions> notification)
        {
            _logger.Debug("SecurityTokenValidated");
            try
            {
                _logger.Debug("Authentication Properties", new Dictionary<string, object>
                {
                    {
                        "claims",
                        JsonConvert.SerializeObject(
                            notification.AuthenticationTicket.Identity.Claims.Select(x =>
                                new {x.Value, x.ValueType, x.Type}))
                    },
                    {
                        "authentication-type",
                        notification.AuthenticationTicket.Identity.AuthenticationType
                    },
                    {"role-type", notification.AuthenticationTicket.Identity.RoleClaimType}
                });
                if (notification.AuthenticationTicket.Identity.HasClaim("http://schemas.xmlsoap.org/claims/Group",
                    _rolesSettings.Tier2Claim))
                {
                    _logger.Debug("Adding Tier2 Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role,
                        _rolesSettings.T2Role));
                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role,
                        _rolesSettings.ConsoleUserRole));
                }
                else if (notification.AuthenticationTicket.Identity.HasClaim(
                    "http://schemas.xmlsoap.org/claims/Group", _rolesSettings.GroupClaim))
                {
                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role,
                        _rolesSettings.ConsoleUserRole));
                }
                else
                {
                    throw new SecurityTokenValidationException();
                }

                var userEmail = notification.AuthenticationTicket.Identity.Claims
                    .Single(x => x.Type == ClaimTypes.Upn).Value;
                notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email,
                    userEmail));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "callback error");
            }

            _logger.Debug("End of callback");

            return Task.FromResult(0);
        }
    }
}