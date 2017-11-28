using System.Web.Mvc;
using Microsoft.Azure;
using SFA.DAS.Portal.Core.Services;

namespace SFA.DAS.Portal.Web
{
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