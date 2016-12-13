namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
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
            if (!vacancyId.HasValue && !vacancyReferenceNumber.HasValue && !vacancyGuid.HasValue)
            {
                var message = new HttpResponseMessage(HttpStatusCode.BadRequest) { ReasonPhrase = "Please specify either a vacancyId, a vacancyReferenceNumber or a vacancyGuid" };
                throw new HttpResponseException(message);
            }

            try
            {
                Vacancy vacancy;

                if (vacancyId.HasValue)
                {
                    vacancy = _getVacancyStrategies.GetVacancyById(vacancyId.Value);
                }
                else if (vacancyReferenceNumber.HasValue)
                {
                    vacancy = _getVacancyStrategies.GetVacancyByReferenceNumber(vacancyReferenceNumber.Value);
                }
                else
                {
                    vacancy = _getVacancyStrategies.GetVacancyByGuid(vacancyGuid.Value);
                }

                if (vacancy == null)
                {
                    var message = new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "The requested vacancy has not been found" };
                    throw new HttpResponseException(message);
                }

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

        /*[HttpPost]
        [Route("vacancy")]
        [SwaggerOperation("Create")]
        public IHttpActionResult Create(Vacancy vacancy)
        {
            return Ok();
        }*/
    }
}