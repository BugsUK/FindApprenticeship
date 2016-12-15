namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Models;
    using Strategies;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    [RoutePrefix("vacancy")]
    public class VacancyManagementController : ApiController
    {
        private readonly IEditWageStrategy _editWageStrategy;

        public VacancyManagementController(IEditWageStrategy editWageStrategy)
        {
            _editWageStrategy = editWageStrategy;
        }

        [Route("wage")]
        [SwaggerOperation("EditVacancyWage")]
        [HttpPut]
        public IHttpActionResult EditWage(WageUpdate wage, int? vacancyId = null, int? vacancyReferenceNumber = null, Guid? vacancyGuid = null)
        {
            return Ok(_editWageStrategy.EditWage(wage, vacancyId, vacancyReferenceNumber, vacancyGuid));
        }
    }
}