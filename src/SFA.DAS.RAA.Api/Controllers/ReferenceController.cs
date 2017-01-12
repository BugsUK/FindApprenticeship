namespace SFA.DAS.RAA.Api.Controllers
{
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Domain.Entities.Raa.Reference;

    public class ReferenceController : ApiController
    {
        private readonly IReferenceDataService _referenceDataService;

        public ReferenceController(IReferenceDataService referenceDataService)
        {
            _referenceDataService = referenceDataService;
        }

        [Route("reference/counties")]
        [ResponseType(typeof(IEnumerable<County>))]
        public IHttpActionResult GetCounties()
        {
            return Ok(_referenceDataService.GetCounties());
        }

        [Route("reference/localauthorities")]
        [ResponseType(typeof(IEnumerable<LocalAuthority>))]
        public IHttpActionResult GetLocalAuthorities()
        {
            return Ok(_referenceDataService.GetLocalAuthorities());
        }
    }
}