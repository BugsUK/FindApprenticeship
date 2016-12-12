namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Domain.Entities.Exceptions;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Apprenticeships.Application.Interfaces;
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
        public Vacancy Get(int vacancyId)
        {
            try
            {
                var vacancy = _getVacancyStrategies.GetVacancyById(vacancyId);
                return vacancy;
            }
            catch (CustomException ex)
            {
                if (ex.Code == ErrorCodes.ProviderVacancyAuthorisation.Failed)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "You are not authorized to view this vacancy" };
                    throw new HttpResponseException(message);
                }
                throw;
            }
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