using Microsoft.Owin.Security.WsFederation;

namespace SFA.DAS.Support.Portal.Web
{
    public interface IADFSConfiguration
    {
        WsFederationAuthenticationOptions GetADFSOptions();
    }
}