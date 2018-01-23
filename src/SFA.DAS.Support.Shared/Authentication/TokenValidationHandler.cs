using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Protocols;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.SiteConnection;

namespace SFA.DAS.Support.Shared.Authentication
{
    [ExcludeFromCodeCoverage]
    public class TokenValidationHandler : DelegatingHandler
    {
        private const string AuthorityBaseUrl = "https://login.microsoftonline.com/";
        private static string _audience = string.Empty;
        private readonly string _authority;
        private readonly ILog _logger;
        private readonly string _scope;
        private readonly string scopeClaimType = "http://schemas.microsoft.com/identity/claims/scope";

        private string _issuer = string.Empty;
        private List<SecurityToken> _signingTokens;
        private DateTime _stsMetadataRetrievalTime = DateTime.MinValue;

        public TokenValidationHandler(ISiteValidatorSettings settings, ILog logger)
        {
            _logger = logger;
            _audience = settings.Audience;
            _authority = $"{AuthorityBaseUrl}{settings.Tenant}";
            _scope = settings.Scope;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string jwtToken = null;
            var authHeader = request.Headers.Authorization;
            if (authHeader != null) jwtToken = authHeader.Parameter;

            if (jwtToken == null)
            {
                _logger.Warn($"No token was found in the Authorization header");
                var response = BuildResponseErrorMessage(HttpStatusCode.Unauthorized);
                return response;
            }

            if (DateTime.UtcNow.Subtract(_stsMetadataRetrievalTime).TotalHours > 24
                || string.IsNullOrEmpty(_issuer)
                || _signingTokens == null)
                try
                {
                    var stsDiscoveryEndpoint = $"{_authority}/.well-known/openid-configuration";
                    var configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint);
                    var config = await configManager.GetConfigurationAsync(cancellationToken);
                    _issuer = config.Issuer;
                    _signingTokens = config.SigningTokens.ToList();
                    _stsMetadataRetrievalTime = DateTime.UtcNow;
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Exception occured obtaining configuration from authority");
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }

            var issuer = _issuer;
            var signingTokens = _signingTokens;
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidAudience = _audience,
                ValidIssuer = issuer,
                IssuerSigningTokens = signingTokens,
                CertificateValidator = X509CertificateValidator.None
            };

            try
            {
                SecurityToken validatedToken = new JwtSecurityToken();
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out validatedToken);

                Thread.CurrentPrincipal = claimsPrincipal;

                if (HttpContext.Current != null) HttpContext.Current.User = claimsPrincipal;

                if (ClaimsPrincipal.Current.FindFirst(scopeClaimType) != null &&
                    ClaimsPrincipal.Current.FindFirst(scopeClaimType).Value != _scope)
                {
                    _logger.Warn($"The supplied token does not provide the required scope");
                    var response = BuildResponseErrorMessage(HttpStatusCode.Forbidden);
                    return response;
                }

                return await base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException e)
            {
                _logger.Error(e, $"The supplied token is not valid. ");
                var response = BuildResponseErrorMessage(HttpStatusCode.Unauthorized);
                return response;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"An exception has occurred validating the supplied token");
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        private HttpResponseMessage BuildResponseErrorMessage(HttpStatusCode statusCode)
        {
            var response = new HttpResponseMessage(statusCode);
            var parameter = "authorization_uri=\"" + _authority + "\"" + "," + "resource_id=" + _audience;
            var authenticateHeader = new AuthenticationHeaderValue("Bearer", parameter);
            response.Headers.WwwAuthenticate.Add(authenticateHeader);
            return response;
        }
    }
}