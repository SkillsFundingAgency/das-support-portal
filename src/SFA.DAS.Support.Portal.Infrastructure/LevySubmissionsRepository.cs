using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HMRC.ESFA.Levy.Api.Client;
using HMRC.ESFA.Levy.Api.Types;
using HMRC.ESFA.Levy.Api.Types.Exceptions;
using SFA.DAS.NLog.Logger;
using SFA.DAS.Support.Portal.ApplicationServices;
using SFA.DAS.Support.Portal.Core.Domain.Exceptions;

namespace SFA.DAS.Support.Portal.Infrastructure
{
    public sealed class LevySubmissionsRepository : ILevySubmissionsRepository
    {
        private readonly IApprenticeshipLevyApiClient _levyApiClient;
        private readonly ILog _logger;

        public LevySubmissionsRepository(ILog logger, IApprenticeshipLevyApiClient levyApiClient)
        {
            _logger = logger;
            _levyApiClient = levyApiClient;
        }

        /// <summary>
        ///     Get levy declarations
        /// </summary>
        /// <param name="payeScheme"></param>
        /// <returns></returns>
        /// <exception cref="ApiHttpException"></exception>
        /// <exception cref="EntityNotFoundException"></exception>
        public async Task<LevyDeclarations> Get(string payeScheme)
        {
            _logger.Debug($"IApprenticeshipLevyApiClient.GetEmployerLevyDeclarations(\"{payeScheme}\");");

            try
            {
                var response = await _levyApiClient.GetEmployerLevyDeclarations(payeScheme);
                var filteredDeclarations = GetFilteredDeclarations(response.Declarations);

                response.Declarations = filteredDeclarations.OrderByDescending(x => x.SubmissionTime)
                    .ThenByDescending(x => x.Id).ToList();

                return response;
            }
            catch (ApiHttpException ex)
            {
                if (ex.HttpCode == (int) HttpStatusCode.NotFound)
                    throw new EntityNotFoundException(
                        $"IApprenticeshipLevyApiClient.GetEmployerLevyDeclarations(\"{payeScheme}\");", payeScheme, ex);

                var properties = new Dictionary<string, object>
                {
                    {"RequestCtx.StatusCode", ex.HttpCode}
                };
                _logger.Error(ex, "Issue retrieving levy declarations", properties);
                throw;
            }
        }

        private List<Declaration> GetFilteredDeclarations(List<Declaration> resultDeclarations)
        {
            return resultDeclarations.Where(x => x.SubmissionTime >= new DateTime(2017, 4, 1)).ToList();
        }
    }
}