namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Constants;
    using Models;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    public class VacancyController : ApiController
    {
        private readonly IGetVacancyStrategies _getVacancyStrategies;

        public VacancyController(IGetVacancyStrategies getVacancyStrategies)
        {
            _getVacancyStrategies = getVacancyStrategies;
        }

        [Route("vacancies")]
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [Route("vacancy")]
        [SwaggerOperation("GetOne")]
        public IEnumerable<string> Get(int vacancyId)
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("vacancy")]
        [SwaggerOperation("Create")]
        public IHttpActionResult Create(Vacancy vacancy)
        {
            return Ok();
        }
    }
}