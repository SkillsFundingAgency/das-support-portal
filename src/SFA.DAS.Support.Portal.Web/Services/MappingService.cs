using System;
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
                .ForMember(x => x.Results, opt => opt.MapFrom(y => y.Results))
                .ForMember(x => x.NewResults, y => y.Ignore())
                .ForMember(x => x.ErrorMessage, y => y.Ignore());
        }

        private MapperConfiguration Config()
        {
            return new MapperConfiguration(cfg =>
            {
                CreateEmployerUserSearchResultsMappings(cfg);
            });
        }
    }
}