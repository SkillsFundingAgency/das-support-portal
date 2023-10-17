using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security.Notifications;
using Newtonsoft.Json;
using OpenAthens.Owin.Security.OpenIdConnect;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.Support.Portal.Web.Api.Enums;
using SFA.DAS.Support.Portal.Web.Api.Models;
using SFA.DAS.Support.Portal.Web.Constants;
using SFA.DAS.Support.Portal.Web.Extensions;
using SFA.DAS.Support.Portal.Web.Interfaces;
using SFA.DAS.Support.Portal.Web.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using OpenIdConnectMessage = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectMessage;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class DfESignInOpenIdConnectConfiguration : IOpenIdConnectConfiguration
    {
        private const string ServiceClaimType = "http://service/service";
        private readonly ILog _logger;
        private readonly IRoleSettings _rolesSettings;
        private readonly IWebConfiguration _webConfiguration;
        private readonly IDfESignInServiceConfiguration _dfESignInServiceConfiguration;

        public DfESignInOpenIdConnectConfiguration(
            ILog logger,
            IRoleSettings rolesSettings,
            IWebConfiguration webConfiguration,
            IDfESignInServiceConfiguration dfESignInServiceConfiguration)
        {
            _rolesSettings = rolesSettings;
            _webConfiguration = webConfiguration;
            _dfESignInServiceConfiguration = dfESignInServiceConfiguration;
            _logger = logger;
        }

        public OpenIdConnectAuthenticationOptions GetOpenIdConnectOptions()
        {
            var oidcRedirectUrl = _webConfiguration.Authentication.Realm + "/sign-in";
            return new OpenIdConnectAuthenticationOptions
            {
                Authority = _dfESignInServiceConfiguration.DfEOidcConfiguration.BaseUrl,
                ClientId = _dfESignInServiceConfiguration.DfEOidcClientConfiguration.ClientId,
                ClientSecret = _dfESignInServiceConfiguration.DfEOidcClientConfiguration.Secret,
                GetClaimsFromUserInfoEndpoint = true,
                PostLogoutRedirectUri = oidcRedirectUrl,
                RedirectUri = oidcRedirectUrl,
                ResponseType = OpenIdConnectResponseType.Code,
                Scope = _dfESignInServiceConfiguration.DfEOidcConfiguration.Scopes,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = async notification =>
                    {
                        await PopulateAccountsClaim(notification);
                    }
                },
            };
        }

        /// <summary>
        /// Method to populate/add the OpenIdConnect claims to the initial identity.
        /// </summary>
        /// <param name="notification">Security Token.</param>
        /// <returns>Task.</returns>
        private async Task PopulateAccountsClaim(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            _logger.Info("SecurityTokenValidated notification called");

            // "READ THE CLAIMS FROM AUTHENTICATION TICKET"
            var userId = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.NameIdentifier);
            var firstName = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.GivenName);
            var lastName = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.Surname);
            var userOrganization = JsonConvert.DeserializeObject<Organisation>(notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.Organisation));
            
            var ukPrn = userOrganization.UkPrn != null ? Convert.ToInt64(userOrganization.UkPrn) : 0;

            // when the UserId and UserOrgId are available then fetch additional claims from DfESignIn Api Service.
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userOrganization.Id.ToString()))
                await DfEPublicApi(notification, userId, userOrganization.Id.ToString());

            System.Web.HttpContext.Current.Items.Add(ClaimsIdentity.DefaultNameClaimType, ukPrn);
            System.Web.HttpContext.Current.Items.Add(CustomClaimsIdentity.DisplayName, $"{firstName} {lastName}");

            notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, $"{firstName} {lastName}"));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(CustomClaimsIdentity.DisplayName, $"{firstName} {lastName}"));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(CustomClaimsIdentity.UkPrn, ukPrn.ToString()));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, $"{firstName} {lastName}"));
        }

        /// <summary>
        /// Method to get additional claims from DfESignIn Api Service.
        /// </summary>
        /// <param name="notification">Security Token.</param>
        /// <param name="userId">string User Identifier.</param>
        /// <param name="userOrgId">string User Organization Identifier.</param>
        /// <returns>Task.</returns>
        private async Task DfEPublicApi(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification, string userId, string userOrgId)
        {
            _logger.Info("Calling DfESignIn Api for additional roles");

            var dfESignInService = DependencyResolver.Current.GetService<IDfESignInService>();

            // fetch the additional claims from DfESignIn Services.
            var apiServiceResponse = await dfESignInService.Get<ApiServiceResponse>(userId, userOrgId);

            if (apiServiceResponse != null)
            {
                var roleClaims = new List<Claim>();
                foreach (var role in apiServiceResponse.Roles.Where(role => role.Status.Id.Equals((int)RoleStatus.Active)))
                {
                    // add to current identity because you cannot have multiple identities
                    roleClaims.Add(new Claim(ClaimName.RoleCode, role.Code, ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleId, role.Id.ToString(), ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleName, role.Name, ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleNumericId, role.NumericId.ToString(), ClaimTypes.Role, notification.Options.ClientId));

                    // add service role claim to initial identity.
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ServiceClaimType, role.Code));
                }

                // add service role claims to identity.
                notification.AuthenticationTicket.Identity.AddClaims(roleClaims);
            }

            // add additional role(s) to the original identity.
            MapIdentityClaimRole(notification);
        }

        /// <summary>
        /// Method to add additional role to the initial identity.
        /// </summary>
        /// <param name="notification">Security Token.</param>
        /// <exception cref="SecurityTokenValidationException"></exception>
        private void MapIdentityClaimRole(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            try
            {
                _logger.Debug("Authentication Properties", new Dictionary<string, object>
                {
                    {
                        "claims", JsonConvert.SerializeObject(notification.AuthenticationTicket.Identity.Claims.Select(x => new
                        {
                            x.Value, 
                            x.ValueType, 
                            x.Type
                        }))
                    },
                    {
                        "authentication-type", notification.AuthenticationTicket.Identity.AuthenticationType
                    },
                    {
                        "role-type", notification.AuthenticationTicket.Identity.RoleClaimType
                    }
                });

                if (notification.AuthenticationTicket.Identity.HasClaim(ServiceClaimType, _rolesSettings.Tier2Claim))
                {
                    _logger.Debug("Adding Tier2 Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, _rolesSettings.T2Role));

                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, _rolesSettings.ConsoleUserRole));
                }
                else if (notification.AuthenticationTicket.Identity.HasClaim(ServiceClaimType, _rolesSettings.GroupClaim))
                {
                    _logger.Debug("Adding ConsoleUser Role");
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, _rolesSettings.ConsoleUserRole));
                }
                else
                {
                    throw new SecurityTokenValidationException();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "OpenIdConnect Authentication Callback Error");
            }
        }
    }
}
