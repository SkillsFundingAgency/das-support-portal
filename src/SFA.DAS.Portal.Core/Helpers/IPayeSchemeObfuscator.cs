namespace SFA.DAS.Portal.Core.Helpers
{
    public interface IPayeSchemeObfuscator
    {
        string ObscurePayeScheme(string payeSchemeId);
    }
}