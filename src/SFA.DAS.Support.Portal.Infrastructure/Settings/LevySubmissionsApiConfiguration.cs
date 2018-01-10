using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Portal.Infrastructure.Settings
{
    [ExcludeFromCodeCoverage]
    public class LevySubmissionsApiConfiguration : ILevySubmissionsApiConfiguration
    {

        [JsonRequired]
        public string ApiBaseUrl { get; set; }
        [JsonRequired]
        public string ClientId { get; set; }
        [JsonRequired]
        public string ClientSecret { get; set; }
        [JsonRequired]
        public string IdentifierUri { get; set; }
        [JsonRequired]
        public string Tenant { get; set; }
        [JsonIgnore]
        public X509Certificate TokenCertificate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(LevyTokenCertificatethumprint))
                {
                    return null;
                }
                var store = new X509Store(StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                try
                {
                    var certificates = store.Certificates.Find(X509FindType.FindByThumbprint, LevyTokenCertificatethumprint, false);
                    if (certificates.Count > 0)
                    {
                        return certificates[0];
                    }
                }
                finally
                {
                    store.Close();
                }
                throw new KeyNotFoundException($"Couldn't find the certificate for thumbprint '{LevyTokenCertificatethumprint}'");
            }
            set { }
        }
        [JsonRequired]
        public string LevyTokenCertificatethumprint { get; set; }
        
    }
}