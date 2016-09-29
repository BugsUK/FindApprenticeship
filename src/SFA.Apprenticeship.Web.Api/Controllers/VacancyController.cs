using System.Collections.Generic;
using System.Web.Http;

namespace SFA.Apprenticeship.Web.Api.Controllers
{
    using Apprenticeships.Application.Vacancy;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Domain.Raa.Interfaces.Repositories.Models;

    /// <summary>
    /// CRUD vacancies for the calling provider
    /// </summary>
    public class VacancyController : ApiController
    {
        private IVacancySummaryService _VacancySummaryService;

        public VacancyController(IVacancySummaryService VacancySummaryService)
        {
            _VacancySummaryService = VacancySummaryService;
        }

        /// <summary>
        /// Search and filter vacancies for the calling provider
        /// </summary>
        /// <param name="filter">Filter by vacancy status</param>
        /// <param name="providerSiteId">The site Id of the provider</param>
        /// <param name="providerId">The providers database id</param>
        /// <param name="type">The type of vacancy; Apprenticeship or Traineeeship</param>
        /// <returns>A list of vacancies</returns>
        public IEnumerable<VacancySummary> Get(VacancyType type, VacanciesSummaryFilterTypes filter, int providerId, int providerSiteId)
        {
            var query = new VacancySummaryQuery()
            {
                PageSize = 100,
                RequestedPage = 1,
                VacancyType = type,
                Filter = filter,
                OrderByField = VacancySummaryOrderByColumn.OrderByFilter,
                Order = Order.Ascending,
                ProviderId = providerId,
                ProviderSiteId = providerSiteId
            };

            int totalRecords;
            return _VacancySummaryService.GetSummariesForProvider(query, out totalRecords);
        }

        /// <summary>
        /// Retrieve a specific vacancy for the calling provider by ID
        /// </summary>
        /// <param name="id">The ID of the desired vacancy</param>
        /// <returns></returns>
        //public string Get(int id)
        //{
        //    return "value";
        //}

        /// <summary>
        /// Add a new vacancy for the calling provider
        /// </summary>
        /// <param name="value">The vacancy details</param>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
