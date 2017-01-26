namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Providers;

    [Authorize(Roles = Roles.Provider)]
    [RoutePrefix("vacancy")]
    public class VacancyController : ApiController
    {
        private readonly IVacancyProvider _vacancyProvider;

        public VacancyController(IVacancyProvider vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
        }

        [Route("{vacancyId}")]
        [ResponseType(typeof(Vacancy))]
        [HttpGet]
        public IHttpActionResult Get(int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            return Ok(_vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid));
        }
    }
}