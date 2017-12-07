using System.Web.Mvc;
using Microsoft.Azure;
using SFA.DAS.Support.Portal.Core.Services;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Support.Portal.Web
{
    [ExcludeFromCodeCoverage]
    public class ControllerFilters
    {
        private static readonly string AdfsMetadata = CloudConfigurationManager.GetSetting("ida_ADFSMetadata");

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            var roleSettings = DependencyResolver.Current.GetService<IRoleSettings>();

            //filters.Add(new HandleErrorAttribute());
            if (!System.Diagnostics.Debugger.IsAttached && !string.IsNullOrEmpty(AdfsMetadata.Trim()))
            {
                filters.Add(new AuthorizeAttribute { Roles = roleSettings.ConsoleUserRole });
            }
        }
    }
}