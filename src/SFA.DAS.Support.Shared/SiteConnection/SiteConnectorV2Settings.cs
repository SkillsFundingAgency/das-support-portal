using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace SFA.DAS.Support.Shared.SiteConnection
{

    public interface ISiteConnectorV2Settings
    {
        string BaseUrl { get; set; }
        string Tenant { get; set; }
        string IdentifierUri { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorV2Settings : ISiteConnectorV2Settings
    {
        public string BaseUrl { get; set; }

        public string IdentifierUri { get; set; }

        public string Tenant { get; set; }
    }


    public interface ISiteConnectorV3Settings
    {
        string BaseUrl { get; set; }
        string Tenant { get; set; }
        string IdentifierUri { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorV3Settings : ISiteConnectorV3Settings
    {
        public string BaseUrl { get; set; }

        public string IdentifierUri { get; set; }

        public string Tenant { get; set; }
    }



    public interface ISiteConnectorV7Settings
    {
        List<SiteConnectorV7Settings> SiteConnectorV3 { get; set; }       
    }

    [ExcludeFromCodeCoverage]
    public sealed class SiteConnectorV7Settings : ISiteConnectorV7Settings
    {       
        public List<SiteConnectorV7Settings> SiteConnectorV3 { get; set; }
    }





    //[ExcludeFromCodeCoverage]
    //public sealed class SiteConnectorV3Settings : ISiteConnectorV3Settings
    //{
    //   public List<SiteConnectorV2Settings> SiteConnectorV3 { get; set; }
    //}


    public interface ISiteConnectorV33Settings
    {
        public List<SiteConnectorV3Settings> SiteConnectorV33 { get; set; }
        //public string BaseUrl { get; set; }

        //public string IdentifierUri { get; set; }

        //public string Tenant { get; set; }
    }

    public class SiteConnectorV33Settings
    {
        public List<SiteConnectorV3Settings> SiteConnectorV33 { get; set; }
        public string BaseUrl { get; set; }

        public string IdentifierUri { get; set; }

        public string Tenant { get; set; }
    }



    //public interface ISupportConnectorV2
    //{
    //    SiteConnectorV2Settings[] SiteConnectorV2Settings { get; set; }
    //}

    //public class SupportConnectorV2 : ISupportConnectorV2
    //{
    //    public SiteConnectorV2Settings[] SiteConnectorV2Settings { get; set; }
    //}
}
