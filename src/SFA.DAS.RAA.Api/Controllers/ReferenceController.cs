namespace SFA.DAS.RAA.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using System.Web.Http.Description;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Domain.Entities.Raa.Reference;
    using Constants;

    public class ReferenceController : ApiController
    {
        private readonly IReferenceDataProvider _referenceDataProvider;

        public ReferenceController(IReferenceDataProvider referenceDataProvider)
        {
            _referenceDataProvider = referenceDataProvider;
        }

        [Route("reference/counties")]
        [ResponseType(typeof(IEnumerable<County>))]
        public IHttpActionResult GetCounties()
        {
            return Ok(_referenceDataProvider.GetCounties());
        }

        [Route("reference/county")]
        [ResponseType(typeof(County))]
        public IHttpActionResult GetCounty(int? countyId, string countyCode)
        {
            if (!countyId.HasValue && string.IsNullOrEmpty(countyCode))
            {
                throw new ArgumentException(ReferenceMessages.MissingCountyIdentifier);
            }

            var county = countyId.HasValue ? _referenceDataProvider.GetCountyById(countyId.Value) : _referenceDataProvider.GetCountyByCode(countyCode);

            if (county == null)
            {
                throw new KeyNotFoundException(ReferenceMessages.CountyNotFound);
            }

            return Ok(county);
        }

        [Route("reference/localauthorities")]
        [ResponseType(typeof(IEnumerable<LocalAuthority>))]
        public IHttpActionResult GetLocalAuthorities()
        {
            return Ok(_referenceDataProvider.GetLocalAuthorities());
        }
    }
}