namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Web.Http;
    using Apprenticeships.Domain.Entities.Raa;
    using Models;
    using Swashbuckle.Swagger.Annotations;

    [Authorize(Roles = Roles.Provider)]
    public class EmployerController : ApiController
    {
        /// <summary>
        /// Endpoint for linking an employer to a provider site.
        /// You must supply either the employerId or edsUrn to identify the employer you would like to link
        /// </summary>
        /// <param name="employerProviderSiteLink">Defines the provider site to link to as well as additional employer information. Note that you can specify the employer identifier in either the URL or the POST body</param>
        /// <param name="employerId">The employer's primary identifier. You must supply this or the employer's EDSURN</param>
        /// <param name="edsUrn">The employer's secondary identifier. You must supply this or the employer's ID</param>
        /// <returns></returns>
        [Route("employer/link")]
        [SwaggerOperation("LinkEmployer")]
        public IHttpActionResult LinkEmployer(EmployerProviderSiteLink employerProviderSiteLink, int? employerId = null, int? edsUrn = null)
        {
            return Ok();
        }
    }
}