namespace SFA.Apprenticeship.Api.AvService.Providers.Version51
{
    using System.Collections.Generic;
    using System.Linq;
    using Apprenticeships.Domain.Entities.Reference;
    using Apprenticeships.Domain.Interfaces.Repositories;
    using DataContracts.Version51;
    using Mappers.Version51;

    public class ReferenceDataProvider : IReferenceDataProvider
    {
        private readonly IReferenceRepository _referenceRepository;
        private readonly ICountyMapper _mapper;
        private readonly IRegionMapper _regionMapper;
        private readonly ILocalAuthorityMapper _localAuthorityMapper;

        public ReferenceDataProvider(IReferenceRepository referenceRepository, ICountyMapper mapper, IRegionMapper regionMapper, ILocalAuthorityMapper localAuthorityMapper)
        {
            _referenceRepository = referenceRepository;
            _mapper = mapper;
            _regionMapper = regionMapper;
            _localAuthorityMapper = localAuthorityMapper;
        }

        public List<CountyData> GetCounties()
        {
            var counties = _referenceRepository.GetCounties() ?? Enumerable.Empty<County>();

            var countyDataList = _mapper.MapToCountyDatas(counties);

            return countyDataList;
        }

        public List<RegionData> GetRegions()
        {
            var regions = _referenceRepository.GetRegions() ?? Enumerable.Empty<Region>();

            var regionDataList = _regionMapper.MapToRegionDatas(regions);

            return regionDataList;
        }

        public List<LocalAuthorityData> GetLocalAuthorities()
        {
            var localAuthorities = _referenceRepository.GetLocalAuthorities() ?? Enumerable.Empty<LocalAuthority>();

            var LocalAuthoritiesDataList = _localAuthorityMapper.MapToLocalAuthorityDatas(localAuthorities);

            return LocalAuthoritiesDataList;
        }
    }
}