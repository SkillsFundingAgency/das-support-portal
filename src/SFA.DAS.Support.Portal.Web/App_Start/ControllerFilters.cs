using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using SFA.DAS.Support.Portal.Core.Services;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class ControllerFilters
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var roleSettings = DependencyResolver.Current.GetService<IRoleSettings>();
            filters.Add(new AuthorizeAttribute { Roles = roleSettings.ConsoleUserRole });

        }
    }
}