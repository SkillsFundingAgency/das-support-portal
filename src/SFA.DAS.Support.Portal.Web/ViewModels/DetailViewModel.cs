using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Support.Portal.Core.Domain.Model;

namespace SFA.DAS.Support.Portal.Web.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class DetailViewModel
    {
        public EmployerUser User { get; set; }
        public string SearchUrl { get; set; }
    }
}