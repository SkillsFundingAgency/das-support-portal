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
                AuthenticationFailed = nx => OnAuthenticationFailed(nx),
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

        private Task OnAuthenticationFailed(AuthenticationFailedNotification<WsFederationMessage, WsFederationAuthenticationOptions> nx)
        {
            var logReport = $"AuthenticationFailed, State: {nx.State}, Exception: {nx.Exception.GetBaseException().Message}, Protocol Message: Wct {nx.ProtocolMessage.Wct},\r\nWfresh {nx.ProtocolMessage.Wfresh},\r\nWhr {nx.ProtocolMessage.Whr},\r\nWp {nx.ProtocolMessage.Wp},\r\nWpseudo{nx.ProtocolMessage.Wpseudo},\r\nWpseudoptr {nx.ProtocolMessage.Wpseudoptr},\r\nWreq {nx.ProtocolMessage.Wreq},\r\nWfed {nx.ProtocolMessage.Wfed},\r\nWreqptr {nx.ProtocolMessage.Wreqptr},\r\nWres {nx.ProtocolMessage.Wres},\r\nWreply{nx.ProtocolMessage.Wreply},\r\nWencoding {nx.ProtocolMessage.Wencoding},\r\nWtrealm {nx.ProtocolMessage.Wtrealm},\r\nWresultptr {nx.ProtocolMessage.Wresultptr},\r\nWauth {nx.ProtocolMessage.Wauth},\r\nWattrptr{nx.ProtocolMessage.Wattrptr},\r\nWattr {nx.ProtocolMessage.Wattr},\r\nWa {nx.ProtocolMessage.Wa},\r\nIsSignOutMessage {nx.ProtocolMessage.IsSignOutMessage},\r\nIsSignInMessage {nx.ProtocolMessage.IsSignInMessage},\r\nWctx {nx.ProtocolMessage.Wctx},\r\n";
            _logger.Debug(logReport);
            return Task.FromResult(0);
        }

        private Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<WsFederationMessage, WsFederationAuthenticationOptions> notification)
        {
            _logger.Debug("SecurityTokenValidated");

            try
            {
                _logger.Debug("Authentication Properties", new Dictionary<string, object>
                {
                    {
                        "claims",
                        JsonConvert.SerializeObject(
                            notification.AuthenticationTicket.Identity.Claims.Select(x =>new {x.Value, x.ValueType, x.Type}))
                    },
                    {
                        "authentication-type",
                        notification.AuthenticationTicket.Identity.AuthenticationType
                    },
                    {"role-type", notification.AuthenticationTicket.Identity.RoleClaimType}
                });

                const string serviceClaimType = "http://service/service";

                if (notification.AuthenticationTicket.Identity.HasClaim(serviceClaimType, _rolesSettings.Tier2Claim))
                {
                    _logger.Debug("Adding Tier2 Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role,_rolesSettings.T2Role));

                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, _rolesSettings.ConsoleUserRole));
                }
                else if (notification.AuthenticationTicket.Identity.HasClaim(serviceClaimType, _rolesSettings.GroupClaim))
                {
                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role,_rolesSettings.ConsoleUserRole));
                }
                else
                {
                    throw new SecurityTokenValidationException();
                }

                var firstName = notification.AuthenticationTicket.Identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var lastName = notification.AuthenticationTicket.Identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                var userEmail = notification.AuthenticationTicket.Identity.Claims.Single(x => x.Type == ClaimTypes.Upn).Value;

                notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email,  userEmail));
                notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, $"{firstName} {lastName}"));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "IDAMS Authentication Callback Error");
            }

            _logger.Debug("End of callback");

            return Task.FromResult(0);
        }
    }
}