using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class ControllerFilters
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
#if DEBUG
#else
            var roleSettings = DependencyResolver.Current.GetService<IRoleSettings>();
            filters.Add(new AuthorizeAttribute { Roles = roleSettings.ConsoleUserRole });
#endif
        }
    }
}