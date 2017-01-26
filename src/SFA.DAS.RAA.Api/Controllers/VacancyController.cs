namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Domain.Entities.Raa.Vacancies;
    using Models;
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

        [Route("{id}")]
        [ResponseType(typeof(Vacancy))]
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return Ok(_vacancyProvider.Get(new VacancyIdentifier(id)));
        }

        [Route("reference/{reference}")]
        [ResponseType(typeof(Vacancy))]
        [HttpGet]
        public IHttpActionResult GetByReferenceNumber(string reference)
        {
            return Ok(_vacancyProvider.Get(new VacancyIdentifier(reference)));
        }

        [Route("guid/{guid}")]
        [ResponseType(typeof(Vacancy))]
        [HttpGet]
        public IHttpActionResult GetByGuid(Guid guid)
        {
            return Ok(_vacancyProvider.Get(new VacancyIdentifier(guid)));
        }
    }
}