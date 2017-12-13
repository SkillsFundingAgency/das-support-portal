using System;
using System.Linq;
using AutoMapper;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices.Responses;
using SFA.DAS.Support.Portal.Web.ViewModels;

namespace SFA.DAS.Support.Portal.Web.Services
{
    public class MappingService : IMappingService
    {
        private readonly ILog _logger;
        private readonly IMapper _mapper;
        private readonly ISearchTableResultBuilder _searchTableResultBuilder;

        public MappingService(ILog logger, ISearchTableResultBuilder searchTableResultBuilder)
        {
            _logger = logger;
            Configuration = Config();
            _mapper = Configuration.CreateMapper();
            _searchTableResultBuilder = searchTableResultBuilder;
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
                .ForMember(x => x.Results, opt => opt.MapFrom(y => y.Results))
                .ForMember(x => x.CustomSearchResult, y => y.Ignore())
                .ForMember(x => x.ErrorMessage, y => y.Ignore());
        }

        private void CreateSearchTableResultsMappings(IMapperConfiguration cfg)
        {
            cfg.CreateMap<SearchResponse, SearchResultsViewModel>()
                .ForMember(x => x.Results, o => o.Ignore())
                .ForMember(x => x.ErrorMessage, y => y.Ignore())
                .ForMember(x => x.CustomSearchResult, opt => opt.MapFrom(o => _searchTableResultBuilder.CreateTableResult(o)));
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