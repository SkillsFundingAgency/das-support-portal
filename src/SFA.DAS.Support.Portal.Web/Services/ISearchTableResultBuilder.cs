using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public interface ISearchTableResultBuilder
    {
        SearchTableResultViewModel CreateTableResult(SearchResponse model);
    }
}