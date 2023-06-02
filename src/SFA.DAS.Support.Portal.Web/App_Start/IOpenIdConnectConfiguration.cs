using OpenAthens.Owin.Security.OpenIdConnect;

namespace SFA.DAS.Support.Portal.Web
{
    public interface IOpenIdConnectConfiguration
    {
        OpenIdConnectAuthenticationOptions GetOpenIdConnectOptions();
    }
}
