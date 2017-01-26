namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Domain.Entities.Raa;
    using Apprenticeships.Web.Common.Extensions;
    using Models;
    using Strategies;

    [Authorize(Roles = Roles.Provider)]
    [RoutePrefix("employer")]
    public class EmployerController : ApiController
    {
        private readonly ILinkEmployerStrategy _linkEmployerStrategy;

        public EmployerController(ILinkEmployerStrategy linkEmployerStrategy)
        {
            _linkEmployerStrategy = linkEmployerStrategy;
        }

        /// <summary>
        /// Endpoint for linking an employer to a provider site.
        /// </summary>
        /// <param name="employerProviderSiteLinkRequest">Defines the provider site to link to as well as additional employer information. Note that you can specify the employer identifier in either the URL or the POST body</param>
        /// <param name="edsUrn">The employer's secondary identifier.</param>
        /// <returns></returns>
        [Route("{edsUrn}/link")]
        [ResponseType(typeof(EmployerProviderSiteLink))]
        [HttpPost]
        public IHttpActionResult LinkEmployer(EmployerProviderSiteLinkRequest employerProviderSiteLinkRequest, int edsUrn)
        {
            return Ok(_linkEmployerStrategy.LinkEmployer(employerProviderSiteLinkRequest, edsUrn, User.GetUkprn()));
        }
    }
}