namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories.Models;
    using Apprenticeships.Web.Common.Extensions;

    [Authorize(Roles = Roles.Provider)]
    public class VacancySummaryController : ApiController
    {
        private readonly IProviderReadRepository _providerReadRepository;
        private readonly IProviderSiteReadRepository _providerSiteReadRepository;
        private readonly IVacancySummaryRepository _vacancySummaryRepository;

        public VacancySummaryController(IProviderReadRepository providerReadRepository, IProviderSiteReadRepository providerSiteReadRepository, IVacancySummaryRepository vacancySummaryRepository)
        {
            _providerReadRepository = providerReadRepository;
            _providerSiteReadRepository = providerSiteReadRepository;
            _vacancySummaryRepository = vacancySummaryRepository;
        }

        [Route("vacancysummaries")]
        [HttpGet]
        public IEnumerable<VacancySummary> GetAll(VacanciesSummaryFilterTypes filterType = VacanciesSummaryFilterTypes.All, int page = 1)
        {
            //TODO: Strategy
            //TODO: Support employer access

            if (page < 1)
            {
                page = 1;
            }

            var provider = _providerReadRepository.GetByUkprn(User.GetUkprn());
            var providerSites = _providerSiteReadRepository.GetByProviderId(provider.ProviderId);
            var providerSiteId = providerSites.First().ProviderSiteId;

            var query = new VacancySummaryQuery
            {
                ProviderId = provider.ProviderId,
                ProviderSiteId = providerSiteId,
                OrderByField = VacancySummaryOrderByColumn.Title,
                Filter = filterType,
                PageSize = 50,
                RequestedPage = page,
                //SearchMode = vacanciesSummarySearch.SearchMode,
                //SearchString = searchString,
                Order = Order.Ascending,
                VacancyType = VacancyType.Apprenticeship
            };
            int totalRecords;
            return _vacancySummaryRepository.GetSummariesForProvider(query, out totalRecords);
        }
    }
}