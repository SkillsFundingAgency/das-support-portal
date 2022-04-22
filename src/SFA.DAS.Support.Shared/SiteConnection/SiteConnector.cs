using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Shared.Authentication;
using SFA.DAS.Support.Shared.Discovery;

namespace SFA.DAS.Support.Shared.SiteConnection
{
    public class SiteConnector : ISiteConnector
    {
        private const string TheHttpClientMayNotBeNull = "The Http client may not be null";
        private readonly HttpClient _client;
        private readonly IClientAuthenticator _clientAuthenticator;
        private readonly List<IHttpStatusCodeStrategy> _handlers;
        private readonly ILog _logger;
        private readonly ISiteConnectorSettings _settings;
        private readonly ISupportEASSiteConnectorSettings _supportEASSiteConnectorSettings;
        private readonly ISupportCommitmentsSiteConnectorSettings _supportCommitmentsSiteConnectorSettings;
        private readonly ISupportEmployerUsersSiteConnectorSettings _supportEmployerUsersSiteConnectorSettings;

        public SiteConnector(HttpClient client,
            IClientAuthenticator clientAuthenticator,
            ISiteConnectorSettings settings,
            ISupportEASSiteConnectorSettings supportEASSiteConnectorSettings,
            ISupportCommitmentsSiteConnectorSettings supportCommitmentsSiteConnectorSettings,
            ISupportEmployerUsersSiteConnectorSettings supportEmployerUsersSiteConnectorSettings,
            List<IHttpStatusCodeStrategy> handlers,
            ILog logger)
        {
            _client = client ?? throw new ArgumentNullException(TheHttpClientMayNotBeNull);
            _clientAuthenticator = clientAuthenticator ?? throw new ArgumentNullException(nameof(clientAuthenticator));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _supportEASSiteConnectorSettings = supportEASSiteConnectorSettings ?? throw new ArgumentNullException(nameof(supportEASSiteConnectorSettings));
            _supportCommitmentsSiteConnectorSettings = supportCommitmentsSiteConnectorSettings ?? throw new ArgumentNullException(nameof(supportCommitmentsSiteConnectorSettings));
            _supportEmployerUsersSiteConnectorSettings = supportEmployerUsersSiteConnectorSettings ?? throw new ArgumentNullException(nameof(supportEmployerUsersSiteConnectorSettings));
            if (!handlers.Any()) throw new ArgumentException(nameof(handlers));
            _handlers = handlers;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public HttpStatusCode LastCode { get; set; }


        public string LastContent { get; set; }
        public Exception LastException { get; set; }
        public HttpStatusCodeDecision HttpStatusCodeDecision { get; set; }

        public async Task<T> Download<T>(string url, SupportServiceResourceKey resourceKey) where T : class
        {
            return await Download<T>(new Uri(url), resourceKey);
        }

        public async Task<string> Download(Uri url, SupportServiceResourceKey resourceKey)
        {
            return await Download<string>(url, resourceKey);
        }

        public async Task<T> Upload<T>(Uri uri, string content, SupportServiceResourceKey resourceKey) where T : class
        {
            await EnsureClientAuthorizationHeader(resourceKey);
            //await EnsureClientAuthorizationHeader();
            try
            {

                var postContent = new StringContent(content);

                var response = await _client.PostAsync(uri, postContent);

                LastContent = await response.Content.ReadAsStringAsync();

                HttpStatusCodeDecision = ExamineResponse(response);

                switch (HttpStatusCodeDecision)
                {
                    case HttpStatusCodeDecision.HandleException:
                        LastException = LastException ??
                                        new Exception($"An enforced exception has occured in {nameof(SiteConnector)}");
                        throw LastException;

                    case HttpStatusCodeDecision.ReturnNull:
                        return null;

                    default:


                        if (typeof(T) == typeof(string)) return LastContent as T;

                        return JsonConvert.DeserializeObject<T>(LastContent);
                }
            }
            catch (Exception e)
            {
                _logger.Debug($"Call to sub site failed {nameof(SiteConnector)} with {e.HResult} returning null response. Stack: {e.StackTrace}");
                return null;
            }
        }
        public async Task<T> Upload<T>(Uri uri, IDictionary<string, string> formData) where T : class
        {            
            await EnsureClientAuthorizationHeader();

            try
            {
                var response = await _client.PostAsync(uri, new FormUrlEncodedContent(formData));

                LastContent = await response.Content.ReadAsStringAsync();

                HttpStatusCodeDecision = ExamineResponse(response);

                switch (HttpStatusCodeDecision)
                {
                    case HttpStatusCodeDecision.HandleException:
                        LastException = LastException ??
                                        new Exception($"An enforced exception has occured in {nameof(SiteConnector)}");
                        throw LastException;

                    case HttpStatusCodeDecision.ReturnNull:
                        return null;

                    default:


                        if (typeof(T) == typeof(string)) return LastContent as T;

                        return JsonConvert.DeserializeObject<T>(LastContent);
                }
            }
            catch (Exception e)
            {
                _logger.Debug($"Call to sub site failed {nameof(SiteConnector)} with {e.HResult} returning null response. Stack: {e.StackTrace}");
                return null;
            }
        }


        public async Task<T> Download<T>(Uri uri, SupportServiceResourceKey resourceKey) where T : class
        {
            //await EnsureClientAuthorizationHeader();
            await EnsureClientAuthorizationHeader(resourceKey);

            try
            {
                var response = await _client.GetAsync(uri);

                LastContent = await response.Content.ReadAsStringAsync();

                HttpStatusCodeDecision = ExamineResponse(response);

                switch (HttpStatusCodeDecision)
                {
                    case HttpStatusCodeDecision.RethrowException:
                        var exception = LastException ??
                                        new Exception(
                                            $"A manufactured exception has occured in {nameof(SiteConnector)} after receiving status code {LastCode} and Content of: {LastContent}");
                        _logger.Error(exception,
                            $"Forced Exception from {nameof(SiteConnector)} recieving {LastCode} and Content of: {LastContent} and Content of: {LastContent}");
                        throw exception;

                    case HttpStatusCodeDecision.HandleException:
                        var generatedException = LastException ??
                                                 new Exception(
                                                     $"A manufactured exception has occured in {nameof(SiteConnector)} after receiving status code {LastCode}");
                        _logger.Error(generatedException,
                            $"Exception from {nameof(SiteConnector)} recieving {LastCode} and Content of: {LastContent} from {uri}");
                        return null;

                    case HttpStatusCodeDecision.ReturnNull:
                        _logger.Debug(
                            $"Ignoring Return value from {nameof(SiteConnector)} receiving {LastCode} and Content of: {LastContent}");
                        return null;


                    default:
                        _logger.Info($"Successful call to site from {nameof(SiteConnector)} recieving {LastCode}");
                        var content = await response.Content.ReadAsStringAsync();
                        if (typeof(T) == typeof(string)) return content as T;
                        return JsonConvert.DeserializeObject<T>(content);
                }
            }
            catch (Exception e)
            {
                _logger.Debug($"Call to sub site failed {nameof(SiteConnector)} with {e.HResult} returning null response. Stack: {e.StackTrace}");
                return null;
            }
        }

        private async Task EnsureClientAuthorizationHeader()
        {
            try
            {
                if (_client.DefaultRequestHeaders.Authorization == null)
                {
                    var token = await _clientAuthenticator.Authenticate(_settings.ClientId,
                        _settings.ClientSecret,
                        _settings.IdentifierUri,
                        _settings.Tenant);
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error obtaining active directory token. Communication with the sub sites is not possible.");
                _client.DefaultRequestHeaders.Authorization = null;
            }
        }

        private async Task EnsureClientAuthorizationHeader(SupportServiceResourceKey resourceKey)
        {
            try
            {               

                if (_client.DefaultRequestHeaders.Authorization == null)
                {
                    switch (resourceKey)
                    {
                        case SupportServiceResourceKey.EmployerAccount:
                        case SupportServiceResourceKey.EmployerAccountHeader:                        
                        case SupportServiceResourceKey.EmployerAccountTeam:
                        case SupportServiceResourceKey.EmployerAccountFinance:
                        case SupportServiceResourceKey.EmployerAccountPayeSchemeLevys:
                        case SupportServiceResourceKey.EmployerAccountFinanceChallenge:
                            {
                                var employerToken = IsClientCredentialConfiguration(_supportEASSiteConnectorSettings.ClientId, _supportEASSiteConnectorSettings.ClientSecret, _supportEASSiteConnectorSettings.Tenant)
                                  ? await  GetClientCredentialAuthenticationResult(_supportEASSiteConnectorSettings.ClientId,
                                              _supportEASSiteConnectorSettings.ClientSecret,
                                              _supportEASSiteConnectorSettings.IdentifierUri,
                                              _supportEASSiteConnectorSettings.Tenant)
                                  : await GetManagedIdentityAuthenticationResult(_supportEASSiteConnectorSettings.IdentifierUri);                               
                                
                                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", employerToken);
                                break;
                            }
                        case SupportServiceResourceKey.CommitmentSearch:
                        case SupportServiceResourceKey.CommitmentApprenticeDetail:
                        case SupportServiceResourceKey.CommitmentCohortDetail:
                            {
                                var commitmentToken = IsClientCredentialConfiguration(_supportCommitmentsSiteConnectorSettings.ClientId,
                                    _supportCommitmentsSiteConnectorSettings.ClientSecret, _supportCommitmentsSiteConnectorSettings.Tenant)
                                  ? await GetClientCredentialAuthenticationResult(_supportCommitmentsSiteConnectorSettings.ClientId,
                                              _supportCommitmentsSiteConnectorSettings.ClientSecret,
                                              _supportCommitmentsSiteConnectorSettings.IdentifierUri,
                                              _supportCommitmentsSiteConnectorSettings.Tenant)
                                  : await GetManagedIdentityAuthenticationResult(_supportCommitmentsSiteConnectorSettings.IdentifierUri);

                                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", commitmentToken);
                                break;
                            }
                        case SupportServiceResourceKey.EmployerUser:
                        case SupportServiceResourceKey.EmployerUserHeader:
                        case SupportServiceResourceKey.EmployerUserAccounts:
                            {
                                var employerUsersToken = IsClientCredentialConfiguration(_supportEmployerUsersSiteConnectorSettings.ClientId,
                                    _supportEmployerUsersSiteConnectorSettings.ClientSecret, _supportEmployerUsersSiteConnectorSettings.Tenant)
                                  ? await GetClientCredentialAuthenticationResult(_supportEmployerUsersSiteConnectorSettings.ClientId,
                                              _supportEmployerUsersSiteConnectorSettings.ClientSecret,
                                              _supportEmployerUsersSiteConnectorSettings.IdentifierUri,
                                              _supportEmployerUsersSiteConnectorSettings.Tenant)
                                  : await GetManagedIdentityAuthenticationResult(_supportEmployerUsersSiteConnectorSettings.IdentifierUri);

                                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", employerUsersToken);
                                break;
                            }
                        default:
                            {

                                var token = await _clientAuthenticator.Authenticate(_settings.ClientId,
                                    _settings.ClientSecret,
                                    _settings.IdentifierUri,
                                    _settings.Tenant);
                                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                break;
                            }
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error obtaining active directory token. Communication with the sub sites is not possible.");
                _client.DefaultRequestHeaders.Authorization = null;
            }
        }

        private async Task<string> GetClientCredentialAuthenticationResult(string clientId, string clientSecret, string identifierUri, string tenant)
        {
            return await _clientAuthenticator.Authenticate(clientId, clientSecret, identifierUri, tenant);
        }
       
        private async Task<string> GetManagedIdentityAuthenticationResult(string resource)
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();            
            return await azureServiceTokenProvider.GetAccessTokenAsync(resource);
        }


        private HttpStatusCodeDecision ExamineResponse(HttpResponseMessage message)
        {
            LastException = null;
            LastCode = message.StatusCode;

            var handlerOptions = _handlers.Where(x => x.Low <= LastCode &&
                                                      x.High >= LastCode &&
                                                      x.ExcludeStatuses.All(y => y != LastCode)).ToList();

            if (handlerOptions.Any()) return handlerOptions.First().Handle(_client, LastCode);

            return HttpStatusCodeDecision.Continue;
        }

        private bool IsClientCredentialConfiguration(string clientId, string clientSecret, string tenant)
        {
            return !string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret) && !string.IsNullOrEmpty(tenant);
        }
    }
}