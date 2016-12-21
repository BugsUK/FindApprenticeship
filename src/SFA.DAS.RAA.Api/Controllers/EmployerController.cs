namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Models;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    public class EmployerController : ApiController
    {
        [Route("employer/link")]
        [SwaggerOperation("LinkEmployer")]
        public IHttpActionResult LinkEmployer(EmployerLink employerLink, int? employerId = null, int? edsUrn = null)
        {
            return Ok();
        }
    }
}