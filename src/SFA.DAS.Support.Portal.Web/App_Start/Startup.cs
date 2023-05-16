using System.Collections.Generic;
using System;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Newtonsoft.Json;
using Owin;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.Web.Api.Models;
using SFA.DAS.Support.Portal.Web.App_Start;
using SFA.DAS.Support.Portal.Web.Constants;
using SFA.DAS.Support.Portal.Web.Interfaces;
using SFA.DAS.Support.Portal.Web.Settings;
using OpenAthens.Owin.Security.OpenIdConnect;
using SFA.DAS.Support.Portal.Web.Extensions;
using OpenIdConnectMessage = Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectMessage;

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

                var dfESignInConfig = DependencyResolver.Current.GetService<IDfESignInServiceConfiguration>();
                var oidcRedirectUrl = webConfiguration.Authentication.Realm + "sign-in";
                app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
                {
                    Authority = dfESignInConfig.DfEOidcConfiguration.BaseUrl,
                    ClientId = dfESignInConfig.DfEOidcClientConfiguration.ClientId,
                    ClientSecret = dfESignInConfig.DfEOidcClientConfiguration.Secret,
                    GetClaimsFromUserInfoEndpoint = true,
                    PostLogoutRedirectUri = oidcRedirectUrl,
                    RedirectUri = oidcRedirectUrl,
                    ResponseType = OpenIdConnectResponseType.Code,
                    Scope = "openid email profile organisation organisationid",
                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        SecurityTokenValidated = async notification =>
                        {
                            await PopulateAccountsClaim(notification);
                        }
                    },
                });
            }
            else
            {
                DependencyResolver.Current.GetService<ILog>().Info("Using WsFederation Authentication");

                var adfsConfiguration = DependencyResolver.Current.GetService<IADFSConfiguration>();
                app.UseWsFederationAuthentication(adfsConfiguration.GetADFSOptions());
            }
        }

        /// <summary>
        /// Method to populate/add the OpenIdConnect claims to the initial identity.
        /// </summary>
        /// <param name="notification">Security Token.</param>
        /// <returns>Task.</returns>
        private static async Task PopulateAccountsClaim(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            DependencyResolver.Current.GetService<ILog>().Info("SecurityTokenValidated notification called");

            #region "READ THE CLAIMS FROM AUTHENTICATION TICKET"
            var userId = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.NameIdentifier);
            var emailAddress = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.Email);
            var firstName = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.GivenName);
            var lastName = notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.Surname);
            var userOrganization = JsonConvert.DeserializeObject<Organisation>(notification.AuthenticationTicket.Identity.GetClaimValue(ClaimName.Organisation));
            #endregion

            var ukPrn = userOrganization.UkPrn != null ? Convert.ToInt64(userOrganization.UkPrn) : 0;

            // when the UserId and UserOrgId are available then fetch additional claims from DfESignIn Api Service.
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(userOrganization.Id.ToString()))
                await DfEPublicApi(notification, userId, userOrganization.Id.ToString());

            System.Web.HttpContext.Current.Items.Add(ClaimsIdentity.DefaultNameClaimType, ukPrn);
            System.Web.HttpContext.Current.Items.Add(CustomClaimsIdentity.DisplayName, $"{firstName} {lastName}");

            notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, ukPrn.ToString()));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(CustomClaimsIdentity.DisplayName, $"{firstName} {lastName}"));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(CustomClaimsIdentity.UkPrn, ukPrn.ToString()));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Email, emailAddress));
            notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Name, $"{firstName} {lastName}"));
        }

        /// <summary>
        /// Method to get additional claims from DfESignIn Api Service.
        /// </summary>
        /// <param name="notification">Security Token.</param>
        /// <param name="userId">string User Identifier.</param>
        /// <param name="userOrgId">string User Organization Identifier.</param>
        /// <returns>Task.</returns>
        private static async Task DfEPublicApi(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification, string userId, string userOrgId)
        {
            DependencyResolver.Current.GetService<ILog>().Info("Calling DfESignIn Api for additional roles");

            var dfESignInService = DependencyResolver.Current.GetService<IDfESignInService>();

            // fetch the additional claims from DfESignIn Services.
            var apiServiceResponse = await dfESignInService.Get<ApiServiceResponse>(userId, userOrgId);

            if (apiServiceResponse != null)
            {
                var roleClaims = new List<Claim>();
                foreach (var role in apiServiceResponse.Roles.Where(role => role.Status.Id.Equals(1)))
                {
                    // add to current identity because you cannot have multiple identities
                    roleClaims.Add(new Claim(ClaimName.RoleCode, role.Code, ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleId, role.Id.ToString(), ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleName, role.Name, ClaimTypes.Role, notification.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimName.RoleNumericId, role.NumericId.ToString(), ClaimTypes.Role, notification.Options.ClientId));

                    // add service role claim to initial identity.
                    notification.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                }

                // add service role claims to identity.
                notification.AuthenticationTicket.Identity.AddClaims(roleClaims);
            }
        }
    }
}