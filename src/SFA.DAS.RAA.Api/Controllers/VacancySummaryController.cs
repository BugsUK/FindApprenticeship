namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Constants;
    using Models;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    public class VacancySummaryController : ApiController
    {
        [SwaggerOperation("GetAll")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [SwaggerOperation("Create")]
        public IHttpActionResult Create(Vacancy vacancy)
        {
            return Ok();
        }
    }
}