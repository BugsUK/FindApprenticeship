namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Providers;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    [RoutePrefix("vacancy")]
    public class VacancyManagementController : ApiController
    {
        private readonly IVacancyProvider _vacancyProvider;

        public VacancyManagementController(IVacancyProvider vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
        }

        [Route("wage")]
        [SwaggerOperation("EditVacancyWage")]
        [HttpPut]
        public Vacancy EditWage(int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            return _vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid);
        }
    }
}