using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SFA.DAS.Support.Portal.Core.Services;
using SFA.DAS.TokenService.Api.Client;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    public class LevySubmissionsApiConfiguration : ITokenServiceApiClientConfiguration
    {
        private readonly IProvideSettings _settings;

        public LevySubmissionsApiConfiguration(IProvideSettings settings)
        {
            _settings = settings;
        }

        public string ApiBaseUrl
        {
            get { return _settings.GetSetting("LevyApiBaseUrl"); }
            set { }
        }

        public string ClientId
        {
            get { return _settings.GetSetting("LevyApiClientId"); }
            set { }
        }

        public string ClientSecret
        {
            get { return _settings.GetSetting("LevyApiClientSecret"); }
            set { }
        }

        public string IdentifierUri
        {
            get { return _settings.GetSetting("LevyApiIdentifierUri"); }
            set { }
        }

        public string Tenant
        {
            get { return _settings.GetSetting("LevyApiTenant"); }
            set { }
        }

        public X509Certificate TokenCertificate
        {
            get
            {
                var thumbprint = _settings.GetNullableSetting("LevyTokenCertificate");
                if (string.IsNullOrWhiteSpace(thumbprint))
                {
                    return null;
                }

                var store = new X509Store(StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                try
                {

                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);

                    if (certificates.Count > 0)
                    {
                        return certificates[0];
                    }
                }
                finally
                {
                    store.Close();
                }

                throw new KeyNotFoundException($"Couldn't find the certificate for thumbprint '{thumbprint}'");
            }
            set { }
        }
    }
}