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
        public IHttpActionResult GetCounty(int? countyId = null, string countyCode = null)
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

        [Route("reference/localauthority")]
        [ResponseType(typeof(LocalAuthority))]
        public IHttpActionResult GetLocalAuthority(int? localAuthorityId = null, string localAuthorityCode = null)
        {
            if (!localAuthorityId.HasValue && string.IsNullOrEmpty(localAuthorityCode))
            {
                throw new ArgumentException(ReferenceMessages.MissingLocalAuthorityIdentifier);
            }

            var localAuthority = localAuthorityId.HasValue ? _referenceDataProvider.GetLocalAuthorityById(localAuthorityId.Value) : _referenceDataProvider.GetLocalAuthorityByCode(localAuthorityCode);

            if (localAuthority == null)
            {
                throw new KeyNotFoundException(ReferenceMessages.LocalAuthorityNotFound);
            }

            return Ok(localAuthority);
        }

        [Route("reference/regions")]
        [ResponseType(typeof(IEnumerable<Region>))]
        public IHttpActionResult GetRegions()
        {
            return Ok(_referenceDataProvider.GetRegions());
        }

        [Route("reference/region")]
        [ResponseType(typeof(Region))]
        public IHttpActionResult GetRegion(int? regionId = null, string regionCode = null)
        {
            if (!regionId.HasValue && string.IsNullOrEmpty(regionCode))
            {
                throw new ArgumentException(ReferenceMessages.MissingRegionIdentifier);
            }

            var region = regionId.HasValue ? _referenceDataProvider.GetRegionById(regionId.Value) : _referenceDataProvider.GetRegionByCode(regionCode);

            if (region == null)
            {
                throw new KeyNotFoundException(ReferenceMessages.RegionNotFound);
            }

            return Ok(region);
        }
    }
}