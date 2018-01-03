using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.ViewModels;
using SFA.DAS.Support.Shared.SearchIndexModel;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class MappingService : IMappingService
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;

        public MappingService(ILog logger)
        {
            _logger = logger;
            Configuration = Config();
            _mapper = Configuration.CreateMapper();
        }

        public MapperConfiguration Configuration { get; private set; }

        public TDest Map<TSource, TDest>(TSource source)
        {
            try
            {
                return _mapper.Map<TSource, TDest>(source);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
                throw;
            }
        }

        public TDest Map<TSource, TDest>(TSource source, Action<IMappingOperationOptions<TSource, TDest>> opts)
        {
            try
            {
                return _mapper.Map(source, opts);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error mapping objects");
                throw;
            }
        }

        private void CreateEmployerUserSearchResultsMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<EmployerUserSearchResponse, SearchResultsViewModel>()
                .ForMember(x => x.AccountSearchResults, y => y.Ignore())
                .ForMember(x => x.UserSearchResults, y => y.Ignore())
                .ForMember(x => x.ErrorMessage, y => y.Ignore())
                .ForMember(x => x.TotalAccountSearchItems, y => y.Ignore())
                .ForMember(x => x.TotalUserSearchItems, y => y.Ignore())
                .ForMember(x => x.SearchType, y => y.Ignore());
        }

        private void CreateSearchTableResultsMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<SearchResponse, SearchResultsViewModel>()
                .ForMember(x => x.ErrorMessage, y => y.Ignore())
                .ForMember(x => x.AccountSearchResults, o => o.MapFrom(x => x.AccountSearchResult == null ? new List<AccountSearchModel>() : x.AccountSearchResult.Results))
                .ForMember(x => x.TotalAccountSearchItems, o => o.MapFrom(x => x.AccountSearchResult.TotalCount))
                .ForMember(x => x.UserSearchResults, o => o.MapFrom(x => x.UserSearchResult == null ? new List<UserSearchModel>() : x.UserSearchResult.Results))
                .ForMember(x => x.TotalUserSearchItems, o => o.MapFrom(x => x.UserSearchResult.TotalCount))
               .ForMember(x => x.LastPage, o => o.MapFrom(x => GetLastPage(x)));
        }

        private int GetLastPage(SearchResponse response)
        {
            var lastPage = response.SearchType == SearchCategory.Account ? response.AccountSearchResult?.LastPage : response.UserSearchResult?.LastPage;
            return lastPage.GetValueOrDefault();
        }

        private MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                CreateEmployerUserSearchResultsMappings(cfg);
                CreateSearchTableResultsMappings(cfg);

            });
        }
    }
}