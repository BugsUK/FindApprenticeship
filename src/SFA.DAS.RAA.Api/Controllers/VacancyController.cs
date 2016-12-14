namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Providers;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    public class VacancyController : ApiController
    {
        private readonly IVacancyProvider _vacancyProvider;

        public VacancyController(IVacancyProvider vacancyProvider)
        {
            _vacancyProvider = vacancyProvider;
        }

        /*[Route("vacancies")]
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }*/

        [Route("vacancy")]
        [SwaggerOperation("GetVacancy")]
        public Vacancy Get(int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            return _vacancyProvider.Get(vacancyId, vacancyReferenceNumber, vacancyGuid);
        }

        /*[HttpPost]
        [Route("vacancy")]
        [SwaggerOperation("Create")]
        public IHttpActionResult Create(VacancyApiModel vacancy)
        {
            return Ok();
        }*/
    }
}