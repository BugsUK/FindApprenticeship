using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SFA.Apprenticeship.Web.Api.Controllers
{
    using Apprenticeships.Application.Interfaces.Providers;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Web.Common.ViewModels;
    using Apprenticeships.Web.Raa.Common.Strategies;
    using Apprenticeships.Web.Raa.Common.ViewModels.ProviderUser;
    using Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

    /// <summary>
    /// CRUD vacancies for the calling provider
    /// </summary>
    public class VacancyController : ApiController
    {
        private IVacancySummaryStrategy _vacancySummaryStrategy;

        public VacancyController(IVacancySummaryStrategy vacancySummaryStrategy)
        {
            _vacancySummaryStrategy = vacancySummaryStrategy;
        }

        /// <summary>
        /// Search and filter vacancies for the calling provider
        /// </summary>
        /// <param name="filter">Filter by vacancy status</param>
        /// <param name="providerSiteId">The site Id of the provider</param>
        /// <param name="providerId">The providers database id</param>
        /// <param name="type">The type of vacancy; Apprenticeship or Traineeeship</param>
        /// <returns>A list of vacancies</returns>
        public IEnumerable<VacancySummaryViewModel> Get(VacancyType type, VacanciesSummaryFilterTypes filter, int providerId, int providerSiteId)
        {
            return _vacancySummaryStrategy.GetVacancySummaries(null, providerSiteId, providerId, type, filter, null, Order.Ascending, 25, 1).Page;
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
