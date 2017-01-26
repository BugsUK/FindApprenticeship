namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Strategies;

    [Authorize(Roles = Roles.Provider)]
    [RoutePrefix("vacancy")]
    public class VacancyManagementController : ApiController
    {
        private readonly IEditWageStrategy _editWageStrategy;

        public VacancyManagementController(IEditWageStrategy editWageStrategy)
        {
            _editWageStrategy = editWageStrategy;
        }

        /// <summary>
        /// Endpoint for changing the wage of a Live or Closed vacancy. The wage can only be increased from its current level.
        /// You must supply either the vacancyId, vacancyReferenceNumber or vacancyGuid as query string parameters to identify the vacancy you would like to change.
        /// The API key used must be authorized to modify the vacancy
        /// </summary>
        /// <param name="wageUpdate">Defines the changes to be made to a vacancies wage</param>
        /// <param name="vacancyId">The vacancies primary identifier</param>
        /// <param name="vacancyReferenceNumber">The vacancies secondary reference number identifier</param>
        /// <param name="vacancyGuid">The vacancies secondary GUID identifier</param>
        /// <returns></returns>
        [Route("wage")]
        [ResponseType(typeof(Vacancy))]
        [HttpPut]
        public IHttpActionResult EditWage(WageUpdate wageUpdate, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            return Ok(_editWageStrategy.EditWage(wageUpdate, vacancyId, vacancyReferenceNumber, vacancyGuid));
        }
    }
}